using Booktex.Application.Subscription;
using Booktex.Persistence.Configuration;
using Booktex.Persistence.Context;
using Booktex.Persistence.Subscription;
using Booktex.Persistence.Subscription.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql.NameTranslation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Persistence;
public static class DependencyInjectionPersistence
{
    public static WebApplicationBuilder AddPersistenceConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<PersistenceConfiguration>(builder.Configuration.GetSection(PersistenceConfiguration.ConfigurationElementName));
        return builder;
    }

    public static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContextFactory<BooktexDbContext>(ConfigureDb, lifetime: ServiceLifetime.Singleton);
        builder.Services.AddSingleton<ISubscriptionRepo, SubscriptionRepo>();
        return builder;
    }

    private static void ConfigureDb(IServiceProvider provider, DbContextOptionsBuilder optsBuilder)
    {
        var connString = provider.GetRequiredService<IOptions<PersistenceConfiguration>>().Value.Db.ConnectionString;
        optsBuilder.UseNpgsql(connString, opts =>
        {
            opts.MapEnum<BooktexSubscriptionTypeDbo>("subscription_type", nameTranslator: new NpgsqlNullNameTranslator());
        }).EnableDetailedErrors();
    }


}
