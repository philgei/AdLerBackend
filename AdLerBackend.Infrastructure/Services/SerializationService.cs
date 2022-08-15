using System.Text.Json;
using AdLerBackend.Application.Common.Exceptions.LMSBAckupProcessor;
using AdLerBackend.Application.Common.Interfaces;

namespace Infrastructure.Services;

public class SerializationService : ISerialization
{
    public Task<TClass> GetObjectFromJsonStreamAsync<TClass>(Stream stream)
    {
        stream.Position = 0;

        var retVal = JsonSerializer.Deserialize<TClass>(stream, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new LmsBackupProcessorException("Could not deserialize DSL file");
        return Task.FromResult(retVal);
    }
}