using System.IO.Compression;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Interfaces;

namespace Infrastructure.Storage;

public class StorageService : IFileAccess
{
    public List<string> StoreH5PFilesForCourse(CourseStoreDto courseToStore)
    {
        var workingDir = Path.Join("wwwroot", "h5p", courseToStore.AuthorId.ToString(),
            courseToStore.CourseInforamtion.LearningWorld.Identifier.Value);

        var h5PFilePaths = courseToStore.H5PFiles.Select(item =>
        {
            var zipStream = new ZipArchive(item.H5PFile!, ZipArchiveMode.Read);
            zipStream.ExtractToDirectory(Path.Combine(workingDir, item.H5PFileName!));

            return Path.Combine(workingDir, item.H5PFileName!);
        }).ToList();


        return h5PFilePaths;
    }
}