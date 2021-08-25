using System;

namespace InternetTester.Lib.Tracked
{
	public class SpanStatistics
	{
		public int Count { get; set; }
		public TimeSpan Max { get; set; }
		public TimeSpan Min { get; set; }
		public TimeSpan Avg { get; set; }

		public double MaxMs => Max.TotalMilliseconds;
		public double MinMs => Min.TotalMilliseconds;
		public double AvgMs => Avg.TotalMilliseconds;
	}
}