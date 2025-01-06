using Booktex.Application.Zip;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Application;
public static class DependencyInjectionApplication
{

    public static WebApplicationBuilder AddApplicationConfiguration(this WebApplicationBuilder builder)
    {
        return builder;
    }

    public static WebApplicationBuilder AddApplication(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IZipFileParser, ZipFileParser>();
        return builder;
    }

}
