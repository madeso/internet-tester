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
				WebResult.RunTest, r => r.Report(Data.Web)
			);
		}

        public Data Data { get; } = new Data();
    }
}