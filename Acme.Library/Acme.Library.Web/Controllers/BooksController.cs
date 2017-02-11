using Acme.Library.Domain.Models;
using Acme.Library.Domain.Models.Dto;
using Acme.Library.Domain.Interfaces;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Acme.Library.Controllers
{
	/// <summary>
	/// Manages books in the library
	/// </summary>
	public class BooksController : ApiController
	{
		private readonly IBooksRepository _books;

		/// <summary>
		/// Creates a new controller
		/// </summary>
		public BooksController(IBooksRepository books)
		{
            _books = books;
		}

		/// <summary>
		/// Cleans up
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			_books?.Dispose();
            base.Dispose(disposing);
		}

		// GET api/books
		/// <summary>
		/// Returns all books currently owned by the library
		/// </summary>
		/// <returns></returns>
		public IQueryable<BookDto> Get()
		{
			return _books.Books.Select(book => new BookDto { Id = book.Id, Title = book.Title, AuthorName = book.Author.FirstName + " " + book.Author.LastName });
		}

		// GET api/books/5
		[ResponseType(typeof(BookDto))]
		public async Task<IHttpActionResult> Get(int id)
		{
			var book = await _books.Books.Include(b => b.Author).SingleOrDefaultAsync(b => b.Id == id);
			if (book == null)
				return NotFound();
			return Ok(new BookDto { Id = book.Id, Title = book.Title, AuthorName = book.Author.FirstName + " " + book.Author.LastName });
		}

		// POST api/books
		/// <summary>
		/// Adds a new book to the library collection
		/// </summary>
		public IHttpActionResult Post([FromBody]CreateBookDto book)
		{
			var author = _books.Authors.FirstOrDefault(a => book.AuthorFirstName == a.FirstName && book.AuthorLastName == a.LastName);

			if (author == null)
			{
				author = new Author { FirstName = book.AuthorFirstName, LastName = book.AuthorLastName };
				_books.Authors.Add(author);
			}

			var newBook = new Book { Author = author, Title = book.Title };
			_books.Books.Add(newBook);
			_books.SaveChanges();
			return Ok();
		}

		// DELETE api/books/5
		/// <summary>
		/// Removes a book from the library collection
		/// </summary>
		public IHttpActionResult Delete(int id)
		{
			var book = _books.Books.SingleOrDefault(b => b.Id == id);
			if (book == null)
				return NotFound();

			_books.Books.Remove(book);
			_books.SaveChanges();

			return Ok();
		}
	}
}
