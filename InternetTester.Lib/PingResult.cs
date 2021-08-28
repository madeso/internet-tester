using System;

namespace InternetTester.Lib
{
	public class PingResult : IResult
	{
		public string Error { get; set; }
		public TimeSpan Time { get; set; }

		public static PingResult RunTest()
		{
			const string ip = "8.8.8.8";

			var r = Ping.Run(ip);

			if (r.Error != null)
			{
				return new PingResult { Error = r.Error };
			}
			else
			{
				return new PingResult { Time = r.Time};
			}
		}

		public void Report(Tracked.Container container)
		{
			var time = DateTime.Now;

			if (string.IsNullOrEmpty(Error))
			{
				container.PushUptime(time, Time);
			}
			else
			{
				container.PushDowntime(time, Error);
			}
		}

		public bool IsError => Error != null;
	}
}