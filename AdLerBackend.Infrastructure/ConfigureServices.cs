using AdLerBackend.Application.Common.Interfaces;
using Infrastructure.Moodle;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Add Moodle to DI
        services.AddSingleton<IMoodle, MoodleWebApi>();
        return services;
    }
}