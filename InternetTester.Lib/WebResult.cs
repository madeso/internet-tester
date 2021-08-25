using System;
using System.Net;

namespace InternetTester.Lib
{
	public class WebResult
	{
		public WebException Error { get; set; }
		public string Message { get; set; }

		public static WebResult RunTest()
		{
			const string Url = "https://www.google.se";

			var r = Lib.Web.FetchStringAdvanced(Url);

			if (r.Error != null)
			{
				return new WebResult {Error = r.Error};
			}
			else
			{
				return new WebResult {Message = r.Text};
			}
		}

		public void Report(Tracked.Container container)
		{
			var time = DateTime.Now;

			if (Error == null)
			{
				var message = Html.GetTitle(Message) ?? Message;
				container.PushUptime(time, message);
			}
			else
			{
				container.PushDowntime(time, Error.Message);
			}
		}
	}
}