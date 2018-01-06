using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace SampeHttps
{
	public class Program
    {
        public static void Main(string[] args)
        {
			IWebHostBuilder builder = new WebHostBuilder();

			builder.ConfigureServices(s => {
				s.AddSingleton(builder);
			});

			builder.UseKestrel()
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseStartup<Startup>()
                .UseIISIntegration();

			var host = builder.Build();

			host.Run();
		}
    }
}
