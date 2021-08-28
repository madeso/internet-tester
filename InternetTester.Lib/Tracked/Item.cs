using System;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
using InternetTester.Lib.Annotations;
using JsonSubTypes;
using Newtonsoft.Json;

namespace InternetTester.Lib.Tracked
{
	[JsonConverter(typeof(JsonSubtypes), "type")]
	[JsonSubtypes.KnownSubType(typeof(Uptime), TypeJson.Uptime)]
	[JsonSubtypes.KnownSubType(typeof(Downtime), TypeJson.Downtime)]
	[JsonSubtypes.KnownSubType(typeof(Shutdown), TypeJson.Shutdown)]

	// [JsonObject(MemberSerialization.OptIn)]
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

		[JsonProperty(PropertyName = "type")]
		public abstract Type Type { get; }

		[JsonIgnore]
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

		[JsonIgnore]
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

		[JsonIgnore]
		public TimeSpan Duration => EndTime.Subtract(StartTime);

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}