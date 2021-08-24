namespace InternetTester.Lib
{
	public static class Util
	{
		public static string MaxLength(int length, string str)
		{
			if (str.Length < length)
			{
				return str;
			}

			return str.Substring(0, length - 1);
		}
	}
}