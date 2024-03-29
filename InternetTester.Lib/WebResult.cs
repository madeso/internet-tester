﻿using System;
using System.Net;
using InternetTester.Lib.Annotations;

namespace InternetTester.Lib
{
	public class WebResult : IResult
	{
		public WebException Error { get; set; }
		public string Message { get; set; }

		public static WebResult RunTest()
		{
			const string url = "https://www.google.se";

			var r = Lib.Web.FetchStringAdvanced(url);

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

		public bool IsError => Error != null;
	}
}