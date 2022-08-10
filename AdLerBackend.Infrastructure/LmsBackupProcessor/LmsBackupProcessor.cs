using System.Text;
using System.Xml.Serialization;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;

namespace Infrastructure.LmsBackupProcessor;

// TODO: Implement n Task-Queue for parallel processing of the backup files
public class LmsBackupProcessor : ILmsBackupProcessor
{
    public IList<H5PDto> GetH5PFilesFromBackup(Stream backupFile)
    {
        var filesDescriptionStream = getFileFromTarStream(backupFile, "files.xml");

        var filesDescription = DeserializeToObject<Files>(filesDescriptionStream);

        List<string> h5pHashes = new();
        foreach (var file in filesDescription.File)
            if (file.Mimetype == "application/zip.h5p")
                h5pHashes.Add(file.Contenthash);

        // remove duplicates from h5pHashes since files are represented twice in the backup
        h5pHashes = h5pHashes.Distinct().ToList();
        if (h5pHashes.Count == 0)
            return new List<H5PDto>();

        var h5PFiles = h5pHashes
            .Select(h5pHash => getFileFromTarStream(backupFile, "files/" + h5pHash.Substring(0, 2) + "/" + h5pHash))
            .ToList();

        return h5PFiles.Select(h5PFile => new H5PDto
        {
            H5PFile = h5PFile
        }).ToList();
    }


    private T DeserializeToObject<T>(Stream file) where T : class
    {
        try
        {
            var ser = new XmlSerializer(typeof(T));

            using (var sr = new StreamReader(file))
            {
                return (T) ser.Deserialize(sr);
            }
        }
        catch (Exception e)
        {
            throw new Exception("Could not deserialize file for " + nameof(T), e);
        }
    }


    private Stream getFileFromTarStream(Stream backupFile, string fileName)
    {
        var tarStream = getTarInputStream(backupFile);
        TarEntry te;
        while ((te = tarStream.GetNextEntry()) != null)
            if (te.Name == fileName)
            {
                Stream fs = new MemoryStream();
                tarStream.CopyEntryContents(fs);
                fs.Position = 0;
                return fs;
            }

        throw new NotFoundException(fileName + " not found in backup");
    }

    private TarInputStream getTarInputStream(Stream backupFile)
    {
        backupFile.Position = 0;
        Stream source = new GZipInputStream(backupFile);

        return new TarInputStream(source, Encoding.Default);
    }
}