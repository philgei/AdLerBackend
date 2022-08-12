using System.IO.Compression;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Interfaces;

namespace Infrastructure.Storage;

public class StorageService : IFileAccess
{
    public List<string>? StoreH5PFilesForCourse(CourseStoreH5pDto courseToStoreH5P)
    {
        var workingDir = Path.Join("wwwroot", "courses", courseToStoreH5P.AuthorId.ToString(),
            courseToStoreH5P.CourseInforamtion.LearningWorld.Identifier.Value, "h5p");

        var h5PFilePaths = courseToStoreH5P.H5PFiles.Select(item =>
        {
            var zipStream = new ZipArchive(item.H5PFile!, ZipArchiveMode.Read);
            zipStream.ExtractToDirectory(Path.Combine(workingDir, item.H5PFileName!));

            return Path.Combine(workingDir, item.H5PFileName!);
        }).ToList();


        return h5PFilePaths;
    }

    public string StoreDSLFileForCourse(StoreCourseDslDto courseToStoreH5P)
    {
        courseToStoreH5P.DslFile.Position = 0;
        var workingDir = Path.Join("wwwroot", "courses", courseToStoreH5P.AuthorId.ToString(),
            courseToStoreH5P.CourseInforamtion.LearningWorld.Identifier.Value);

        // save stream on courseToStore on disk
        var dslFilePath = Path.Combine(workingDir,
            courseToStoreH5P.CourseInforamtion.LearningWorld.Identifier.Value + ".json");

        // create directory if not exists
        if (!Directory.Exists(workingDir))
            Directory.CreateDirectory(workingDir);

        var fileStream = new FileStream(dslFilePath, FileMode.Create);
        courseToStoreH5P.DslFile.CopyTo(fileStream);
        fileStream.Close();
        return dslFilePath;
    }
}