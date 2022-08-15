using System.Runtime.Serialization;

namespace AdLerBackend.Application.Common.Exceptions.LMSBAckupProcessor;

public class LmsBackupProcessorException : Exception
{
    public LmsBackupProcessorException()
    {
    }

    protected LmsBackupProcessorException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public LmsBackupProcessorException(string? message) : base(message)
    {
    }

    public LmsBackupProcessorException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}