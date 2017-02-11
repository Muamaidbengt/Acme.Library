namespace Acme.Library.Domain.Models.Dto
{
	public class CreateLoanDto
	{
		public int BookId { get; set; }
		public string LoanerFirstName { get; set; }
		public string LoanerLastName { get; set; }
	}
}
