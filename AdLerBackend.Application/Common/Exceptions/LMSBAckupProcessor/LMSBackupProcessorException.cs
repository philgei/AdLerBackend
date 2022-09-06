using System.Diagnostics.CodeAnalysis;

namespace AdLerBackend.Application.Common.Exceptions.LMSBAckupProcessor;

// Exclude this file from test coverage because it is a simple wrapper around an exception type.
[ExcludeFromCodeCoverage]
public class LmsBackupProcessorException : Exception
{
    public LmsBackupProcessorException()
    {
    }

    public LmsBackupProcessorException(string? message) : base(message)
    {
    }

    public LmsBackupProcessorException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}