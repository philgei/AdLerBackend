using AdLerBackend.Application.Common.Exceptions.LMSAdapter;
using Infrastructure.Moodle;
using RichardSzalay.MockHttp;

namespace AdLerBackend.Infrastructure.UnitTests.Moodle;

public class MoodleWebApiTest
{
    private MockHttpMessageHandler _mockHttp = null!;
    private MoodleWebApi _systemUnderTest = null!;

    [SetUp]
    public void SetUp()
    {
        _mockHttp = new MockHttpMessageHandler();
        _systemUnderTest = new MoodleWebApi(_mockHttp.ToHttpClient());
    }

    [Test]
    public async Task GetMoodleToken_ValidResponse_ReturnsToken()
    {
        // Arrange
        _mockHttp.When("*")
            .Respond(
                "application/json", "{\"token\":\"testToken\"}");

        // Act
        var result = await _systemUnderTest.GetMoodleUserTokenAsync("moodleUser", "moodlePassword");

        // Assert
        Assert.That(result.moodleToken, Is.EqualTo("testToken"));
    }

    [Test]
    public async Task GetMoodleToken_InvalidResponse_ReturnsException()
    {
        // Arrange
        _mockHttp.When("*")
            .Respond(
                "application/json",
                "{\"error\":\"Invalidlogin,pleasetryagain\",\"errorcode\":\"invalidlogin\",\"stacktrace\":null,\"debuginfo\":null,\"reproductionlink\":null}");

        Assert.ThrowsAsync<LmsException>(() =>
            _systemUnderTest.GetMoodleUserTokenAsync("moodleUser", "moodlePassword"));
    }

    [Test]
    public async Task MoodleAPI_WSNotAvaliblale_ReturnsException()
    {
        // Arrange
        _mockHttp.When("*")
            .Throw(new HttpRequestException());

        var exception = Assert.ThrowsAsync<LmsException>(async () =>
            await _systemUnderTest.GetMoodleUserTokenAsync("moodleUser", "moodlePassword"));
    }

    [Test]
    public async Task MoodleAPI_WSNotReadAble_Throws()
    {
        // Arrange
        _mockHttp.When("*")
            .Respond(
                "application/json",
                "<blablabla>");

        var exception = Assert.ThrowsAsync<LmsException>(async () =>
            await _systemUnderTest.GetMoodleUserTokenAsync("moodleUser", "moodlePassword"));

        // check exception message
        Assert.That(exception!.Message, Is.EqualTo("Das Ergebnis der Moodle Web Api konnte nicht gelesen werden"));
    }

    [Test]
    public async Task GetMoodleUserData_ValidResponse_ReturnsUserData()
    {
        // Arrange
        _mockHttp.When("*")
            .Respond(
                "application/json", "{\"username\":\"test\",\"userissiteadmin\":true}");

        // Act
        var result = await _systemUnderTest.GetMoodleUserDataAsync("moodleToken");

        // Assert
        Assert.That(result.IsAdmin, Is.EqualTo(true));
    }

    [Test]
    public async Task GetMoodleUserData_InvalidResponseWrongToken_ThrowsCorrectException()
    {
        // Arrange
        _mockHttp.When("*")
            .Respond(
                "application/json",
                "{\"exception\":\"moodle_exception\",\"errorcode\":\"invalidtoken\",\"message\":\"Invalidtoken-tokennotfound\"}");

        var exception = Assert.ThrowsAsync<LmsException>(async () =>
            await _systemUnderTest.GetMoodleUserDataAsync("moodleToken"));

        // check exception message
        Assert.That(exception!.LmsErrorCode, Is.EqualTo("invalidtoken"));
    }
}