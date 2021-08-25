using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace InternetTester.Lib
{

	public class Worker<TResult> where TResult : class
	{
		private readonly BackgroundWorker _worker;
		private readonly Action<TResult> _onResult;

		public Worker(string name, Func<TResult> thread, Action<TResult> onResult)
		{
			this._onResult = onResult;

			this._worker = new BackgroundWorker { WorkerSupportsCancellation = true, WorkerReportsProgress = true };
			this._worker.ProgressChanged += (sender, args) => this.UpdateData(args.UserState);
			this._worker.DoWork += (sender, args) => TestInternetThread(name, args, thread, this._worker);
			this._worker.RunWorkerAsync();
		}

		private static void TestInternetThread(string name, DoWorkEventArgs args, Func<TResult> thread,
			BackgroundWorker worker)
		{
			var r = new Random();
			const int Sec = 1000;
			while (worker.CancellationPending == false)
			{
				var x = thread();
				worker.ReportProgress(0, x);
				var sleepTime = r.Next(Sec, Sec * 10);
				Debug.WriteLine("{0}: {1}", name, sleepTime);
				Thread.Sleep(x != null ? Sec : sleepTime);
			}
		}

		private void UpdateData(object userState)
		{
			if (userState is TResult result)
			{
				_onResult(result);
			}
		}
    }
}