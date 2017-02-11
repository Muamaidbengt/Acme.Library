using System;

namespace Acme.Library.Domain.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public virtual Book Book { get; set; }
		public virtual Customer Loaner { get; set; }
    }
}
