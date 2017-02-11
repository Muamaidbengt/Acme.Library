using Acme.Library.Domain.Interfaces;
using Acme.Library.Domain.Models;
using Acme.Library.Tests.TestHelpers;
using Acme.Library.Web.Controllers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Acme.Library.Tests.Controllers
{
	[TestClass]
	public class LoansControllertest
	{
		private MockTable<Book> _books;
		private MockTable<Customer> _customers;
		private MockTable<Loan> _loans;
		private MockTable<LoanHistory> _loanHistories;
		private Mock<ILoansRepository> _loansRepository;
		private Mock<ILoanHistoriesRepository> _loanHistoriesRepository;

		[TestInitialize]
		public void SetupPerTest()
		{
			_loanHistories = new MockTable<LoanHistory>();
			_loanHistoriesRepository = new Mock<ILoanHistoriesRepository>();
			_loanHistoriesRepository.Setup(lh => lh.LoanHistories).Returns(_loanHistories.Mock.Object);

			_books = new MockTable<Book>();
			_customers = new MockTable<Customer>();
			_loans = new MockTable<Loan>();
			
			_loansRepository = new Mock<ILoansRepository>();
			_loansRepository.Setup(l => l.Books).Returns(_books.Mock.Object);
			_loansRepository.Setup(l => l.Customers).Returns(_customers.Mock.Object);
			_loansRepository.Setup(l => l.Loans).Returns(_loans.Mock.Object);
		}

		private LoansController CreateController()
		{
			return new LoansController(_loansRepository.Object, _loanHistoriesRepository.Object);
		}

		[TestMethod]
		public void Get_WhenDatabaseContainsOneLoan_ReturnsIt()
		{
			var sut = CreateController();
			_loans.TableContents.Add(new Loan { Book = new Book(), Loaner = new Customer() });

			var result = sut.Get();
			result.Should().NotBeNullOrEmpty().And.HaveCount(1);
		}

		[TestMethod]
		public void Post_WhenLoanIsPossible_CreatesLoan()
		{
			var sut = CreateController();
			var book = new Book { Title = "The big Fake", Id = 4 };
			_books.TableContents.Add(book);

			var result = sut.Post(new Domain.Models.Dto.CreateLoanDto { BookId = book.Id, LoanerFirstName = "John", LoanerLastName = "Doe" });

			_loans.Mock.Verify(l => l.Add(It.IsAny<Loan>()), Times.Once);
		}

		[TestMethod]
		public void Post_WhenLoanIsPossibleAndCustomerDoesNotExist_CreatesCustomer()
		{
			var sut = CreateController();
			var book = new Book { Title = "The big Fake", Id = 4 };
			_books.TableContents.Add(book);

			var result = sut.Post(new Domain.Models.Dto.CreateLoanDto { BookId = book.Id, LoanerFirstName = "John", LoanerLastName = "Doe" });

			_customers.Mock.Verify(l => l.Add(It.IsAny<Customer>()), Times.Once);
		}

		[TestMethod]
		public void Post_WhenLoanIsPossibleAndCustomerExists_DoesNotCreateCustomer()
		{
			var sut = CreateController();
			var book = new Book { Title = "The big Fake", Id = 4 };
			_books.TableContents.Add(book);
			var existingCustomer = new Customer { FirstName = "Jane", LastName = "McLoanington" };
			_customers.TableContents.Add(existingCustomer);

			var result = sut.Post(new Domain.Models.Dto.CreateLoanDto { BookId = book.Id, LoanerFirstName = existingCustomer.FirstName, LoanerLastName = existingCustomer.LastName });

			_customers.Mock.Verify(l => l.Add(It.IsAny<Customer>()), Times.Never);
		}

		[TestMethod]
		public void Post_WhenLoanerIsNotActive_DeniesLoan()
		{
			var sut = CreateController();
			var book = new Book { Title = "The big Fake", Id = 4 };
			_books.TableContents.Add(book);
			var inactiveCustomer = new Customer { FirstName = "Scrooge", LastName = "McStealington", Id = 2, IsActive = false };
			_customers.TableContents.Add(inactiveCustomer);

			var result = sut.Post(new Domain.Models.Dto.CreateLoanDto { BookId = book.Id, LoanerFirstName = inactiveCustomer.FirstName, LoanerLastName = inactiveCustomer.LastName });

			_loans.Mock.Verify(l => l.Add(It.IsAny<Loan>()), Times.Never);
		}

		[TestMethod]
		public void Post_WhenBookIsAlreadyLoanedToSomeone_DeniesLoan()
		{
			var sut = CreateController();
			var book = new Book { Title = "The big Fake", Id = 4 };
			_books.TableContents.Add(book);
			var existingLoan = new Loan { Book = book };
			_loans.TableContents.Add(existingLoan);

			var result = sut.Post(new Domain.Models.Dto.CreateLoanDto { BookId = book.Id, LoanerFirstName = "John", LoanerLastName = "Doe" });

			_loans.Mock.Verify(l => l.Add(It.IsAny<Loan>()), Times.Never);
		}

		[TestMethod]
		public void Delete_WhenLoanDoesNotExist_DoesNotDeleteAnything()
		{
			var sut = CreateController();
			var book = new Book { Title = "The big Fake", Id = 4 };
			var existingLoan = new Loan { Book = book };
			_loans.TableContents.Add(existingLoan);

			var result = sut.Delete(42);

			_loans.Mock.Verify(l => l.Remove(It.IsAny<Loan>()), Times.Never);
			_loanHistories.Mock.Verify(lh => lh.Add(It.IsAny<LoanHistory>()), Times.Never);
		}

		[TestMethod]
		public void Delete_WhenLoanExists_ArchivesTheLoan()
		{
			var sut = CreateController();
			var book = new Book { Title = "The big Fake", Id = 4 };
			var existingLoan = new Loan { Book = book, Id = 3 };
			_loans.TableContents.Add(existingLoan);

			var result = sut.Delete(existingLoan.Id);

			_loans.Mock.Verify(l => l.Remove(It.IsAny<Loan>()), Times.Once);
			_loanHistories.Mock.Verify(lh => lh.Add(It.IsAny<LoanHistory>()), Times.Once);
		}
	}
}
