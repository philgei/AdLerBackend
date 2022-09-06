namespace AdLerBackend.Application.Common.Exceptions;

public class CourseCreationException : Exception
{
    public CourseCreationException(string? message) : base(message)
    {
    }
}