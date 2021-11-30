using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ct.Interview.Web.Api.Controllers
{
    [Route("companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private ICompanyService _companyService;
        private ILogger _logger;

        public CompaniesController(
            ICompanyService companyService
            /*ILogger logger*/)
        {
            _companyService = companyService;
            //_logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<Company>> Get(
            [FromQuery, Required, MaxLength(3)] string countryCode,
            [FromQuery, Required, MaxLength(10)] string stockCode)
        {
            try
			{
                var company = await _companyService.GetCompanyByCountryCodeAndStockCode(countryCode, stockCode);

                if (company != null && company.CompanyId != -1)
                    return new ObjectResult(company);
                else
                    return StatusCode(404, "The company does not exist.");
            }

            catch (Exception e)
			{
                
                //_logger.Log(LogLevel.Error, e.Message, e.StackTrace, e.InnerException?.Message, e.InnerException?.Message);
                return new StatusCodeResult(500);
			}
        }
    }
}
