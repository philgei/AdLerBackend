using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using MediatR;

namespace AdLerBackend.Application.Course.CourseManagement.UploadH5pBase;

public class UploadH5PBaseHandler : IRequestHandler<UploadH5PBaseCommand, bool>
{
    private readonly IFileAccess _fileAccess;
    private readonly IMoodle _moodle;

    public UploadH5PBaseHandler(IMoodle moodle, IFileAccess fileAccess)
    {
        _moodle = moodle;
        _fileAccess = fileAccess;
    }

    public async Task<bool> Handle(UploadH5PBaseCommand request, CancellationToken cancellationToken)
    {
        // Check, if user is Admin
        var userData = await _moodle.GetMoodleUserDataAsync(request.WebServiceToken);
        if (userData.IsAdmin == false) throw new ForbiddenAccessException("User is not admin");

        _fileAccess.StoreH5PBase(request.H5PBaseZipStream);

        return true;
    }
}