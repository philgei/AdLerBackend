using AdLerBackend.Application.Common.Interfaces;
using MediatR;

namespace AdLerBackend.Application.Course.UploadCourse;

public class UploadCourseCommandHandler : IRequestHandler<UploadCourseCommand, bool>
{
    private readonly ILmsBackupProcessor _lmsBackupProcessor;

    public UploadCourseCommandHandler(ILmsBackupProcessor lmsBackupProcessor)
    {
        _lmsBackupProcessor = lmsBackupProcessor;
    }

    public Task<bool> Handle(UploadCourseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var h5pFilesInBackup = _lmsBackupProcessor.GetH5PFilesFromBackup(request.Content);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        throw new NotImplementedException("Gagag");
    }
}