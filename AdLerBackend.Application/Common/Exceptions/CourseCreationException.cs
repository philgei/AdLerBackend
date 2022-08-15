using System.Runtime.Serialization;

namespace AdLerBackend.Application.Common.Exceptions;

public class CourseCreationException : Exception
{
    public CourseCreationException()
    {
    }

    protected CourseCreationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public CourseCreationException(string? message) : base(message)
    {
    }

    public CourseCreationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}