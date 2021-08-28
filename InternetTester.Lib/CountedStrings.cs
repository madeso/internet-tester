using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace InternetTester.Lib
{
	[JsonObject(MemberSerialization.OptIn)]
	public class CountedStrings
	{
		private Dictionary<string, NameAndCount> _data;

		[JsonProperty(PropertyName = "items")]
		public ObservableCollection<NameAndCount> Items { get; set; } = new ObservableCollection<NameAndCount>();

		public void Add(string s)
		{
			MakeSureDataExists();
			if (_data.ContainsKey(s))
			{
				_data[s].Count += 1;
			}
			else
			{
				var count = new NameAndCount(s);
				_data[s] = count;
				Items.Add(count);
			}
		}

		private void MakeSureDataExists()
		{
			if (this._data != null)
			{
				return;
			}

			this._data = new Dictionary<string, NameAndCount>();
			foreach(var k in this.Items)
			{
				_data.Add(k.Name, k);
			}
		}
	}
}