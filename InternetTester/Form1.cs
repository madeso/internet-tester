namespace InternetTester
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows.Forms;

    public partial class Form1 : Form
    {
        readonly Data data = CreateData();

        private readonly BackgroundWorker bw;

        public Form1()
        {
            this.InitializeComponent();
            this.bw = new BackgroundWorker { WorkerSupportsCancellation = true, WorkerReportsProgress = true };
            this.bw.ProgressChanged += (sender, args) => this.UpdateData(args.UserState);
            this.bw.DoWork += (sender, args) => TestInternetThread(args, this.bw);
            this.bw.RunWorkerAsync();
        }

        private static Data CreateData()
        {
            return Data.Restore() ?? new Data();
        }

        private static string GetTitle(string um)
        {
            if (um == null) return string.Empty;

            var t = "<title>";
            var s = um.IndexOf(t, StringComparison.Ordinal);
            var e = um.IndexOf("</title>", s, StringComparison.Ordinal);

            if (s != -1 && e != -1)
            {
                s = s + t.Length;
                return um.Substring(s, e-s);
            }

            return null;
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
                worker.ReportProgress(0, x == null? res : (object)x);
                Thread.Sleep(x != null ? Sec : r.Next(Sec, Sec * 10));
            }
        }

        private void DisplayData()
        {
            var message = this.data.Exception ?? this.data.Output;
            var down = this.data.TotalDowntime;
            var downstr = down.HasValue ? string.Format("Downtime: {0}", down.Value) : string.Empty;
            var m = string.Format("Last date: {0}\r\n{1}\r\n{2}", this.data.Time.ToLongTimeString(), message, downstr);
            var sm = string.Format("Last date: {0}\r\n{1}", this.data.Time.ToLongTimeString(), downstr);
            this.dOuput.Text = m;

            var hasInternet = this.data.Exception == null;

            this.dLastError.Text = hasInternet ? string.Empty : m;
            dNotify.Text = MaxLength(64, hasInternet ? "Internet: OK" : sm);
            this.Icon = dNotify.Icon = hasInternet ? Properties.Resources.main_icon : Properties.Resources.error_icon;

            this.dHistory.Text = this.data.ToExceptionString();
        }

        private static string MaxLength(int length, string str)
        {
            if (str.Length < length) return str;
            else return str.Substring(0, length-1);
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
    }
}
