namespace Ct.Interview.Web.Api
{
    public class Company
    {
        public long CompanyId { get; set; }

        public string ExchangeId { get; set; }

        public string CountryCode { get; set; }

        public string StockCode { get; set; }

        public string CompanyName { get; set; }

        public string GicsIndustryGroup { get; set; }

        public bool ListStatus { get; set; }
    }
}