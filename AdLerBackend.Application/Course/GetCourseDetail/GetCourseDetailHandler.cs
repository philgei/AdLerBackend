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

    public async Task<LearningWorldDtoResponse> Handle(GetCourseDetailCommand request,
        CancellationToken cancellationToken)
    {
        //TODO: Add Error handling
        // TODO: Currently there are 2 DSL DTOs
        // Get Course from Database
        var course = await _courseRepository.GetAsync(request.CourseId);

        // Get Course from Moodle
        var searchedCourses = await _moodleService.SearchCoursesAsync(request.WebServiceToken, course.Name);

        // Get Course DSL 

        var dslFileStram = _fileAccess.GetFileStream(course.DslLocation);

        // Parse DSL File
        var dslFile = await _serialization.GetObjectFromJsonStreamAsync<LearningWorldDtoResponse>(dslFileStram);

        return new LearningWorldDtoResponse
        {
            LearningWorld = dslFile.LearningWorld
        };
    }
}