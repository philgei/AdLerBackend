namespace AdLerBackend.Application.Common.Exceptions;
// Exclude from code coverage because it's a wrapper for a simple exception 

public class NotFoundException : Exception
{
    public NotFoundException()
    {
    }


    public NotFoundException(string? message) : base(message)
    {
    }

    public NotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}