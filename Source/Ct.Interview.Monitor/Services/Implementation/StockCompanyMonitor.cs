using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ct.Interview.Core;
using System.Net;
using Flurl.Http;
using CsvHelper;
using System.Globalization;

namespace Ct.Interview.Monitor
{
	public class StockCompanyMonitor : BackgroundService
	{
		private readonly ILogger<StockCompanyMonitor> _logger;
		private IConfiguration _config;
		private IDatabaseService _databaseService;
		private System.Timers.Timer _timer = null;
		private List<Company> _companies = null;

		public StockCompanyMonitor(ILogger<StockCompanyMonitor> logger,
			IConfiguration config,
			IDatabaseService databaseService)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_config = config ?? throw new ArgumentNullException(nameof(config));
			_databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
			_timer = new System.Timers.Timer
			{
				AutoReset = true,
				Interval = _config.GetValue<double>("Service:PullFrequencyHours") * 3600000,
			};
		}

		#region Implementation

		public override void Dispose()
		{
			_timer?.Dispose();
		}

		public override Task StopAsync(CancellationToken stoppingToken)
		{
			_timer.Elapsed -= checkForChanges;
			_timer.Dispose();
			return Task.CompletedTask;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Started.");
			_companies = _databaseService.GetAllCompanies() ?? new();
			_timer.Elapsed += checkForChanges;
			_timer.Start();
			await Task.Yield();
		}

		#endregion

		#region Methods

		private async void checkForChanges(object source, ElapsedEventArgs e)
		{
			if (_config.GetValue<DateTime>("Service:LastPullDateUtc").ToUniversalTime() < DateTime.UtcNow)
			{
				_logger.LogInformation("Updating...");

				try
				{
					await checkForUpdatedList();
					updateAppSettingsJson();
				}

				catch (Exception ex)
				{
					_logger.LogError("An error has occured:", ex.ToString());
				}
			}
		}

		private async Task checkForUpdatedList()
		{
			var listCompanies = new List<Company>();
			var delistedCompanies = new List<Company>();
			var newCompanies = new List<Company>();

			var path = await _config.GetValue<string>("Service:ListedSecuritiesCsvUrl").DownloadFileAsync(Directory.GetCurrentDirectory());
			using (var reader = new StreamReader(path))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				csv.Read();
				csv.Read();
				csv.ReadHeader();
				while (csv.Read())
				{
					listCompanies.Add(new Company
					{
						CompanyName = csv.GetField("Company name"),
						StockCode = csv.GetField("ASX code"),
						GicsIndustryGroup = csv.GetField("GICS industry group"),
					});
				}
			}

			foreach (var c in _companies)
			{
				if (listCompanies.SingleOrDefault(company => company.CompanyName == c.CompanyName) == null)
				{
					c.TradeStatus = false;
					delistedCompanies.Add(c);
				}
			}

			if (delistedCompanies.Any()) _databaseService.UpdateCompanies(delistedCompanies);

			foreach (var c in listCompanies)
			{
				if (_companies.SingleOrDefault(company => company.CompanyName == c.CompanyName) == null) newCompanies.Add(new Company
				{
					CompanyName = c.CompanyName,
					CountryCode = "AU",
					ExchangeId = "ASX",
					GicsIndustryGroup = c.GicsIndustryGroup == "Not Applic" ? "N/A" : c.GicsIndustryGroup,
					TradeStatus = true,
					StockCode = c.StockCode
				});
			}

			if (newCompanies.Any()) _databaseService.InsertCompanies(newCompanies);
		}

		private void updateAppSettingsJson()
		{
			var timeStamp = DateTime.UtcNow;
			var json = JObject.Parse(File.ReadAllText($"{Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json")}"));
			json["Service"]["LastPullDateUtc"] = timeStamp.ToString("yyyy-MM-dd HH:mm:ss");

			using (StreamWriter file = File.CreateText($"{Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json")}"))
			{
				JsonSerializer serializer = new JsonSerializer();
				serializer.Formatting = Formatting.Indented;
				serializer.Serialize(file, json);
			}
		}

		#endregion

	}
}
