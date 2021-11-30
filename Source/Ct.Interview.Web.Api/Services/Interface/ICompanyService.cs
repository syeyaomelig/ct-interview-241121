using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ct.Interview.Web.Api
{
    public interface ICompanyService
    {
        Task<Company> GetCompanyByCountryCodeAndStockCode(string countryCode, string stockCode);
    }
}
