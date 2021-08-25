using System;
using System.ComponentModel;
using System.Threading;

namespace InternetTester.Lib
{
	public class App
	{
		private readonly BackgroundWorker _worker;

        public App()
        {
	        this._worker = new BackgroundWorker { WorkerSupportsCancellation = true, WorkerReportsProgress = true };
            this._worker.ProgressChanged += (sender, args) => this.UpdateData(args.UserState);
            this._worker.DoWork += (sender, args) => TestInternetThread(args, this._worker);
            this._worker.RunWorkerAsync();
        }

        private static Exception TestInternet(out string res)
        {
            const string Url = "https://www.google.se";

            var r = Lib.Web.FetchStringAdvanced(Url);
            res = r.Text;
            return r.Error ?? null;
        }

        private static void TestInternetThread(DoWorkEventArgs args, BackgroundWorker worker)
        {
            var r = new Random();
            const int Sec = 1000;
            while (worker.CancellationPending == false)
            {
                string res;
                var x = TestInternet(out res);
                worker.ReportProgress(0, x == null ? res : (object)x);
                Thread.Sleep(x != null ? Sec : r.Next(Sec, Sec * 10));
            }
        }

        public Tracked.Container Web { get; } = new Tracked.Container();

        private void UpdateData(object userState)
        {
            var userMessage = userState as string;

            var time = DateTime.Now;

            if (userMessage != null)
            {
	            var message = Html.GetTitle(userMessage) ?? userMessage;
                Web.PushUptime(time, message);
            }
            else
            {
	            var exception = (Exception) userState;
                Web.PushDowntime(time, exception.Message);
            }
        }
    }
}