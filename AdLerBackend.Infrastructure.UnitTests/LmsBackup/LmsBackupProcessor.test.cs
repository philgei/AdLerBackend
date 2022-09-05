using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Exceptions.LMSBAckupProcessor;
using AdLerBackend.Infrastructure.LmsBackup;

#pragma warning disable CS8618

namespace AdLerBackend.Infrastructure.UnitTests.LmsBackup;

public class LmsBackupProcessorTest
{
    private FileStream _backupFileStream;
    private FileStream _dslFileStream;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GetH5PFilesFromBackup_Valid_GetsH5PFiles()
    {
        // Arrange
        _backupFileStream = new FileStream("../../../LmsBackup/TestFiles/backupWith1H5p/welt1.mbz", FileMode.Open);
        var systemUnderTest = new LmsBackupProcessor();

        // Act
        var result = systemUnderTest.GetH5PFilesFromBackup(_backupFileStream);

        // Assert
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].H5PFileName, Is.EqualTo("a"));
    }

    [Test]
    public void GetLevelDescriptionFromBackup_Valid_GetDslObject()
    {
        // Arrange
        _dslFileStream = new FileStream("../../../LmsBackup/TestFiles/backupWith1H5p/DSL_Document.json",
            FileMode.Open);
        var systemUnderTest = new LmsBackupProcessor();

        // Act
        var result = systemUnderTest.GetLevelDescriptionFromBackup(_dslFileStream);

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void GetH5PFilesFromBackup_ValidNoH5P_GetsEmptyList()
    {
        // Arrange
        _backupFileStream = new FileStream("../../../LmsBackup/TestFiles/backupWithNoH5p/backup.mbz", FileMode.Open);
        var systemUnderTest = new LmsBackupProcessor();

        // Act
        var result = systemUnderTest.GetH5PFilesFromBackup(_backupFileStream);

        // Assert
        Assert.That(result.Count, Is.EqualTo(0));
    }

    [Test]
    public void GetH5PFilesFromBackup_BrokenBackup_Throws()
    {
        // Arrange
        _backupFileStream =
            new FileStream("../../../LmsBackup/TestFiles/backupBrokenWith1H5P/backup.mbz", FileMode.Open);
        var systemUnderTest = new LmsBackupProcessor();

        // Act
        // Assert
        Assert.Throws<NotFoundException>(() => systemUnderTest.GetH5PFilesFromBackup(_backupFileStream));
    }

    [Test]
    public void GetH5PFilesFromBackup_BrokenBackupFilesXml_Throws()
    {
        // Arrange
        _backupFileStream =
            new FileStream("../../../LmsBackup/TestFiles/backupWithBrokenFilesContent1H5p/backup.mbz", FileMode.Open);
        var systemUnderTest = new LmsBackupProcessor();

        // Act
        // Assert
        Assert.Throws<LmsBackupProcessorException>(() => systemUnderTest.GetH5PFilesFromBackup(_backupFileStream));
    }
}