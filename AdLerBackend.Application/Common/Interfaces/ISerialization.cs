namespace AdLerBackend.Application.Common.Interfaces;

public interface ISerialization
{
    public Task<TClass> GetObjectFromJsonStreamAsync<TClass>(Stream stream);
}