using Acme.Library.Domain.Models;
using Acme.Library.Domain.Interfaces;
using SQLite.CodeFirst;
using System.Data.Entity;

namespace Acme.Library.Data
{
    public class LibraryContext : DbContext, IBooksRepository, ILoansRepository, ILoanHistoriesRepository
    {
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<LibraryContext>(modelBuilder);
			Database.SetInitializer(sqliteConnectionInitializer);
		}

		public DbSet<Book> Books { get; set; }
		public DbSet<Author> Authors { get; set; }
        public DbSet<Loan> Loans { get; set; }
		public DbSet<LoanHistory> LoanHistories { get; set; }
		public DbSet<Customer> Customers { get; set; }
	}
}
