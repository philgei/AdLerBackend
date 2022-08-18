using System.Runtime.Serialization;

namespace AdLerBackend.Application.Common.Exceptions.LMSAdapter;

public class LmsException : Exception
{
    public LmsException()
    {
    }

    protected LmsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public LmsException(string? message) : base(message)
    {
    }

    public LmsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public string LmsErrorCode { get; set; } = "";
}