using System;

namespace InternetTester.Lib
{
	public class PingResult
	{
		public string Error { get; set; }
		public TimeSpan Time { get; set; }

		public static PingResult RunTest()
		{
			const string Ip = "8.8.8.8";

			var r = Ping.Run(Ip);

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
				var message = "ok";
				container.PushUptime(time, message);
			}
			else
			{
				container.PushDowntime(time, Error);
			}
		}
	}
}