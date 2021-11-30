using Grpc.Core;

namespace Ct.Interview.Service
{
	public interface IStockService
	{
		Task<CompanyResponse> GetCompanyByCountryCodeAndStockCode(CompanyRequest request, ServerCallContext context);
	}
}
