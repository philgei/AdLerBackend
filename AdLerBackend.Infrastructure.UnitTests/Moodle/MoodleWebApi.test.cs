using System.Text.Json;
using AdLerBackend.Application.Common.Exceptions.LMSAdapter;
using AdLerBackend.Application.Common.Responses;
using Infrastructure.Moodle;
using NSubstitute;
using NSubstitute.Extensions;
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
    public async Task UsUserAdmin_Valid_HeIs()
    {
        // Arrange
        var test = Substitute.ForPartsOf<MoodleWebApi>(_mockHttp.ToHttpClient());
        test.Configure().GetMoodleUserDataAsync("token").Returns(new MoodleUserDataResponse
        {
            IsAdmin = true,
            UserId = 1,
            MoodleUserName = "testUser"
        });

        // Act
        var result = await test.IsMoodleAdminAsync("token");

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public async Task GetCourseForUser_Valid_GetCoursesForUser()
    {
        // Arrange
        _mockHttp.When("*").Respond("application/json",
            "{\"total\":6,\"courses\":[{\"id\":315,\"fullname\":\"MetrikenTeil1Lernwelt\",\"displayname\":\"MetrikenTeil1Lernwelt\",\"shortname\":\"MetrikenTeil1Lernwelt\",\"categoryid\":10,\"categoryname\":\"PhilippsTesträume\",\"sortorder\":0,\"summary\":\"\",\"summaryformat\":1,\"summaryfiles\":[],\"overviewfiles\":[],\"showactivitydates\":true,\"showcompletionconditions\":true,\"contacts\":[],\"enrollmentmethods\":[\"guest\"]},{\"id\":320,\"fullname\":\"Metriken_Teil2_Lernwelt\",\"displayname\":\"Metriken_Teil2_Lernwelt\",\"shortname\":\"Metriken_Teil2_Lernwelt\",\"categoryid\":10,\"categoryname\":\"PhilippsTesträume\",\"sortorder\":0,\"summary\":\"\",\"summaryformat\":1,\"summaryfiles\":[],\"overviewfiles\":[],\"showactivitydates\":true,\"showcompletionconditions\":true,\"contacts\":[],\"enrollmentmethods\":[\"guest\"]},{\"id\":321,\"fullname\":\"Metriken_Teil3_Lernwelt\",\"displayname\":\"Metriken_Teil3_Lernwelt\",\"shortname\":\"Metriken_Teil3_Lernwelt\",\"categoryid\":10,\"categoryname\":\"PhilippsTesträume\",\"sortorder\":0,\"summary\":\"\",\"summaryformat\":1,\"summaryfiles\":[],\"overviewfiles\":[],\"showactivitydates\":true,\"showcompletionconditions\":true,\"contacts\":[],\"enrollmentmethods\":[\"guest\"]},{\"id\":329,\"fullname\":\"MetrikenTeil3Lernwelt\",\"displayname\":\"MetrikenTeil3Lernwelt\",\"shortname\":\"MetrikenTeil3Lernwelt\",\"categoryid\":5,\"categoryname\":\"DimisTests\",\"sortorder\":0,\"summary\":\"\",\"summaryformat\":1,\"summaryfiles\":[],\"overviewfiles\":[],\"showactivitydates\":true,\"showcompletionconditions\":true,\"contacts\":[],\"enrollmentmethods\":[\"guest\"]},{\"id\":330,\"fullname\":\"MetrikenTeil3LernweltMitUuidcopy1\",\"displayname\":\"MetrikenTeil3LernweltMitUuidcopy1\",\"shortname\":\"MetrikenTeil3Lernwelt_1\",\"categoryid\":5,\"categoryname\":\"DimisTests\",\"sortorder\":0,\"summary\":\"\",\"summaryformat\":1,\"summaryfiles\":[],\"overviewfiles\":[],\"showactivitydates\":true,\"showcompletionconditions\":true,\"contacts\":[],\"enrollmentmethods\":[\"guest\"]},{\"id\":286,\"fullname\":\"LernweltMetriken\",\"displayname\":\"LernweltMetriken\",\"shortname\":\"LernweltMetriken\",\"categoryid\":5,\"categoryname\":\"DimisTests\",\"sortorder\":40004,\"summary\":\"\",\"summaryformat\":1,\"summaryfiles\":[],\"overviewfiles\":[],\"showactivitydates\":true,\"showcompletionconditions\":true,\"contacts\":[],\"enrollmentmethods\":[\"guest\"]}],\"warnings\":[]}");

        // Act
        var result = await _systemUnderTest.GetCoursesForUserAsync("token");

        // Assert
        Assert.That(result.Total, Is.EqualTo(6));
    }

    [Test]
    public async Task GetCourseContent_Valid_ReturnCourseContent()
    {
        // Arrange
        _mockHttp.When("*")
            .Respond("application/json",
                "[{\"id\":21022,\"name\":\"General\",\"visible\":1,\"summary\":\"\",\"summaryformat\":1,\"section\":0,\"hiddenbynumsections\":0,\"uservisible\":true,\"modules\":[]},{\"id\":21020,\"name\":\"Tile1\",\"visible\":1,\"summary\":\"\",\"summaryformat\":1,\"section\":1,\"hiddenbynumsections\":0,\"uservisible\":true,\"modules\":[{\"id\":685,\"url\":\"https://moodle.cluuub.xyz/mod/h5pactivity/view.php?id=685\",\"name\":\"Element_1\",\"instance\":323,\"contextid\":1146,\"visible\":1,\"uservisible\":true,\"visibleoncoursepage\":1,\"modicon\":\"https://moodle.cluuub.xyz/theme/image.php/boost/h5pactivity/1651754230/icon\",\"modname\":\"h5pactivity\",\"modplural\":\"H5P\",\"availability\":null,\"indent\":0,\"onclick\":\"\",\"afterlink\":null,\"customdata\":\"\\\"\\\"\",\"noviewlink\":false,\"completion\":1,\"completiondata\":{\"state\":0,\"timecompleted\":0,\"overrideby\":null,\"valueused\":false,\"hascompletion\":true,\"isautomatic\":false,\"istrackeduser\":false,\"uservisible\":true,\"details\":[]},\"dates\":[]}]},{\"id\":21021,\"name\":\"Tile2\",\"visible\":1,\"summary\":\"\",\"summaryformat\":1,\"section\":2,\"hiddenbynumsections\":0,\"uservisible\":true,\"modules\":[{\"id\":686,\"url\":\"https://moodle.cluuub.xyz/mod/h5pactivity/view.php?id=686\",\"name\":\"Element_2\",\"instance\":324,\"contextid\":1147,\"visible\":1,\"uservisible\":true,\"visibleoncoursepage\":1,\"modicon\":\"https://moodle.cluuub.xyz/theme/image.php/boost/h5pactivity/1651754230/icon\",\"modname\":\"h5pactivity\",\"modplural\":\"H5P\",\"availability\":null,\"indent\":0,\"onclick\":\"\",\"afterlink\":null,\"customdata\":\"\\\"\\\"\",\"noviewlink\":false,\"completion\":1,\"completiondata\":{\"state\":0,\"timecompleted\":0,\"overrideby\":null,\"valueused\":false,\"hascompletion\":true,\"isautomatic\":false,\"istrackeduser\":false,\"uservisible\":true,\"details\":[]},\"dates\":[]}]},{\"id\":21019,\"name\":\"Tile3\",\"visible\":1,\"summary\":\"\",\"summaryformat\":1,\"section\":3,\"hiddenbynumsections\":0,\"uservisible\":true,\"modules\":[{\"id\":684,\"url\":\"https://moodle.cluuub.xyz/mod/resource/view.php?id=684\",\"name\":\"DSL_Document\",\"instance\":112,\"contextid\":1145,\"visible\":1,\"uservisible\":true,\"visibleoncoursepage\":1,\"modicon\":\"https://moodle.cluuub.xyz/theme/image.php/boost/core/1651754230/f/text-24\",\"modname\":\"resource\",\"modplural\":\"Files\",\"availability\":null,\"indent\":0,\"onclick\":\"\",\"afterlink\":null,\"customdata\":\"{\\\"displayoptions\\\":\\\"\\\",\\\"display\\\":5}\",\"noviewlink\":false,\"completion\":1,\"completiondata\":{\"state\":0,\"timecompleted\":0,\"overrideby\":null,\"valueused\":false,\"hascompletion\":true,\"isautomatic\":false,\"istrackeduser\":false,\"uservisible\":true,\"details\":[]},\"dates\":[],\"contents\":[{\"type\":\"file\",\"filename\":\"DSL_Document\",\"filepath\":\"/\",\"filesize\":942,\"fileurl\":\"https://moodle.cluuub.xyz/webservice/pluginfile.php/1145/mod_resource/content/0/DSL_Document?forcedownload=1\",\"timecreated\":1659961413,\"timemodified\":1659961413,\"sortorder\":0,\"mimetype\":\"text/plain\",\"isexternalfile\":false,\"userid\":3,\"author\":null,\"license\":\"unknown\"}],\"contentsinfo\":{\"filescount\":1,\"filessize\":942,\"lastmodified\":1659961413,\"mimetypes\":[\"text/plain\"],\"repositorytype\":\"\"}}]}]"
            );

        // Act
        var result = await _systemUnderTest.GetCourseContentAsync("token", 1);

        // Assert
        Assert.That(result, Has.Length.EqualTo(4));
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
        Assert.That(result.MoodleToken, Is.EqualTo("testToken"));
    }

    [Test]
    public async Task SearchCoursesAsync_Valid()
    {
        // Arrange
        _mockHttp.When("*")
            .Respond(
                "application/json",
                "{\"total\":2,\"courses\":[{\"id\":49,\"fullname\":\"Lernpfad-Kurs-Customdata\",\"displayname\":\"Lernpfad-Kurs-Customdata\",\"shortname\":\"Lernpfad-Kurs-Customdata\",\"categoryid\":1,\"categoryname\":\"Miscellaneous\",\"sortorder\":10006,\"summary\":\"<pdir=\\\"ltr\\\"style=\\\"text-align:left;\\\">LernzieldiesesKursesistXXXX<\\/p>\",\"summaryformat\":1,\"summaryfiles\":[],\"overviewfiles\":[{\"filename\":\"AddFileFlowChart.png\",\"filepath\":\"\\/\",\"filesize\":319760,\"fileurl\":\"https:\\/\\/moodle.cluuub.xyz\\/webservice\\/pluginfile.php\\/265\\/course\\/overviewfiles\\/Add%20File%20Flow%20Chart.png\",\"timemodified\":1654246976,\"mimetype\":\"image\\/png\"}],\"showactivitydates\":true,\"showcompletionconditions\":true,\"contacts\":[],\"enrollmentmethods\":[\"manual\"]},{\"id\":5,\"fullname\":\"SoftwareEngineering(Lernwelt)\",\"displayname\":\"SoftwareEngineering(Lernwelt)\",\"shortname\":\"SoftwareEngineering\",\"categoryid\":4,\"categoryname\":\"Nichtl\\u00f6schen\",\"sortorder\":30001,\"summary\":\"\",\"summaryformat\":1,\"summaryfiles\":[],\"overviewfiles\":[],\"showactivitydates\":false,\"showcompletionconditions\":true,\"contacts\":[],\"enrollmentmethods\":[\"manual\"]}],\"warnings\":[]}"
            );

        // Act
        var result = await _systemUnderTest.SearchCoursesAsync("testToken", "testSearch");

        // Assert
        Assert.That(result.Total, Is.EqualTo(2));
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
                "application/json", JsonSerializer.Serialize(new UserDataResponse
                {
                    Userid = 1,
                    Userissiteadmin = true,
                    Username = "testUser"
                }));

        // Act
        var result = await _systemUnderTest.GetMoodleUserDataAsync("moodleToken");

        // Assert
        Assert.That(result.IsAdmin, Is.EqualTo(true));
        Assert.That(result.UserId, Is.EqualTo(1));
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