using System.ComponentModel;
using System.Runtime.CompilerServices;
using InternetTester.Lib.Annotations;

namespace InternetTester.Lib
{
	public class NameAndCount : INotifyPropertyChanged
	{
		private int _count = 1;

		public NameAndCount(string name)
		{
			Name = name;
		}

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