using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InternetTester
{
    using System;
    using System.Net;
    using System.Threading;
    using System.Windows.Forms.VisualStyles;

    public partial class Form1 : Form
    {
        private BackgroundWorker bw;

        readonly Data data = CreateData();

        private static Data CreateData()
        {
            return Data.Restore() ?? new Data();
        }

        public Form1()
        {
            this.InitializeComponent();
            this.bw = new BackgroundWorker { WorkerSupportsCancellation = true, WorkerReportsProgress = true };
            this.bw.ProgressChanged += (sender, args) => this.UpdateData(args.UserState);
            this.bw.DoWork += (sender, args) => TestInternetThread(args, this.bw);
            this.bw.RunWorkerAsync();
        }

        private void UpdateData(object userState)
        {
            var x = userState as Exception;
            var um = userState as string;

            this.data.Time = DateTime.Now;
            this.data.Output = GetTitle(um) ?? um;
            this.data.Exception = x != null ? x.Message : null;

            if (this.data.Exception != null)
            {
                this.data.LastErrorTime = DateTime.Now;
                this.data.LastError = this.data.Exception;
            }

            this.data.Backup();
            this.data.UpdateStatistics(x);

            this.DisplayData();
        }

        private void DisplayData()
        {
            var message = this.data.Exception ?? (this.data.Output);
            var down = this.data.TotalDowntime;
            var downstr = down.HasValue ? string.Format("Downtime: {0}", down.Value) : string.Empty;
            var m = string.Format("Last date: {0}\r\n{1}\r\n{2}", this.data.Time.ToLongTimeString(), message, downstr);
            this.dOuput.Text = m;

            if (this.data.Exception != null)
            {
                this.dLastError.Text = m;
            }

            this.dHistory.Text = this.data.ToExceptionString();
        }

        private static string GetTitle(string um)
        {
            if (um == null) return string.Empty;

            var t = "<title>";
            var s = um.IndexOf(t, System.StringComparison.Ordinal);
            var e = um.IndexOf("</title>", s, System.StringComparison.Ordinal);

            if (s != -1 && e != -1)
            {
                s = s + t.Length;
                return um.Substring(s, e-s);
            }

            return null;
        }

        private static void TestInternetThread(DoWorkEventArgs args, BackgroundWorker worker)
        {
            var r = new Random();
            const int Sec = 1000;
            while (worker.CancellationPending == false)
            {
                string res;
                var x = TestInternet(out res);
                worker.ReportProgress(0, x == null? (object)res : (object)x);
                Thread.Sleep(x != null ? Sec : r.Next(Sec, Sec * 10));
            }
        }

        private static Exception TestInternet(out string res)
        {
            const string Url = "https://www.google.se";

            var r = Web.FetchStringAdvanced(Url);
            res = r.Text;
            return r.Error ?? null;
        }
    }
}
