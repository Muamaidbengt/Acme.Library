namespace Acme.Library.Domain.Models.Dto
{
	public class CreateBookDto
	{
		public string AuthorFirstName { get; set; }
		public string AuthorLastName { get; set; }
		public string Title { get; set; }
		public int PublishedYear { get; set; }
		public string Isbn { get; set; }
	}
}
