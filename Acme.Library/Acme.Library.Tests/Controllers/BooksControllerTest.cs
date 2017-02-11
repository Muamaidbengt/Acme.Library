using Microsoft.VisualStudio.TestTools.UnitTesting;
using Acme.Library.Controllers;
using Acme.Library.Domain.Interfaces;
using Moq;
using Acme.Library.Domain.Models;
using FluentAssertions;
using Acme.Library.Tests.TestHelpers;

namespace Acme.Library.Tests.Controllers
{
	[TestClass]
	public class BooksControllerTest
	{
		private Mock<IBooksRepository> _repository;
		private MockTable<Book> _books;
		private MockTable<Author> _authors;

		[TestInitialize]
		public void SetupPerTest()
		{
			_books = new MockTable<Book>();
			_authors = new MockTable<Author>();

			_repository = new Mock<IBooksRepository>();
			_repository.Setup(r => r.Authors).Returns(() => _authors.Mock.Object);
			_repository.Setup(r => r.Books).Returns(() => _books.Mock.Object);
		}

		private BooksController CreateController()
		{
			return new BooksController(_repository.Object);
		}

		[TestMethod]
		public void Get_WhenDatabaseContainsOneBook_ReturnsIt()
		{
			// Arrange
			var controller = CreateController();
			_books.TableContents.Add(new Book { Title = "The big fake", Author = new Author { FirstName = "Jake", LastName = "McFake" } });

			// Act
			var result = controller.Get();

			// Assert
			result.Should().NotBeNullOrEmpty()
				.And.HaveCount(1)
				.And.Contain(p => p.Title == "The big fake");
		}

		[TestMethod]
		public void Post_SavesChangesToDatabase()
		{
			// Arrange
			var controller = CreateController();

			// Act
			var result = controller.Post(new Domain.Models.Dto.CreateBookDto { Title = "The big fake", AuthorFirstName = "Jake", AuthorLastName = "McFake", PublishedYear = 1912 });

			// Assert
			_repository.Verify(r => r.SaveChanges(), Times.AtLeastOnce);
			_books.Mock.Verify(b => b.Add(It.IsAny<Book>()), Times.Once);
		}

		[TestMethod]
		public void Post_WhenAuthorDoesNotExist_CreatesANewAuthor()
		{
			// Arrange
			var controller = CreateController();

			// Act
			var result = controller.Post(new Domain.Models.Dto.CreateBookDto { Title = "The big fake", AuthorFirstName = "Jake", AuthorLastName = "McFake", PublishedYear = 1912 });

			// Assert
			_authors.Mock.Verify(t => t.Add(It.IsAny<Author>()), Times.AtLeastOnce);
		}

		[TestMethod]
		public void Post_WhenAuthorExists_DoesNotCreateANewAuthor()
		{
			// Arrange
			var controller = CreateController();
			var jake = new Author { FirstName = "Jake", LastName = "McFake", Id = 1 };
			_authors.TableContents.Add(jake);

			// Act
			var result = controller.Post(new Domain.Models.Dto.CreateBookDto { Title = "The big fake", AuthorFirstName = jake.FirstName, AuthorLastName = jake.LastName, PublishedYear = 1912 });

			// Assert
			_authors.Mock.Verify(t => t.Add(It.IsAny<Author>()), Times.Never);
		}

		[TestMethod]
		public void Delete_WhenBookExists_DeletesIt()
		{
			// Arrange
			var controller = CreateController();
			var book = new Book { Id = 1, Title = "The big fake", Author = new Author { FirstName = "Jake", LastName = "McFake" } };
			_books.TableContents.Add(book);

			// Act
			var result = controller.Delete(book.Id);

			// Assert
			_books.Mock.Verify(t => t.Remove(It.IsAny<Book>()), Times.Once);
		}

		[TestMethod]
		public void Delete_WhenBookDoesntExist_DoesntDeleteAnything()
		{
			// Arrange
			var controller = CreateController();
			var book = new Book { Id = 1, Title = "The big fake", Author = new Author { FirstName = "Jake", LastName = "McFake" } };
			_books.TableContents.Add(book);

			// Act
			const int someIdThatDoesNotExist = 42;
			var result = controller.Delete(someIdThatDoesNotExist);

			// Assert
			_books.Mock.Verify(t => t.Remove(It.IsAny<Book>()), Times.Never);
		}
	}
}
