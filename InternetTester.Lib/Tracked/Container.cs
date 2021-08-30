using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace InternetTester.Lib.Tracked
{
	public class Container
	{
		[JsonProperty(PropertyName = "items")]
		public ObservableCollection<Item> Items { get; } = new ObservableCollection<Item>();

		public Item Latest => Items.Count == 0 ? null : Items[0];

		private Item AddNewItem(Type type, DateTime time)
		{
			var r = CreateItem(type, time);
			Items.Insert(0, r);
			return r;
		}

		private Item PushTime(Type type, DateTime time)
		{
			if (Items.Count == 0)
			{
				return AddNewItem(type, time);
			}

			var first = Items.First();

			// update the timing of the latest, regardless if it matches or not...
			// if it matched we have to update the current
			// if it doesn't the last item lasted up until this new measurement (and we need to update it... and then add a new)
			first.EndTime = time;

			if (first.Type == type)
			{
				return first;
			}

			return AddNewItem(type, time);
		}

		private static Item CreateItem(Type type, DateTime t)
		{
			switch (type)
			{
				case Type.Downtime: return new Downtime(t);
				case Type.Uptime: return new Uptime(t);
				case Type.Shutdown: return new Shutdown(t);
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}

		public void PushDowntime(DateTime time, string error)
		{
			var item = (Downtime) PushTime(Type.Downtime, time);
			item.Errors.Add(error);
		}

		public void PushUptime(DateTime time, string title)
		{
			var item = (Uptime) PushTime(Type.Uptime, time);
			item.Titles.Add(title);
		}

		public void PushUptime(DateTime time, TimeSpan span)
		{
			var item = (Uptime)PushTime(Type.Uptime, time);
			item.Times.Add(time, span);
		}
	}
}