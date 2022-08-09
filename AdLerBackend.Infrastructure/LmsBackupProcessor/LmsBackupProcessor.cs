using System.Xml.Serialization;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;

namespace Infrastructure.LmsBackupProcessor;

public class LmsBackupProcessor : ILmsBackupProcessor
{
    public Task<IList<H5PDto>> GetH5PFilesFromBackup(Stream backupFile)
    {
        var filesDescription = GetFilesDescription(backupFile);

        throw new NotImplementedException();
    }

    private Files GetFilesDescription(Stream backupFile)
    {
        var fileDescriptionString = getFileDescriptionString(backupFile);

        var serializer = new XmlSerializer(typeof(Files));
        using (var reader = new StringReader(fileDescriptionString))
        {
#pragma warning disable CS8600
            var retVal = (Files) serializer.Deserialize(reader);
#pragma warning restore CS8600
            if (retVal != null) return retVal;
            throw new Exception("Could not Parse File Description");
        }
    }

    private string getFileDescriptionString(Stream backupFile)
    {
        using (Stream source = new GZipInputStream(backupFile))
        {
            using (var tarStream = new TarInputStream(source))
            {
                TarEntry te;
                while ((te = tarStream.GetNextEntry()) != null)
                    if (te.Name == "files.xml")
                        using (Stream fs = new MemoryStream())
                        {
                            tarStream.CopyEntryContents(fs);
                            fs.Position = 0;
                            var retVal = new StreamReader(fs).ReadToEnd();
                            return retVal;
                        }

                throw new NotFoundException("files.xml not found in backup");
            }
        }
    }
}