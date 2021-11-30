using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ct.Interview.Core;

namespace Ct.Interview.Service
{
	public class DatabaseService : IDatabaseService
	{
		TradingContext _database = null;

		public DatabaseService(TradingContext context)
		{
			_database = context;
		}

		public Company GetCompanyByCountryCodeAndStockCode(CompanyRequest request)
		{
			return _database.StockCompanies.SingleOrDefault(c => c.CountryCode == request.CountryCode && c.StockCode == request.StockCode);
		}
	}
}
