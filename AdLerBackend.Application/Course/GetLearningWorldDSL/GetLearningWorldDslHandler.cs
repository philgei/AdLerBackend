using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses;
using AdLerBackend.Application.Common.Responses.Course;
using MediatR;

namespace AdLerBackend.Application.Course.GetLearningWorldDSL;

public class GetLearningWorldDslHandler : IRequestHandler<GetLearningWorldDslCommand, LearningWorldDtoResponse>
{
    private readonly IMoodle _moodleService;

    public GetLearningWorldDslHandler(IMoodle moodleService)
    {
        _moodleService = moodleService;
    }

    public async Task<LearningWorldDtoResponse> Handle(GetLearningWorldDslCommand request,
        CancellationToken cancellationToken)
    {
        var searchedCourses = await _moodleService.SearchCoursesAsync(request.WebServiceToken, request.CourseName);

        if (searchedCourses.Total == 0) throw new NotFoundException($"The Course {request.CourseName} was not found");

        var courseContents =
            await _moodleService.GetCourseContentAsync(request.WebServiceToken, searchedCourses.Courses.First().Id);

        var courseContentWithDsl =
            courseContents.FirstOrDefault(c => c.Modules.FirstOrDefault()?.Name == "DSL_Document");

        if (courseContentWithDsl == null)
            throw new NotFoundException($"The Course {request.CourseName} has no DSL_Document");


        throw new NotImplementedException("Will be implemented, when Upload of files is done");
    }
}