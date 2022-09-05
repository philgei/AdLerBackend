using System.Text;
using System.Text.Json;
using System.Xml.Serialization;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Exceptions.LMSBAckupProcessor;
using AdLerBackend.Application.Common.Interfaces;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;

namespace Infrastructure.LmsBackupProcessor;

// TODO: Implement n Task-Queue for parallel processing of the backup files
public class LmsBackupProcessor : ILmsBackupProcessor
{
    public IList<H5PDto> GetH5PFilesFromBackup(Stream backupFile)
    {
        backupFile.Position = 0;
        var filesDescriptionStream = GetFileFromTarStream(backupFile, "files.xml");

        var filesDescription = DeserializeToObject<Files>(filesDescriptionStream);

        var h5PHashes = (from file in filesDescription.File
            where file.Component == "mod_h5pactivity" && file.Filename != "."
            select new H5PWorkingStorage {H5PFileName = file.Filename, H5PContentHash = file.Contenthash}).ToList();

        // remove duplicates from h5pHashes by Contenthash since files are represented twice in the backup
        h5PHashes = h5PHashes.GroupBy(x => x.H5PContentHash).Select(x => x.First()).ToList();

        if (h5PHashes.Count == 0)
            return new List<H5PDto>();

        foreach (var h5PWorkingStorage in h5PHashes)
            h5PWorkingStorage.H5PFile = GetFileFromTarStream(backupFile,
                string.Concat("files/", h5PWorkingStorage.H5PContentHash!.AsSpan(0, 2), "/",
                    h5PWorkingStorage.H5PContentHash));


        return h5PHashes.Select(h5PFile => new H5PDto
        {
            H5PFile = h5PFile.H5PFile,
            H5PFileName = h5PFile.H5PFileName
        }).ToList();
    }

    public DslFileDto GetLevelDescriptionFromBackup(Stream dslStream)
    {
        dslStream.Position = 0;

        var retVal = JsonSerializer.Deserialize<DslFileDto>(dslStream, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new LmsBackupProcessorException("Could not deserialize DSL file");
        return retVal;
    }


    private T DeserializeToObject<T>(Stream file) where T : class
    {
        try
        {
            var xmlSerializer = new XmlSerializer(typeof(T));

            var obj = (T) xmlSerializer.Deserialize(file)! ?? throw new LmsBackupProcessorException();

            return obj;
        }
        catch (Exception e)
        {
            throw new LmsBackupProcessorException("Could not deserialize file for " + nameof(T), e);
        }
    }


    private Stream GetFileFromTarStream(Stream backupFile, string fileName)
    {
        var tarStream = GetTarInputStream(backupFile);
        while (tarStream.GetNextEntry() is { } te)
            if (te.Name == fileName)
            {
                Stream fs = new MemoryStream();
                tarStream.CopyEntryContents(fs);
                fs.Position = 0;
                return fs;
            }

        throw new NotFoundException(fileName + " not found in backup");
    }

    private static TarInputStream GetTarInputStream(Stream backupFile)
    {
        backupFile.Position = 0;
        Stream source = new GZipInputStream(backupFile);

        return new TarInputStream(source, Encoding.Default);
    }

    private class H5PWorkingStorage
    {
        public Stream? H5PFile { get; set; }
        public string? H5PFileName { get; init; }
        public string? H5PContentHash { get; init; }
    }
}