using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Ct.Interview.Web.Api.Stock;

namespace Ct.Interview.Web.Api
{
	public class CompanyService : ICompanyService
	{
		IConfiguration _configuration = null;
		GrpcChannel _channel = null;
		StockClient _client = null;

		public CompanyService(IConfiguration configuration)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			_channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("ServerAddress"));
			_client = new StockClient(_channel);
		}

		public async Task<Company> GetCompanyByCountryCodeAndStockCode(string countryCode, string stockCode)
		{
			var response = await _client.GetCompanyByCountryCodeAndStockCodeAsync(new CompanyRequest { CountryCode = countryCode, StockCode = stockCode });

			if (response != null)
			{
				return new Company
				{
					CompanyId = response.CompanyId,
					CompanyName = response.CompanyName,
					CountryCode = response.CountryCode,
					StockCode = response.StockCode,
					ExchangeId = response.ExchangeId,
					GicsIndustryGroup = response.GicsIndustryGroup,
					ListStatus = response.ListStatus
				};
			}

			else
				return null;
			
		}
	}
}
