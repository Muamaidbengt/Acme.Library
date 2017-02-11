using System;
using System.Text;

namespace Acme.Library.Utilities
{
	public static class StringExtensions
	{
		/// <summary>
		/// Reverses the contents of a string
		/// </summary>
		/// <example>"Hello".Reverse() yields "olleH"</example>
		public static string Reverse(this string original)
		{
			if (original == null)
				return null;

			var chars = original.ToCharArray();
			Array.Reverse(chars);
			return new string(chars);
		}

		/// <summary>
		/// Repeats a string a given number of times
		/// </summary>
		/// <example>"Hi".Replicate(3) yields "HiHiHi"</example>
		public static string Replicate(this string original, int times)
		{
			if (times < 0)
				throw new ArgumentException("Must be non-negative number", nameof(times));
			if (original == null)
				return null;
			
			var sb = new StringBuilder();
			for (var i = 0; i < times; i++)
				sb.Append(original);
			return sb.ToString();
		}
	}
}