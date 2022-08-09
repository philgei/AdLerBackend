using AdLerBackend.Application.Common.Interfaces;
using Infrastructure.Moodle;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        var httpClient = new HttpClient();
        // Add Moodle to DI
        services.AddSingleton<IMoodle, MoodleWebApi>();
        services.AddSingleton<ILmsBackupProcessor, LmsBackupProcessor.LmsBackupProcessor>();
        services.AddSingleton(httpClient);
        return services;
    }
}