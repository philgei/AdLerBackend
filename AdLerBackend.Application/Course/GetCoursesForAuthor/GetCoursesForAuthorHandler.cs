using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses;
using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Moodle.GetUserData;
using MediatR;

namespace AdLerBackend.Application.Course.GetCoursesForAuthor;

public class GetCoursesForAuthorHandler : IRequestHandler<GetCoursesForAuthorCommand, GetCourseOverviewResponse>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMediator _mediator;

    public GetCoursesForAuthorHandler(ICourseRepository courseRepository, IMediator mediator)
    {
        _courseRepository = courseRepository;
        _mediator = mediator;
    }

    public async Task<GetCourseOverviewResponse> Handle(GetCoursesForAuthorCommand request,
        CancellationToken cancellationToken)
    {
        // Authenticate the user
        var userData = await _mediator.Send(new GetMoodleUserDataCommand
        {
            WebServiceToken = request.WebServiceToken
        });

        if (!userData.isAdmin) throw new ForbiddenAccessException("You are not an admin");

        var courses = await _courseRepository.GetAllCoursesForAuthor(request.AuthorId);

        return new GetCourseOverviewResponse
        {
            CourseNames = courses.Select(c => c.Name).ToList()
        };
    }
}