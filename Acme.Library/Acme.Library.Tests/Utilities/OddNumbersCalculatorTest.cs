using Acme.Library.Utilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Acme.Library.Tests.Utilities
{
	[TestClass]
	public class OddNumbersCalculatorTest
	{
		private OddNumbersCalculator _sut;

		[TestInitialize]
		public void SetupPerTest()
		{
			_sut = new OddNumbersCalculator();
		}

		[TestMethod]
		public void GetPositiveOddNumbersSmallerThan_0_ReturnsEmptyResult()
		{
			var result = _sut.GetPositiveOddNumbersSmallerThan(0);
			result.Should().BeEmpty();
		}

		[TestMethod]
		public void GetPositiveOddNumbersSmallerThan_NegativeNumber_ReturnsEmptyResult()
		{
			var result = _sut.GetPositiveOddNumbersSmallerThan(-1);
			result.Should().BeEmpty();
		}

		[TestMethod]
		public void GetPositiveOddNumbersSmallerThan_1_ReturnsEmptyResult()
		{
			var result = _sut.GetPositiveOddNumbersSmallerThan(1);
			result.Should().BeEmpty();
		}

		[TestMethod]
		public void GetPositiveOddNumbersSmallerThan_2_ReturnsListWith1()
		{
			var result = _sut.GetPositiveOddNumbersSmallerThan(2);
			result.Should().BeEquivalentTo(new List<short> { 1 });
		}

		[TestMethod]
		[TestCategory("YourMileageMayVary")]
		public void GetPositiveOddNumbersSmallerThan_ShortMaxValue_DoesNotRunOutOfMemory() // ...usually
		{
			var result = _sut.GetPositiveOddNumbersSmallerThan(short.MaxValue);
			object temp;
			Action enumeratingTheResults = (() => temp = result.ToList());
			enumeratingTheResults.ShouldNotThrow<OutOfMemoryException>();
		}

		[TestMethod]
		public void GetPositiveOddNumbersSmallerThan_100_ReturnsListWith50UniqueOddItemsLessThan100()
		{
			var result = _sut.GetPositiveOddNumbersSmallerThan(100);
			result.Should().OnlyHaveUniqueItems()
				.And.HaveCount(50)
				.And.NotContain(n => n % 2 == 0, "No number should be even")
				.And.NotContain(n => n > 100 || n < 0, "No number should be outside the interval [0, 100]");
		}
	}
}
