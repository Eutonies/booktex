using Booktex.Application;
using Booktex.Infrastructure;
using Booktex.Persistence;

namespace Booktex.Service;

public static class DependencyInjectionService
{
    public static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddJsonFile("appsettings.json", optional: false);
        builder.Configuration.AddJsonFile("appsettings.local.json", optional: true);
        builder.Configuration.AddEnvironmentVariables();
        builder.AddApplicationConfiguration();
        builder.AddInfrastructureConfiguration();
        builder.AddPersistenceConfiguration();
        return builder;
    }

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.AddApplication();
        builder.AddInfrastructure();
        builder.AddPersistence();
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        return builder;
    }



}
