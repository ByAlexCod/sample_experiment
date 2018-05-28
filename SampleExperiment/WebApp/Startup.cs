using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CK.AspNet;
using CK.Core;
using CK.AspNet.Auth;
using CK.Auth;
using CK.DB.AspNet.Auth;
using System;
using System.IO;

namespace WebApp
{
    class Startup
    {
        readonly IConfiguration _configuration;
        readonly IHostingEnvironment _env;
        const string _connectionString =
            "Server=.;Database=CKTEST_SampleExperiment_Data;Trusted_Connection=True;";

        public Startup( IConfiguration configuration, IHostingEnvironment env )
        {
            _env = env;
            _configuration = configuration;
        }

        public void ConfigureServices( IServiceCollection services )
        {
            services.AddOptions();
            services.AddAuthentication( WebFrontAuthOptions.OnlyAuthenticationScheme )
                .AddWebFrontAuth( options =>
                 {
                     options.ExpireTimeSpan = TimeSpan.FromHours( 1 );
                     options.SlidingExpirationTime = TimeSpan.FromHours( 1 );
                 } );

            if (_env.IsDevelopment())
            {
                string dllPath = _configuration["StObjMap:Path"];
                if( dllPath != null )
                {
                    var parentPath = Path.GetDirectoryName( Path.GetDirectoryName( Path.GetDirectoryName( Path.GetDirectoryName( _env.ContentRootPath ) ) ) );
                    dllPath = Path.Combine( parentPath, dllPath );
                    File.Copy( dllPath, Path.Combine( AppContext.BaseDirectory, "CK.StObj.AutoAssembly.dll" ), overwrite: true );
                }
            }
            services.AddDefaultStObjMap( "CK.StObj.AutoAssembly", _connectionString );

            services.AddSingleton<IAuthenticationTypeSystem, StdAuthenticationTypeSystem>();
            services.AddSingleton<IWebFrontAuthLoginService, SqlWebFrontAuthLoginService>();

            services.AddCors();
        }

        public void Configure( IApplicationBuilder app, IHostingEnvironment env )
        {
            app.UseRequestMonitor();

            app.UseAuthentication();

            app.UseCors( builder =>
            builder.WithOrigins( "localhost" ).AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin() );
        }

    }
}
