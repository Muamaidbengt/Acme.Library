using Acme.Library.Utilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Acme.Library.Tests.Utilities
{
	[TestClass]
	public class NumericExtensionsTest
	{
		[TestMethod]
		public void IsPowerOf2_IfInvokedOnPowerOf2_ReturnsTrue()
		{
			for (var i = 0; i < 62; i++)
			{
				var n = (2L << i);
				n.IsPowerOf2().Should().BeTrue($"{n} should be a power of 2");
			}
		}

		[TestMethod]
		public void IsPowerOf2_IfInvokedOnPowerOf2Plus1_ReturnsFalse()
		{
			for (var i = 0; i < 62; i++)
			{
				var n = (2L << i) + 1;
				n.IsPowerOf2().Should().BeFalse($"{n} should not be a power of 2");
			}
		}

		[TestMethod]
		public void IsPowerOf2_Negative1_ReturnsFalse()
		{
			(-1L).IsPowerOf2().Should().BeFalse();
		}

		[TestMethod]
		public void IsPowerOf2_0_ReturnsFalse()
		{
			0L.IsPowerOf2().Should().BeFalse();
		}

		[TestMethod]
		public void IsPowerOf2_1_ReturnsTrue()
		{
			1L.IsPowerOf2().Should().BeTrue();
		}
	}
}
