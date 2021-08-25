using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using InternetTester.Lib.Annotations;

namespace InternetTester.Lib.Tracked
{
	public class RecordedTimes: INotifyPropertyChanged
	{
		private SpanStatistics _statistics;
		public ObservableCollection<TimeEntry> Times { get; } = new ObservableCollection<TimeEntry>();

		public SpanStatistics Statistics
		{
			get => _statistics;
			set
			{
				if (value == _statistics) return;
				_statistics = value;
				OnPropertyChanged();
			}
		}

		public void Add(DateTime time, TimeSpan timeSpan)
		{
			Times.Add(new TimeEntry{Time = time, Span = timeSpan});
			UpdateStatistics();
		}

		private void UpdateStatistics()
		{
			var max = Times.Max(x => x.Span);
			var min = Times.Min(x => x.Span);
			var avg = TimeSpan.FromMilliseconds(Times.Average(x => x.Span.TotalMilliseconds));

			Statistics = new SpanStatistics {Max = max, Min = min, Avg = avg, Count = Times.Count};
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public class TimeEntry
	{
		public DateTime Time { get; set; }
		public TimeSpan Span { get; set; }
	}
}