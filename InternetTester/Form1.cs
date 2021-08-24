using InternetTester.Lib;

namespace InternetTester
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows.Forms;

    public partial class Form1 : Form
    {
	    private readonly App _app;

	    public Form1()
	    {
		    this.InitializeComponent();
			this._app = new App
		    {
			    DisplayCallback = this.UpdateDisplay
		    };
	    }

	    private void UpdateDisplay(bool hasInternet, string message, string shortMessage, string exceptionString)
	    {
			this.dOuput.Text = message;
			this.dLastError.Text = hasInternet ? string.Empty : message;
			dNotify.Text = Lib.Util.MaxLength(64, hasInternet ? "Internet: OK" : shortMessage);
			this.Icon = dNotify.Icon = hasInternet ? Properties.Resources.main_icon : Properties.Resources.error_icon;
			this.dHistory.Text = exceptionString;
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			this._app.DisplayCallback = null;
		}
	}
}
