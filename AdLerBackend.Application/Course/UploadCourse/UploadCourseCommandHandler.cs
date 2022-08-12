using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses;
using AdLerBackend.Application.Moodle.Commands.GetUserData;
using Domain.Entities;
using MediatR;

namespace AdLerBackend.Application.Course.UploadCourse;

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
        if (!userInformation.isAdmin) throw new ForbiddenAccessException("You are not an admin");

        var courseInformation = _lmsBackupProcessor.GetLevelDescriptionFromBackup(request.DslFileStream);

        // TODO: Check, if course already exists
        var test = await _courseRepository.ExistsCourseForUser(userInformation.userId,
            courseInformation.LearningWorld.Identifier.Value);

        if (test) throw new Exception("Course already exists");

        var dslLocation = _fileAccess.StoreDSLFileForCourse(new StoreCourseDslDto
        {
            AuthorId = userInformation.userId,
            DslFile = request.DslFileStream,
            CourseInforamtion = courseInformation
        });

        var storedH5PFilePaths = StoreH5PFiles(courseInformation, userInformation, request.BackupFileStream);

        var courseEntity = new CourseEntity
        {
            Name = courseInformation.LearningWorld.Identifier.Value,
            AuthorId = userInformation.userId,
            H5PFilesInCourse = GetH5PLocationEntities(storedH5PFilePaths!),
            DslLocation = dslLocation
        };

        var storedEntity = await _courseRepository.CreateCourse(courseEntity);


        return true;
    }

    private List<string> StoreH5PFiles(DslFileDto courseInformation, MoodleUserDataResponse userData, Stream backupFile)
    {
        var storedH5PFilePaths = new List<string>();
        if (courseInformation.LearningWorld.LearningElements.Any(x => x.ElementType == "h5p"))
        {
            var h5PFilesInBackup = _lmsBackupProcessor.GetH5PFilesFromBackup(backupFile);
            storedH5PFilePaths = _fileAccess.StoreH5PFilesForCourse(new CourseStoreH5pDto
            {
                AuthorId = userData.userId,
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