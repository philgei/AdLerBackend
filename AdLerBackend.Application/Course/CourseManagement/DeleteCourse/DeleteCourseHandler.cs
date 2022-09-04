using AdLerBackend.Application.Common.DTOs.Storage;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using MediatR;

namespace AdLerBackend.Application.Course.CourseManagement.DeleteCourse;

public class DeleteCourseHandler : IRequestHandler<DeleteCourseCommand, bool>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IFileAccess _fileAccess;
    private readonly IMoodle _moodle;

    public DeleteCourseHandler(IMoodle moodle, ICourseRepository courseRepository, IFileAccess fileAccess)
    {
        _moodle = moodle;
        _courseRepository = courseRepository;
        _fileAccess = fileAccess;
    }

    public async Task<bool> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        // check if user is Admin
        if (!await _moodle.IsMoodleAdminAsync(request.WebServiceToken))
            throw new ForbiddenAccessException("User is not Admin");

        // get course from db
        var course = await _courseRepository.GetAsync(request.CourseId);

        // Delete from file System
        _fileAccess.DeleteCourse(new CourseDeleteDto
        {
            AuthorId = course.AuthorId,
            CourseName = course.Name
        });

        // Delete from db
        await _courseRepository.DeleteAsync(request.CourseId);

        return true;
    }
}