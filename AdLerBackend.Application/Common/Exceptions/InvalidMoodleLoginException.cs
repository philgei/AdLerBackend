namespace AdLerBackend.Application.Common.Exceptions;

public class InvalidMoodleLoginException : Exception
{
    public InvalidMoodleLoginException()
    {
    }

    public InvalidMoodleLoginException(string message)
        : base(message)
    {
    }

    public InvalidMoodleLoginException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}