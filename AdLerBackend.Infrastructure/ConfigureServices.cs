using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Infrastructure.LmsBackup;
using AdLerBackend.Infrastructure.Moodle;
using AdLerBackend.Infrastructure.Repositories;
using AdLerBackend.Infrastructure.Repositories.Common;
using AdLerBackend.Infrastructure.Repositories.Courses;
using AdLerBackend.Infrastructure.Services;
using AdLerBackend.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdLerBackend.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var httpClient = new HttpClient();
        // Add Moodle to DI
        services.AddSingleton<IMoodle, MoodleWebApi>();
        services.AddSingleton<ILmsBackupProcessor, LmsBackupProcessor>();
        services.AddScoped<IFileAccess, StorageService>();
        services.AddSingleton<ISerialization, SerializationService>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddSingleton(httpClient);

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContextFactory<AdLerBackendDbContext>(options => options.UseSqlServer(connectionString));
        return services;
    }
}