﻿using System;
using System.ComponentModel;
using System.Threading;

namespace InternetTester.Lib
{
	public class App
	{
        readonly Data _data = Data.CreateData();

        private readonly BackgroundWorker _worker;

        // hasInternet, message, shortMessage, error
        public Action<bool, string, string, string> DisplayCallback { get; set; }

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

            var r = Web.FetchStringAdvanced(Url);
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

        private void DisplayData()
        {
            var message = this._data.Exception ?? this._data.Output;
            var down = this._data.TotalDowntime;
            var downstr = down.HasValue ? string.Format("Downtime: {0}", down.Value) : string.Empty;
            var m = string.Format("Last date: {0}\r\n{1}\r\n{2}", this._data.Time.ToLongTimeString(), message, downstr);
            var sm = string.Format("Last date: {0}\r\n{1}", this._data.Time.ToLongTimeString(), downstr);
            

            var hasInternet = this._data.Exception == null;

            if (this.DisplayCallback != null)
            {
	            DisplayCallback(hasInternet, m, sm, this._data.ToExceptionString());
            }
        }

        private void UpdateData(object userState)
        {
            var exception = userState as Exception;
            var userMessage = userState as string;

            this._data.Update(userMessage, exception);

            this.DisplayData();
        }
    }
}