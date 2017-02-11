using Acme.Library.Domain.Models;
using System.Data.Entity;

namespace Acme.Library.Domain.Interfaces
{
	public interface ILoanHistoriesRepository
	{
		DbSet<LoanHistory> LoanHistories { get; set; }
		int SaveChanges();
	}
}
