using System.IO.Abstractions;
using System.IO.Compression;
using AdLerBackend.Application.Common.DTOs.Storage;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;

namespace AdLerBackend.Infrastructure.Storage;

public class StorageService : IFileAccess
{
    private readonly IFileSystem _fileSystem;

    public StorageService(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public List<string>? StoreH5PFilesForCourse(CourseStoreH5PDto courseToStoreH5P)
    {
        var workingDir = _fileSystem.Path.Join("wwwroot", "courses", courseToStoreH5P.AuthorId.ToString(),
            courseToStoreH5P.CourseInforamtion.LearningWorld.Identifier.Value, "h5p");

        var h5PFilePaths = courseToStoreH5P.H5PFiles.Select(item =>
        {
            var zipStream = new ZipArchive(item.H5PFile!, ZipArchiveMode.Read);

            var directory = _fileSystem.Path.Combine(workingDir, item.H5PFileName!);

            ExtractToDirectory(zipStream, directory);

            return directory;
        }).ToList();


        return h5PFilePaths;
    }

    public string StoreDslFileForCourse(StoreCourseDslDto courseToStoreH5P)
    {
        courseToStoreH5P.DslFile.Position = 0;
        var workingDir = _fileSystem.Path.Join("wwwroot", "courses", courseToStoreH5P.AuthorId.ToString(),
            courseToStoreH5P.CourseInforamtion.LearningWorld.Identifier.Value);

        // save stream on courseToStore on disk
        var dslFilePath = _fileSystem.Path.Combine(workingDir,
            courseToStoreH5P.CourseInforamtion.LearningWorld.Identifier.Value + ".json");

        // create directory if not exists
        if (!_fileSystem.Directory.Exists(workingDir))
            _fileSystem.Directory.CreateDirectory(workingDir);

        var fileStream = _fileSystem.FileStream.Create(dslFilePath, FileMode.Create);
        courseToStoreH5P.DslFile.CopyTo(fileStream);
        fileStream.Close();
        return dslFilePath;
    }

    public Stream GetFileStream(string filePath)
    {
        try
        {
            var fileStream = _fileSystem.FileStream.Create(filePath, FileMode.Open);
            return fileStream;
        }
        catch (Exception e)
        {
            throw new NotFoundException("File not found: " + filePath, e);
        }
    }

    public string StoreH5PBase(Stream fileStream)
    {
        var workingPath = _fileSystem.Path.Combine("wwwroot", "common", "h5pBase");

        _fileSystem.Directory.CreateDirectory(workingPath);
        var zipStream = new ZipArchive(fileStream, ZipArchiveMode.Read);

        ExtractToDirectory(zipStream, workingPath);

        return workingPath;
    }

    public bool DeleteCourse(CourseDeleteDto courseToDelete)
    {
        var workingDir = _fileSystem.Path.Join("wwwroot", "courses", courseToDelete.AuthorId.ToString(),
            courseToDelete.CourseName);

        _fileSystem.Directory.Delete(workingDir, true);
        return true;
    }

    private void ExtractToDirectory(ZipArchive zipStream, string workingPath)
    {
        foreach (var entry in zipStream.Entries)
        {
            if (_fileSystem.Path.EndsInDirectorySeparator(entry.FullName)) continue;
            using var inputStream = entry.Open();

            var filePath = _fileSystem.Path.Join(workingPath, entry.FullName);
            var dirName = _fileSystem.Path.GetDirectoryName(filePath);

            _fileSystem.Directory.CreateDirectory(dirName);
            using var unpackedFile = _fileSystem.File.OpenWrite(filePath);
            inputStream.CopyTo(unpackedFile);
            unpackedFile.Flush();
        }
    }
}