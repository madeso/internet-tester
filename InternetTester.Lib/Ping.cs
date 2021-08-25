using System;

namespace InternetTester.Lib
{
	public static class Ping
	{
		public class Result
		{
			public string Error { get; set; }
			public TimeSpan Time { get; set; }
		}

		public static Result Run(string ip)
		{
			var lines = Console.Run("ping", $"{ip} -n 1");
			return Parse(lines);
		}

		private static Result Parse(string[] lines)
		{
			var output = lines[1];

			if (output.Contains("time="))
			{
				return new Result {Time = TimeSpan.FromMilliseconds(10)};
			}

			return new Result {Error = output};
		}
	}
}