namespace Acme.Library.Utilities
{
	public static class NumericExtensions
	{
		public static bool IsPowerOf2(this long someNumber)
		{
			return (someNumber > 0) && ((someNumber & (someNumber - 1)) == 0);
		}
	}
}