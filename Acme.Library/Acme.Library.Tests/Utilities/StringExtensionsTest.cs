using Acme.Library.Utilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Acme.Library.Tests.Utilities
{
	[TestClass]
	public class StringExtensionsTest
	{
		[TestMethod]
		public void Reverse_IfNull_ReturnsNull()
		{
			string stringToTest = null;
			var result = stringToTest.Reverse();
			result.Should().BeNull();
		}

		[TestMethod]
		public void Reverse_EmptyString_ReturnsEmptyString()
		{
			string stringToTest = string.Empty;
			var result = stringToTest.Reverse();
			result.Should().Be(string.Empty);
		}

		[TestMethod]
		public void Reverse_Hello_ReturnsolleH()
		{
			var stringToTest = "Hello";
			var result = stringToTest.Reverse();
			result.Should().Be("olleH");
		}

		[TestMethod]
		public void ReverseReverse_ReturnsOriginal()
		{
			var stringToTest = "dskjfahlkjdsahflkjdslkhlkjasfdlkjhlsadAAsadASD&gfhfdsaruiet43y5898800v0 0 00fdb8fd7b7fd870b7";
			var result = stringToTest.Reverse().Reverse();
			result.Should().Be(stringToTest);
		}

		[TestMethod]
		public void Reverse_NordicCharacters_ReturnsReversedNordicCharacters()
		{
			var stringToTest = "a❤ÄÖåöäÅb";
			var result = stringToTest.Reverse();
			result.Should().Be("bÅäöåÖÄ❤a");
		}

		[TestMethod]
		public void Reverse_AsianCharacters_ReturnsReversedAsianCharacters()
		{
			var stringToTest = "田中さんにあげて下さい";
			var result = stringToTest.Reverse();
			result.Should().Be("いさ下てげあにんさ中田");
		}

		[TestMethod]
		public void Reverse_Emoji_ReturnsReversedEmoji()
		{
			// Curiously, this only works with some emoji...
			var stringToTest = "✌❤"; 
			var result = stringToTest.Reverse();
			
			// ... and even then, some assertion styles seem to just crash MSTest, e.g.
			//result.Should().Be("❤✌")
			//Assert.AreEqual("❤✌", result)

			Assert.IsTrue(string.Equals("❤✌", result)); 
		}

		[TestMethod]
		public void Replicate_Null_ReturnsNull()
		{
			string stringToTest = null;
			var result = stringToTest.Replicate(42);
			result.Should().BeNull();
		}

		[TestMethod]
		public void Replicate_0Times_ReturnsEmptyString()
		{
			var stringToTest = "Hi";
			var result = stringToTest.Replicate(0);
			result.Should().Be(string.Empty);
		}

		[TestMethod]
		public void Replicate_NegativeTimes_ThrowsArgumentException()
		{
			var stringToTest = "Hi";
			Action call = (() => stringToTest.Replicate(-1));
			call.ShouldThrow<ArgumentException>();
		}

		[TestMethod]
		public void Replicate_1Time_ReturnsOriginal()
		{
			var stringToTest = "Hi";
			var result = stringToTest.Replicate(1);
			result.Should().Be(stringToTest);
		}

		[TestMethod]
		public void Replicate_Hi3_ReturnsHiHiHi()
		{
			var stringToTest = "Hi";
			var result = stringToTest.Replicate(3);
			result.Should().Be("HiHiHi");
		}

		[TestMethod]
		public void Replicate_NordicCharacters_ReturnsReplicatedNordicCharacters()
		{
			var stringToTest = "åäöÖÄÅ";
			var result = stringToTest.Replicate(2);
			result.Should().Be("åäöÖÄÅåäöÖÄÅ");
		}
	}
}
