using System;
using System.ComponentModel;
using System.Threading;
using Type = InternetTester.Lib.Tracked.Type;

namespace InternetTester.Lib
{
	public class App
	{
		public App(Action<AppStatus> onStatus)
		{
			this.OnStatus = onStatus;

			var webWorker = new Worker<WebResult>
			(
				"web", WebResult.RunTest, r =>
				{
					r.Report(Data.Web);
					this.UpdateStatus();
				});

			var pingWorker = new Worker<PingResult>
			(
				"ping", PingResult.RunTest, r =>
				{
					r.Report(Data.Ping);
					this.UpdateStatus();
				});
		}

		private void UpdateStatus()
		{
			OnStatus(CalculateStatus());
		}

		private AppStatus CalculateStatus()
		{
			Type? all = null;

			foreach (var c in Data.Containers)
			{
				if (c.Latest == null)
				{
					continue;
				}

				if (all == null)
				{
					all = c.Latest.Type;
				}

				if (all != c.Latest.Type)
				{
					return AppStatus.Unsure;
				}
			}

			switch (all)
			{
				case Type.Downtime:
					return AppStatus.Down;
				case Type.Uptime:
					return AppStatus.Up;
				case Type.Shutdown:
					return AppStatus.Shutdown;
				case null:
					return AppStatus.Unsure;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public Action<AppStatus> OnStatus { get; set; }

		public Data Data { get; } = new Data();
    }
}