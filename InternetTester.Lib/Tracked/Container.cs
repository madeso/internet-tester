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
			if (first.Type == type)
			{
				first.EndTime = time;
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