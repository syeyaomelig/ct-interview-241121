using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ct.Interview.Monitor
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = CreateHostBuilder(args).Build();
			host.Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();

			return Host.CreateDefaultBuilder(args)
				.ConfigureHostConfiguration(configHost =>
				{
					configHost.SetBasePath(Directory.GetCurrentDirectory());
					configHost.AddJsonFile("appsettings.json", optional: true);
				})
				.ConfigureServices((hostContext, services) =>
				{
					services.AddHostedService<StockCompanyMonitor>();
					services.AddScoped<IDatabaseService, DatabaseService>();
					services.AddDbContext<TradingContext>(options =>
					{
						options.UseSqlServer(configuration.GetConnectionString("Main"));
						options.EnableSensitiveDataLogging();
					});
				})
				.ConfigureLogging(logging =>
				{
					logging.AddConsole();
					logging.AddDebug();
					logging.SetMinimumLevel(LogLevel.Trace);
				});
		}
	}
}

