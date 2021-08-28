using System.ComponentModel;
using System.Runtime.CompilerServices;
using InternetTester.Lib.Annotations;
using Newtonsoft.Json;

namespace InternetTester.Lib
{
	[JsonObject(MemberSerialization.OptIn)]
	public class NameAndCount : INotifyPropertyChanged
	{
		[JsonProperty(PropertyName = "count")]
		private int _count = 1;

		public NameAndCount(string name)
		{
			Name = name;
		}

		[JsonProperty(PropertyName = "name")]
		public string Name { get; }

		public int Count
		{
			get => _count;
			set
			{
				if (value == _count) return;
				_count = value;
				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}