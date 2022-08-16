using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Course.GetLearningWorldDSL;
using MediatR;

namespace AdLerBackend.Application.Course.GetCourseDetail;

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

        // Hydrate H5P Files in dsl file with the actual H5P File paths
        foreach (var h5PLocationEntity in course.H5PFilesInCourse)
        {
            var h5PFile = dslFile.LearningWorld.LearningElements.FirstOrDefault(x =>
                x.ElementType == "h5p" && x.Identifier.Value == Path.GetFileName(h5PLocationEntity.Path));

            if (h5PFile != null)
            {
                h5PFile.MetaData ??= new List<MetaData>();
                h5PFile.MetaData.Add(new MetaData
                {
                    Key = "h5pFileName",
                    Value = h5PLocationEntity.Path
                });
            }
            else
            {
                throw new NotFoundException("H5P File with the Id " + h5PLocationEntity.Path + " not found");
            }
        }


        return new LearningWorldDtoResponse
        {
            LearningWorld = dslFile.LearningWorld
        };
    }
}