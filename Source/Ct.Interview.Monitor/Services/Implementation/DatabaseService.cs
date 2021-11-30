using Ct.Interview.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ct.Interview.Monitor
{
	public class DatabaseService : IDatabaseService
	{
		TradingContext _database = null;

		public DatabaseService(TradingContext context)
		{
			_database = context;
		}

		public List<Company> GetAllCompanies()
		{
			return _database.StockCompanies.ToList();
		}

		public void InsertCompanies(List<Company> request)
		{
			using (_database)
			{
				request.ForEach(c => _database.Add(c));
				_database.SaveChanges();
			}
		}

		public void InsertCompany(Company request)
		{
			using (_database)
			{
				_database.Add(request);
				_database.SaveChanges();
			}
		}

		public void UpdateCompanies(List<Company> request)
		{
			request.ForEach(c =>
			{
				_database.Entry(c).State = EntityState.Modified;
			});
			_database.SaveChanges();
		}

		public void UpdateCompany(Company request)
		{
			_database.Entry(request).State = EntityState.Modified;
			_database.SaveChanges();
		}
	}
}
