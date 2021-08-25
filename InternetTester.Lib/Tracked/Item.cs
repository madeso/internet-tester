using System;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
using InternetTester.Lib.Annotations;
using Newtonsoft.Json;

namespace InternetTester.Lib.Tracked
{
	// [JsonConverter(typeof(JsonSubtypes), "type")]
	public abstract class Item : INotifyPropertyChanged
	{
		[JsonProperty(PropertyName = "end_time")]
		private DateTime _endTime;

		[JsonProperty(PropertyName = "start_time")]
		private DateTime _startTime;

		protected Item(DateTime t)
		{
			this.StartTime = t;
			this.EndTime = t;
		}

		public abstract Type Type { get; }

		public DateTime StartTime
		{
			get => _startTime;
			set
			{
				if (value.Equals(_startTime)) return;
				_startTime = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(Duration));
			}
		}

		public DateTime EndTime
		{
			get => _endTime;
			set
			{
				if (value.Equals(_endTime)) return;
				_endTime = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(Duration));
			}
		}

		public TimeSpan Duration => EndTime.Subtract(StartTime);

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}