using System;
using System.Diagnostics;

namespace InternetTester.Lib
{
	public static class Console
	{
		public static string[] Run(string app, string args)
		{
			using (var p = new Process())
			{
				p.StartInfo.FileName = app;
				p.StartInfo.Arguments = args;
				p.StartInfo.UseShellExecute = false;
				p.StartInfo.RedirectStandardOutput = true;
				p.StartInfo.CreateNoWindow = true;

				p.Start();

				p.WaitForExit();
				return p.StandardOutput.ReadToEnd().Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			}
		}
	}
}