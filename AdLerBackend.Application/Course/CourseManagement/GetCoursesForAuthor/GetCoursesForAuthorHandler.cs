using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses;
using AdLerBackend.Application.Moodle.Commands.GetUserData;
using MediatR;

namespace AdLerBackend.Application.Course.CourseManagement.GetCoursesForAuthor;

public class GetCoursesForAuthorHandler : IRequestHandler<GetCoursesForAuthorCommand, GetCoursesResponse>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMediator _mediator;

    public GetCoursesForAuthorHandler(ICourseRepository courseRepository, IMediator mediator)
    {
        _courseRepository = courseRepository;
        _mediator = mediator;
    }

    public async Task<GetCoursesResponse> Handle(GetCoursesForAuthorCommand request,
        CancellationToken cancellationToken)
    {
        // Authenticate the user
        var userData = await _mediator.Send(new GetMoodleUserDataCommand
        {
            WebServiceToken = request.WebServiceToken
        });

        if (!userData.isAdmin) throw new ForbiddenAccessException("You are not an admin");

        var courses = await _courseRepository.GetAllCoursesForAuthor(request.AuthorId);

        return new GetCoursesResponse
        {
            CourseNames = courses.Select(c => c.Name).ToList()
        };
    }
}