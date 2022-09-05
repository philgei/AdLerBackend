using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.DTOs.Storage;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses;
using AdLerBackend.Application.Moodle.GetUserData;
using AdLerBackend.Domain.Entities;
using MediatR;

namespace AdLerBackend.Application.Course.CourseManagement.UploadCourse;

public class UploadCourseCommandHandler : IRequestHandler<UploadCourseCommand, bool>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IFileAccess _fileAccess;
    private readonly ILmsBackupProcessor _lmsBackupProcessor;
    private readonly IMediator _mediator;

    public UploadCourseCommandHandler(ILmsBackupProcessor lmsBackupProcessor, IMediator mediator,
        IFileAccess fileAccess, ICourseRepository courseRepository)
    {
        _lmsBackupProcessor = lmsBackupProcessor;
        _mediator = mediator;
        _fileAccess = fileAccess;
        _courseRepository = courseRepository;
    }

    public async Task<bool> Handle(UploadCourseCommand request, CancellationToken cancellationToken)
    {
        var userInformation = await GetUserInformation(request);
        if (!userInformation.IsAdmin) throw new ForbiddenAccessException("You are not an admin");

        var courseInformation = _lmsBackupProcessor.GetLevelDescriptionFromBackup(request.DslFileStream);


        var existsCourseForUser = await _courseRepository.ExistsCourseForAuthor(userInformation.UserId,
            courseInformation.LearningWorld.Identifier.Value);

        if (existsCourseForUser) throw new CourseCreationException("Course already exists in Database");

        var dslLocation = _fileAccess.StoreDslFileForCourse(new StoreCourseDslDto
        {
            AuthorId = userInformation.UserId,
            DslFile = request.DslFileStream,
            CourseInforamtion = courseInformation
        });

        var storedH5PFilePaths = StoreH5PFiles(courseInformation, userInformation, request.BackupFileStream);

        var courseEntity = new CourseEntity
        {
            Name = courseInformation.LearningWorld.Identifier.Value,
            AuthorId = userInformation.UserId,
            H5PFilesInCourse = GetH5PLocationEntities(storedH5PFilePaths!),
            DslLocation = dslLocation
        };

        await _courseRepository.AddAsync(courseEntity);

        return true;
    }

    private List<string> StoreH5PFiles(DslFileDto courseInformation, MoodleUserDataResponse userData, Stream backupFile)
    {
        var storedH5PFilePaths = new List<string>();
        if (courseInformation.LearningWorld.LearningElements.Any(x => x.ElementType == "h5p"))
        {
            var h5PFilesInBackup = _lmsBackupProcessor.GetH5PFilesFromBackup(backupFile);
            storedH5PFilePaths = _fileAccess.StoreH5PFilesForCourse(new CourseStoreH5PDto
            {
                AuthorId = userData.UserId,
                CourseInforamtion = courseInformation,
                H5PFiles = h5PFilesInBackup
            });
        }

        return storedH5PFilePaths!;
    }

    private async Task<MoodleUserDataResponse> GetUserInformation(UploadCourseCommand request)
    {
        var userData = await _mediator.Send(new GetMoodleUserDataCommand
        {
            WebServiceToken = request.WebServiceToken
        });

        return userData;
    }

    private List<H5PLocationEntity> GetH5PLocationEntities(List<string> storedH5PFilePaths)
    {
        if (storedH5PFilePaths.Count == 0) return new List<H5PLocationEntity>();
        return storedH5PFilePaths.Select(x => new H5PLocationEntity
        {
            Path = x
        }).ToList();
    }
}