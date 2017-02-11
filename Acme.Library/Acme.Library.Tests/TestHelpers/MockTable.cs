using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Acme.Library.Tests.TestHelpers
{
	public class MockTable<T> where T: class
	{
		public Mock<DbSet<T>> Mock { get; }
		public List<T> TableContents { get; }

		public MockTable()
		{
			Mock = new Mock<DbSet<T>>();
			TableContents = new List<T>();

			Mock.As<IQueryable<T>>().Setup(x => x.Provider).Returns(() => TableContents.AsQueryable().Provider);
			Mock.As<IQueryable<T>>().Setup(x => x.Expression).Returns(() => TableContents.AsQueryable().Expression);
			Mock.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(() => TableContents.AsQueryable().ElementType);
			Mock.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(() => TableContents.AsQueryable().GetEnumerator());
		}
	}
}
