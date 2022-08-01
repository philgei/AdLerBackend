using AdLerBackend.Application.Common.Interfaces;
using Infrastructure.Moodle;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Add Moodle to DI
        services.AddSingleton<IMoodle, MoodleWebApi>();
        return services;
    }
}