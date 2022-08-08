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
        _mockHttp.When("https://moodle.cluuub.xyz/login/*")
            .Respond(
                "application/json", "{\"token\":\"testToken\"}");

        // Act
        var result = await _systemUnderTest.GetMoodleUserTokenAsync("moodleUser", "moodlePassword");

        // Assert
        Assert.That(result.moodleToken, Is.EqualTo("testToken"));
    }
}