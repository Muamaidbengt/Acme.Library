using System;

namespace Acme.Library.Domain.Models.Dto
{
	public class LoanDto
	{
		public int Id { get; set; }
		public string BookTitle { get; set; }
		public string LoanerName { get; set; }
		public DateTime ExpiryDate { get; set; }
	}
}
