using Booktex.Application.GitHub;
using Booktex.Infrastructure.Configuration;
using Booktex.Infrastructure.GitHub;
using GitHub;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Infrastructure;
public static class DependencyInjectionInfrastructure
{
    public static WebApplicationBuilder AddInfrastructureConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<InfrastructureConfiguration>(builder.Configuration.GetSection(InfrastructureConfiguration.ConfigurationElementName));
        return builder;
    }

    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        var conf = builder.InfraConf();
        builder.Services.AddScoped<IGitHubService, GitHubService>();
        builder.Services.AddHttpClient<IGitHubApiClient, GitHubApiClient>()
            .ConfigurePrimaryHttpMessageHandler(provider =>
            {
                var handler = new HttpClientHandler();
                return handler;
            }).ConfigureHttpClient((provider, client) =>
            {
                client.BaseAddress = new Uri(conf.GitHub.BaseUrl);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", conf.GitHub.AuthenticationToken());
            });
        return builder;
    }

    private static InfrastructureConfiguration InfraConf(this WebApplicationBuilder builder)
    {
        var returnee = new InfrastructureConfiguration();
        builder.Configuration.Bind(InfrastructureConfiguration.ConfigurationElementName, returnee);
        return returnee;
    }

}
