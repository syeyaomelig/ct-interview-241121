using System;
using System.Collections.Generic;

namespace Ct.Interview.Core
{
    public partial class Company
    {
        public long CompanyId { get; set; }
        public string ExchangeId { get; set; } = "XXX";
        public string CountryCode { get; set; } = "XXX";
        public string StockCode { get; set; } = "XXX";
        public string CompanyName { get; set; } = "(no company name)";
        public string GicsIndustryGroup { get; set; } = "N/A";
        public bool TradeStatus { get; set; } = true;
    }
}
