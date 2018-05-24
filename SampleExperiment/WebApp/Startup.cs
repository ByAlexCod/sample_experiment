using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CK.AspNet;
using CK.Core;

namespace WebApp
{
    class Startup
    {
        readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseRequestMonitor();

            app.Run(async context =>
            {
                var monitor = context.GetRequestMonitor();
                monitor.Info( "Monitor is available." );
                await context.Response.WriteAsync("Hello World!");
            });
        }

    }
}
