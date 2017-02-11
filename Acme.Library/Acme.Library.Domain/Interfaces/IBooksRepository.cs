using Acme.Library.Domain.Models;
using System;
using System.Data.Entity;

namespace Acme.Library.Domain.Interfaces
{
    public interface IBooksRepository : IDisposable
    {
        DbSet<Book> Books { get; set; }
		DbSet<Author> Authors { get; set; }
		int SaveChanges();
    }
}
