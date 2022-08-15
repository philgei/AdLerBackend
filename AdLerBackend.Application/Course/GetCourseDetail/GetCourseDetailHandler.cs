using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.Course;
using MediatR;

namespace AdLerBackend.Application.Course.GetLearningWorldDSL;

public class GetCourseDetailHandler : IRequestHandler<GetCourseDetailCommand, LearningWorldDtoResponse>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IFileAccess _fileAccess;
    private readonly IMoodle _moodleService;
    private readonly ISerialization _serialization;

    public GetCourseDetailHandler(IMoodle moodleService, ICourseRepository courseRepository, IFileAccess fileAccess,
        ISerialization serialization)
    {
        _moodleService = moodleService;
        _courseRepository = courseRepository;
        _fileAccess = fileAccess;
        _serialization = serialization;
    }

    /// <summary>
    ///     Get the course detail for a given course id
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotFoundException">Throws, if a course is not found either on the disc, the database or the moodle api</exception>
    public async Task<LearningWorldDtoResponse> Handle(GetCourseDetailCommand request,
        CancellationToken cancellationToken)
    {
        // Get Course from Database
        var course = await _courseRepository.GetAsync(request.CourseId);
        if (course == null)
            throw new NotFoundException("Course with the Id " + request.CourseId + " not found");

        // Get Course from Moodle
        var searchedCourses = await _moodleService.SearchCoursesAsync(request.WebServiceToken, course.Name);
        if (searchedCourses.Total == 0)
            throw new NotFoundException("Course with the Id " + request.CourseId + " not found on Moodle");

        // Get Course DSL 
        await using var fileStream = _fileAccess.GetFileStream(course.DslLocation);

        // Parse DSL File
        var dslFile = await _serialization.GetObjectFromJsonStreamAsync<LearningWorldDtoResponse>(fileStream);


        return new LearningWorldDtoResponse
        {
            LearningWorld = dslFile.LearningWorld
        };
    }
}