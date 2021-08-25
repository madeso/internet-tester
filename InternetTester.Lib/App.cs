using System;
using System.ComponentModel;
using System.Threading;

namespace InternetTester.Lib
{
	public class App
	{
		public App()
		{
			var webWorker = new Worker<WebResult>
			(
				"web", WebResult.RunTest, r => r.Report(Data.Web)
			);

			var pingWorker = new Worker<PingResult>
			(
				"ping", PingResult.RunTest, r => r.Report(Data.Ping)
			);
		}

        public Data Data { get; } = new Data();
    }
}