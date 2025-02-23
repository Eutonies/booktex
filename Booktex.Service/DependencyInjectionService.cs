using Booktex.Application;
using Booktex.Infrastructure;
using Booktex.Persistence;
using Microsoft.AspNetCore.Builder;

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
        builder.Services.AddControllers(opts => 
        {
        });
        builder.Services.AddOpenApi();
        return builder;
    }


    public static WebApplication UseServices(this WebApplication app)
    {
        app.UseRouting();
        app.UseAuthorization();
        app.MapOpenApi();
        app.UseSwaggerUI(opts =>
        {
            opts.SwaggerEndpoint("/openapi/v1.json", "v1");
        });
        app.MapControllers();

        return app;
    }


}
