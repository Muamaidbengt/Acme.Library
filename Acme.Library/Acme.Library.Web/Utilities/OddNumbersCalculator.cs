using System.Collections.Generic;

namespace Acme.Library.Utilities
{
	public class OddNumbersCalculator
	{
		public IEnumerable<short> GetPositiveOddNumbersSmallerThan(short someNumber)
		{
			for (short i = 1; i < someNumber && i > 0; i += 2)
				yield return i;
		}
	}
}