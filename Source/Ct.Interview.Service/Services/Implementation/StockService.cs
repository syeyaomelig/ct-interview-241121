using Grpc.Core;

namespace Ct.Interview.Service
{
	public class StockService : Stock.StockBase, IStockService
	{
		private readonly ILogger<StockService> _logger;
		private readonly IDatabaseService _database;

		public StockService(
			ILogger<StockService> logger,
			IDatabaseService database)
		{
			_logger = logger;
			_database = database;
		}

		public override Task<CompanyResponse> GetCompanyByCountryCodeAndStockCode(CompanyRequest request, ServerCallContext context)
		{
			_logger.LogInformation($"{nameof(GetCompanyByCountryCodeAndStockCode)} executed.");

			try
			{
				var result = _database.GetCompanyByCountryCodeAndStockCode(request);

				if (result != null)
				{
					return Task.FromResult(new CompanyResponse
					{
						CompanyId = result.CompanyId,
						CompanyName = result.CompanyName,
						CountryCode = result.CountryCode,
						ExchangeId = result.ExchangeId,
						GicsIndustryGroup = result.GicsIndustryGroup,
						StockCode = result.StockCode,
						ListStatus = result.TradeStatus
					});
				}
			}

			catch (Exception ex)
			{
				_logger.LogError($"{ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}Inner Exception: {ex.InnerException?.Message}{Environment.NewLine}{ex.InnerException?.StackTrace}");
			}

			return Task.FromResult(new CompanyResponse { CompanyId = -1} );
		}
	}
}