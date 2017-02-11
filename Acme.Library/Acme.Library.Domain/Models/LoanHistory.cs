namespace Acme.Library.Domain.Models
{

	public class LoanHistory
	{
		public int Id { get; set; }
		public int BookId { get; set; }
		public string BookTitle { get; set; }

		public int LoanerId { get; set; }
		public string LoanerFirstName { get; set; }
		public string LoanerLastName { get; set; }
	}
}
