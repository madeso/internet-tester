using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using InternetTester.Lib.Annotations;
using Newtonsoft.Json;

namespace InternetTester.Lib.Tracked
{
	[JsonObject(MemberSerialization.OptIn)]
	public class RecordedTimes: INotifyPropertyChanged
	{
		private SpanStatistics _statistics;
		private ObservableCollection<TimeEntry> _times = new ObservableCollection<TimeEntry>();

		[JsonProperty(PropertyName = "times")]
		public ObservableCollection<TimeEntry> Times
		{
			get => _times;
			set
			{
				if (Equals(value, _times)) return;
				_times = value;
				OnPropertyChanged();
				UpdateStatistics();
			}
		}

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