using AdLerBackend.Application.Common.DTOs;

namespace AdLerBackend.Application.Common.Interfaces;

public interface ILmsBackupProcessor
{
    public Task<IList<H5PDto>> GetH5PFilesFromBackup(Stream backupFile);
}