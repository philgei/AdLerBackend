using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.DTOs.Storage;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses;
using AdLerBackend.Application.Course.CourseManagement.UploadCourse;
using AdLerBackend.Application.Moodle.GetUserData;
using AdLerBackend.Domain.Entities;
using AutoBogus;
using MediatR;
using NSubstitute;

#pragma warning disable CS8618

namespace AdLerBackend.Application.UnitTests.Course.CourseManagement.UploadCourse;

public class UploadCourseTest
{
    private ICourseRepository _courseRepository;
    private IFileAccess _fileAccess;
    private ILmsBackupProcessor _lmsBackupProcessor;
    private IMediator _mediator;

    [SetUp]
    public void Setup()
    {
        _lmsBackupProcessor = Substitute.For<ILmsBackupProcessor>();
        _mediator = Substitute.For<IMediator>();
        _fileAccess = Substitute.For<IFileAccess>();
        _courseRepository = Substitute.For<ICourseRepository>();
    }

    [Test]
    public async Task Handle_Valid_TriggersUpload()
    {
        // Arrange
        var systemUnderTest =
            new UploadCourseCommandHandler(_lmsBackupProcessor, _mediator, _fileAccess, _courseRepository);

        _mediator.Send(Arg.Any<GetMoodleUserDataCommand>()).Returns(new MoodleUserDataResponse
        {
            IsAdmin = true
        });

        var fakedDsl = AutoFaker.Generate<DslFileDto>();
        fakedDsl.LearningWorld.LearningElements[0] = new LearningElement
        {
            Id = 13337,
            ElementType = "h5p"
        };

        _lmsBackupProcessor.GetLevelDescriptionFromBackup(Arg.Any<Stream>()).Returns(fakedDsl);

        _courseRepository.ExistsCourseForAuthor(Arg.Any<int>(), Arg.Any<string>()).Returns(false);

        _fileAccess.StoreDslFileForCourse(Arg.Any<StoreCourseDslDto>()).Returns("testDSlPath");

        _lmsBackupProcessor.GetH5PFilesFromBackup(Arg.Any<Stream>()).Returns(new List<H5PDto>

        {
            new()
            {
                H5PFile = new MemoryStream(),
                H5PFileName = "FileName"
            }
        });

        _fileAccess.StoreH5PFilesForCourse(Arg.Any<CourseStoreH5PDto>()).Returns(new List<string>
        {
            "path1"
        });


        // Act
        await systemUnderTest.Handle(new UploadCourseCommand
        {
            BackupFileStream = new MemoryStream(),
            DslFileStream = new MemoryStream(),
            WebServiceToken = "testToken"
        }, CancellationToken.None);

        // Assert that AddAsync has been called with the correct entity
        await _courseRepository.Received(1)
            .AddAsync(Arg.Is<CourseEntity>(x => x.Name == fakedDsl.LearningWorld.Identifier.Value));
    }

    [Test]
    public Task Handle_UnauthorizedUser_Throws()
    {
        // Arrange
        var systemUnderTest =
            new UploadCourseCommandHandler(_lmsBackupProcessor, _mediator, _fileAccess, _courseRepository);

        _mediator.Send(Arg.Any<GetMoodleUserDataCommand>()).Returns(new MoodleUserDataResponse
        {
            IsAdmin = false
        });

        // Act
        // Assert
        Assert.ThrowsAsync<ForbiddenAccessException>(async () =>
            await systemUnderTest.Handle(new UploadCourseCommand
            {
                BackupFileStream = new MemoryStream(),
                DslFileStream = new MemoryStream(),
                WebServiceToken = "testToken"
            }, CancellationToken.None));
        return Task.CompletedTask;
    }

    [Test]
    public Task Handle_CourseExists_ThrowsException()
    {
        // Arrange
        var systemUnderTest =
            new UploadCourseCommandHandler(_lmsBackupProcessor, _mediator, _fileAccess, _courseRepository);

        _mediator.Send(Arg.Any<GetMoodleUserDataCommand>()).Returns(new MoodleUserDataResponse
        {
            IsAdmin = true
        });

        var fakedDsl = AutoFaker.Generate<DslFileDto>();
        fakedDsl.LearningWorld.LearningElements[0] = new LearningElement
        {
            Id = 13337,
            ElementType = "h5p"
        };

        _lmsBackupProcessor.GetLevelDescriptionFromBackup(Arg.Any<Stream>()).Returns(fakedDsl);

        _courseRepository.ExistsCourseForAuthor(Arg.Any<int>(), Arg.Any<string>()).Returns(true);

        // Act
        // Assert
        Assert.ThrowsAsync<CourseCreationException>(async () =>
            await systemUnderTest.Handle(new UploadCourseCommand
            {
                BackupFileStream = new MemoryStream(),
                DslFileStream = new MemoryStream(),
                WebServiceToken = "testToken"
            }, CancellationToken.None));
        return Task.CompletedTask;
    }


    [Test]
    public async Task Handle_ValidNoH5p_TriggersUpload()
    {
        // Arrange
        var systemUnderTest =
            new UploadCourseCommandHandler(_lmsBackupProcessor, _mediator, _fileAccess, _courseRepository);

        _mediator.Send(Arg.Any<GetMoodleUserDataCommand>()).Returns(new MoodleUserDataResponse
        {
            IsAdmin = true
        });

        var fakedDsl = AutoFaker.Generate<DslFileDto>();

        _lmsBackupProcessor.GetLevelDescriptionFromBackup(Arg.Any<Stream>()).Returns(fakedDsl);

        _courseRepository.ExistsCourseForAuthor(Arg.Any<int>(), Arg.Any<string>()).Returns(false);

        _fileAccess.StoreDslFileForCourse(Arg.Any<StoreCourseDslDto>()).Returns("testDSlPath");

        _lmsBackupProcessor.GetH5PFilesFromBackup(Arg.Any<Stream>()).Returns(new List<H5PDto>());

        _fileAccess.StoreH5PFilesForCourse(Arg.Any<CourseStoreH5PDto>()).Returns(new List<string>
        {
            "path1"
        });


        // Act
        var result = await systemUnderTest.Handle(new UploadCourseCommand
        {
            BackupFileStream = new MemoryStream(),
            DslFileStream = new MemoryStream(),
            WebServiceToken = "testToken"
        }, CancellationToken.None);

        // Assert that AddAsync has been called with the correct entity
        await _courseRepository.Received(1)
            .AddAsync(Arg.Is<CourseEntity>(x => x.Name == fakedDsl.LearningWorld.Identifier.Value));
    }
}