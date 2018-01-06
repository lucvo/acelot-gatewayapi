using CacheManager.Core;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace SampeHttps
{
	public class Startup
    {
		public Startup(IHostingEnvironment env)
		{
			var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddJsonFile("configuration.json")
				.AddEnvironmentVariables();

			Configuration = builder.Build();
		}

		public IConfigurationRoot Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
            services.AddCors(options =>
            {
                options.AddPolicy("Sample", policyBuilder =>
                {
                    policyBuilder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });
            var authenticationProviderKey = "secret";

            services.AddAuthentication()
                .AddIdentityServerAuthentication(authenticationProviderKey, o =>
            {
                o.Authority = Configuration["identityUrl"];
                o.ApiName = "CustomerServices";
                o.RequireHttpsMetadata = false;
            });

            services.AddOcelot(Configuration)
					.AddCacheManager(x => {
						x.WithMicrosoftLogging(log =>
						{
							log.AddConsole(LogLevel.Warning);
						})
						.WithDictionaryHandle();
					});
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            app.UseCors("Sample");
            var forwardOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                RequireHeaderSymmetry = false
            };

            forwardOptions.KnownNetworks.Clear();
            forwardOptions.KnownProxies.Clear();

            // ref: https://github.com/aspnet/Docs/issues/2384
            app.UseForwardedHeaders(forwardOptions);
            app.UseAuthentication();
            app.UseOcelot().Wait();
		}
	}
}
