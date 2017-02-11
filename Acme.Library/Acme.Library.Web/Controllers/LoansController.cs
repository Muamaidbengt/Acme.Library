using Acme.Library.Data;
using Acme.Library.Domain.Interfaces;
using Acme.Library.Domain.Models;
using Acme.Library.Domain.Models.Dto;
using System;
using System.Linq;
using System.Web.Http;

namespace Acme.Library.Web.Controllers
{
	/// <summary>
	/// Loans and returns books from the library
	/// </summary>
    public class LoansController : ApiController
    {
		private readonly ILoansRepository _loans;
		private readonly ILoanHistoriesRepository _loanHistories;

		/// <summary>
		/// Creates a new instance
		/// </summary>
		public LoansController(ILoansRepository loans, ILoanHistoriesRepository loanHistories)
		{
			_loans = loans;
			_loanHistories = loanHistories;
		}

        // GET api/loans
		/// <summary>
		/// Returns a list of all loans
		/// </summary>
        public IQueryable<LoanDto> Get()
        {
			return _loans.Loans
				.Select(loan => new LoanDto { Id = loan.Id, BookTitle = loan.Book.Title, LoanerName = loan.Loaner.FirstName + " " + loan.Loaner.LastName, ExpiryDate = loan.ExpiryDate });
        }

		// POST api/loans
		/// <summary>
		/// Loans a book. The book must not already be loaned to someone, and the customer must be in good standing.
		/// </summary>
		public IHttpActionResult Post([FromBody]CreateLoanDto loan)
        {
			var book = _loans.Books.SingleOrDefault(b => b.Id == loan.BookId);
			if (book == null)
				return NotFound();

			var existingLoan = _loans.Loans.FirstOrDefault(l => l.Book.Id == loan.BookId);
			if (existingLoan != null)
				return StatusCode(System.Net.HttpStatusCode.Forbidden);

			var customer = _loans.Customers.SingleOrDefault(c => c.FirstName == loan.LoanerFirstName && c.LastName == loan.LoanerLastName);
			if (customer == null)
			{
				customer = new Customer { FirstName = loan.LoanerFirstName, LastName = loan.LoanerLastName, IsActive = true, CreatedDate = DateTime.Today };
				_loans.Customers.Add(customer);
			}
			else if (!customer.IsActive)
			{
				return StatusCode(System.Net.HttpStatusCode.Forbidden);
			}

			var newLoan = new Loan { Book = book, ExpiryDate = DateTime.Today.AddDays(7), Loaner = customer };
			_loans.Loans.Add(newLoan);

			_loans.SaveChanges();
			return Ok();
		}

        // DELETE api/loans/5
		/// <summary>
		/// Terminates a loan and returns the book
		/// </summary>
        public IHttpActionResult Delete(int id)
        {
			var loan = _loans.Loans.SingleOrDefault(l => l.Id == id);
			if (loan == null)
				return NotFound();

			var loanHistory = new LoanHistory();
			_loanHistories.LoanHistories.Add(loanHistory);
			_loans.Loans.Remove(loan);

			_loans.SaveChanges();
			_loanHistories.SaveChanges();
			return Ok();
        }

		/// <summary>
		/// Cleans up
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			_loans?.Dispose();
			base.Dispose(disposing);
		}
	}
}