namespace Acme.Library.Domain.Models
{
	public class Book
	{
		public string Title { get; set; }
		public int Id { get; set; }
		public virtual Author Author { get; set; }
		public string Isbn { get; set; }
	}
}
