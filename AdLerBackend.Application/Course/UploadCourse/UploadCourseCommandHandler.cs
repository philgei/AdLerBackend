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

        IList<H5PDto> h5PFilesInBackup = new List<H5PDto>();
        if (courseInformation.LearningWorld.LearningElements.Any(x => x.ElementType == "h5p"))
            h5PFilesInBackup = _lmsBackupProcessor.GetH5PFilesFromBackup(request.H5PFileSteam);

        var storedH5PFilePaths = _fileAccess.StoreH5PFilesForCourse(new CourseStoreDto
        {
            AuthorId = userInformation.userId,
            CourseInforamtion = courseInformation,
            H5PFiles = h5PFilesInBackup
        });

        // TODO: Add The List of H5P Files to the Course Information and store them in the Database

        var courseEntity = new CourseEntity
        {
            Name = courseInformation.LearningWorld.Identifier.Value,
            AuthorId = userInformation.userId
            // H5PFilesInCourse = storedH5PFilePaths.Select(x => new H5PLocationEntity
            // {
            //     Path = x
            // }).ToList()
        };

        var storedEntity = await _courseRepository.CreateCourse(courseEntity);


        throw new NotImplementedException("Gagag");
    }

    private async Task<MoodleUserDataResponse> GetUserInformation(UploadCourseCommand request)
    {
        var userData = await _mediator.Send(new GetMoodleUserDataCommand
        {
            WebServiceToken = request.WebServiceToken
        });

        return userData;
    }
}