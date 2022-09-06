namespace AdLerBackend.Application.Common.Exceptions.LMSAdapter;

public class LmsException : Exception
{
    public LmsException()
    {
    }


    public LmsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public string LmsErrorCode { get; set; } = "";
}