using AdLerBackend.Application.Common.DTOs;

namespace AdLerBackend.Application.Common.Interfaces;

public interface ILmsBackupProcessor
{
    public IList<H5PDto> GetH5PFilesFromBackup(Stream backupFile);
    public DslFileDto GetLevelDescriptionFromBackup(Stream dslStream);
}