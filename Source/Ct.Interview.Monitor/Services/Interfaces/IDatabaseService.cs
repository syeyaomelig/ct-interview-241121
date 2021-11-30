using Ct.Interview.Core;

namespace Ct.Interview.Monitor
{
	public interface IDatabaseService
	{
		List<Company> GetAllCompanies();

		void InsertCompany(Company request);

		void InsertCompanies(List<Company> request);

		void UpdateCompany(Company request);

		void UpdateCompanies(List<Company> request);
	}
}
