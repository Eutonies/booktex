using Booktex.Infrastructure;

namespace Booktex.Service;

public static class DependencyInjectionService
{
    public static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddJsonFile("appsettings.json", optional: false);
        builder.Configuration.AddJsonFile("appsettings.local.json", optional: true);
        builder.Configuration.AddEnvironmentVariables();
        builder.AddInfrastructureConfiguration();
        return builder;
    }

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.AddInfrastructure();
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();


        return builder;
    }



}
