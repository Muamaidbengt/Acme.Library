using Acme.Library.Domain.Models;
using System;
using System.Data.Entity;

namespace Acme.Library.Domain.Interfaces
{
    public interface ILoansRepository : IDisposable
    {
        DbSet<Loan> Loans { get; set; }
		DbSet<Book> Books { get; set; }
		DbSet<Customer> Customers { get; set; }
		int SaveChanges();
    }
}
