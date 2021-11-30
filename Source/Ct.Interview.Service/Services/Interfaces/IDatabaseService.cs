using Ct.Interview.Core;

namespace Ct.Interview.Service
{
	public interface IDatabaseService
	{
		Company GetCompanyByCountryCodeAndStockCode(CompanyRequest request);
	}
}
