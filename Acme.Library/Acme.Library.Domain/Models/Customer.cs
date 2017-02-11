using System;

namespace Acme.Library.Domain.Models
{
    public class Customer
    {
		public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
