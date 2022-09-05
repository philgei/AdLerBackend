using System.Text.Json;
using AdLerBackend.Infrastructure.Services;

namespace AdLerBackend.Infrastructure.UnitTests.Services;

public class SerializationServiceTest
{
    [Test]
    public async Task Deserialize_Valid_CanSerializeFromStream()
    {
        // Arrange
        var service = new SerializationService();

        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        await writer.WriteAsync(
            "{\"browsers\":{\"firefox\":{\"name\":\"Firefox\",\"pref_url\":\"about:config\",\"releases\":{\"1\":{\"release_date\":\"2004-11-09\",\"status\":\"retired\",\"engine\":\"Gecko\",\"engine_version\":\"1.7\"}}}}}");

        await writer.FlushAsync();
        stream.Position = 0;

        // Act
        var result = await service.GetObjectFromJsonStreamAsync<Root>(stream);

        // Assert
        Assert.NotNull(result);
    }

    [Test]
    public async Task Deserialize_InvalidJSON_ThrowsException()
    {
        // Arrange
        var service = new SerializationService();

        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        // broken json
        await writer.WriteAsync(
            "{\"browsers\":{\"firefox\":{\"name\":\"Firefox,\"pref_url\":\"about:config\",\"releases\":{\"1\":{\"release_date\":\"2004-11-09\",\"status\":\"retired\",\"engine\":\"Gecko\",\"engine_version\":\"1.7\"}}}}}");

        await writer.FlushAsync();
        stream.Position = 0;

        // Act
        // Assert
        Assert.ThrowsAsync<JsonException>(async () => await service.GetObjectFromJsonStreamAsync<Root>(stream));
    }
}

public class BrokenClass
{
    public int foo { get; set; }
}

public class _1
{
    public string release_date { get; set; }
    public string status { get; set; }
    public string engine { get; set; }
    public string engine_version { get; set; }
}

public class Browsers
{
    public Firefox firefox { get; set; }
}

public class Firefox
{
    public string name { get; set; }
    public string pref_url { get; set; }
    public Releases releases { get; set; }
}

public class Releases
{
    public _1 _1 { get; set; }
}

public class Root
{
    public Browsers browsers { get; set; }
}