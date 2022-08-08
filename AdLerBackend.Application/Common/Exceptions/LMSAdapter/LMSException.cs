namespace AdLerBackend.Application.Common.Exceptions.LMSAdapter;

public class LmsException : Exception
{
    public string LmsErrorCode { get; set; }
}