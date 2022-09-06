using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.DTOs.Storage;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Infrastructure.Storage;
using AutoBogus;

#pragma warning disable CS8618

namespace AdLerBackend.Infrastructure.UnitTests.Storage;

public class StorageServiceTest
{
    private IFileSystem _fileSystem;

    [SetUp]
    public void Setup()
    {
        _fileSystem = new MockFileSystem();
    }

    [Test]
    public void StoreH5PFilesForCourse_Valid_StoresFiles()
    {
        // Arrange
        var storageService = new StorageService(_fileSystem);
        var CourseDtoFake = new CourseStoreH5PDto
        {
            AuthorId = 1,
            CourseInforamtion = AutoFaker.Generate<DslFileDto>(),
            H5PFiles = new List<H5PDto>
            {
                new()
                {
                    H5PFile = new FileStream("../../../Storage/TestFiles/FakeH5p.zip", FileMode.Open),
                    H5PFileName = "H5PName"
                }
            }
        };

        CourseDtoFake.CourseInforamtion.LearningWorld.Identifier.Value = "LearningWorldIdentifier";

        // Act
        var retunValue = storageService.StoreH5PFilesForCourse(CourseDtoFake);

        // Assert
        Assert.IsTrue(_fileSystem.Directory.Exists("wwwroot/courses/1/LearningWorldIdentifier/h5p/H5PName/Folder"));
        Assert.IsTrue(
            _fileSystem.File.Exists("wwwroot/courses/1/LearningWorldIdentifier/h5p/H5PName/Folder/FileInFolder"));
        Assert.IsTrue(_fileSystem.File.Exists("wwwroot/courses/1/LearningWorldIdentifier/h5p/H5PName/fileAtRoot.txt"));


        Assert.That(retunValue!, Has.Count.EqualTo(1));
        Assert.That(retunValue![0], Is.EqualTo("wwwroot\\courses\\1\\LearningWorldIdentifier\\h5p\\H5PName"));
    }

    [Test]
    public void StoreDslFileForCourse_Valid_StoresFile()
    {
        // Arrange
        var storageService = new StorageService(_fileSystem);
        var dto = new StoreCourseDslDto
        {
            AuthorId = 1,
            CourseInforamtion = AutoFaker.Generate<DslFileDto>(),
            DslFile = new FileStream("../../../Storage/TestFiles/DSL_Document.json", FileMode.Open)
        };

        dto.CourseInforamtion.LearningWorld.Identifier.Value = "LearningWorldIdentifier";

        // Act
        var dslLocation = storageService.StoreDslFileForCourse(dto);

        // Assert
        Assert.IsTrue(
            _fileSystem.File.Exists("wwwroot/courses/1/LearningWorldIdentifier/LearningWorldIdentifier.json"));


        Assert.That(dslLocation,
            Is.EqualTo("wwwroot\\courses\\1\\LearningWorldIdentifier\\LearningWorldIdentifier.json"));
    }

    [Test]
    public void GetFileStream_Valid_ShoudGetFileStream()
    {
        // Arrange
        var filePath = @"c:\myfile.txt";

        _fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {@"c:\myfile.txt", new MockFileData("Testing is meh.")}
        });
        var storageService = new StorageService(_fileSystem);

        // Act
        var fileStream = storageService.GetFileStream(filePath);

        // Assert
        Assert.That(fileStream, Is.Not.Null);
    }

    [Test]
    public void GetFileStream_Invalid_ShoudThrowException()
    {
        // Arrange
        var filePath = @"c:\myfile.txt";

        var storageService = new StorageService(_fileSystem);

        // Act
        // Assert
        Assert.Throws<NotFoundException>(() => storageService.GetFileStream(filePath));
    }

    [Test]
    public void StoreH5PBase_Valid_StoresH5pBase()
    {
        // Arrange
        var storageService = new StorageService(_fileSystem);

        var h5pStream = new FileStream("../../../Storage/TestFiles/FakeH5p.zip", FileMode.Open);

        // Act
        var h5pBaseLocation = storageService.StoreH5PBase(h5pStream);

        // Assert
        Assert.IsTrue(_fileSystem.Directory.Exists("wwwroot/common/h5pBase"));
        Assert.IsTrue(_fileSystem.File.Exists("wwwroot/common/h5pBase/fileAtRoot.txt"));

        Assert.That(h5pBaseLocation, Is.EqualTo("wwwroot\\common\\h5pBase"));
    }

    [Test]
    public void DeleteCourse_Valid_DeletesCourse()
    {
        // Arrange
        var dto = new CourseDeleteDto
        {
            AuthorId = 1,
            CourseName = "CourseName"
        };

        _fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {@"wwwroot\courses\1\CourseName\CourseName.json", new MockFileData("Testing is meh.")}
        });
        var storageService = new StorageService(_fileSystem);

        // Act
        storageService.DeleteCourse(dto);

        // Assert
        Assert.IsFalse(_fileSystem.Directory.Exists("wwwroot/courses/1/CourseName"));
    }
}