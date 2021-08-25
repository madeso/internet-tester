using System;
using System.Text.RegularExpressions;

namespace InternetTester.Lib
{
	public static class Ping
	{
		public class Result
		{
			public string Error { get; set; }
			public TimeSpan Time { get; set; }
			public int Bytes { get; set; }
			public int Ttl { get; set; }
			public string Sender { get; set; }
		}

		public static Result Run(string ip)
		{
			var lines = Console.Run("ping", $"{ip} -n 1");
			return Parse(lines);
		}

		// Reply from 8.8.8.8: bytes=32 time=927ms TTL=112
		private static readonly Regex ReplyRegex = new Regex(@"Reply from ([^:]+): bytes=([0-9]+) time=([0-9]+)ms TTL=([0-9]+)");

		private static Result Parse(string[] lines)
		{
			var output = lines[1];

			var match = ReplyRegex.Match(output);

			if (match.Success)
			{
				var sender = match.Groups[1].Value;
				var bytes = match.Groups[2].Value;
				var time = match.Groups[3].Value;
				var ttl = match.Groups[4].Value;
				var r = new Result
				{
					Sender = sender,
					Bytes = int.Parse(bytes),
					Time = TimeSpan.FromMilliseconds(int.Parse(time)),
					Ttl = int.Parse(ttl)
				};
				return r;
			}

			return new Result {Error = output};
		}
	}
}