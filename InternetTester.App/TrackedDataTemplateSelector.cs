using System;
using System.Windows;
using System.Windows.Controls;
using Type = InternetTester.Lib.Tracked.Type;

namespace InternetTester.App
{
	public class TrackedDataTemplateSelector : DataTemplateSelector
	{
		public DataTemplate Uptime { get; set; }
		public DataTemplate Downtime { get; set; }
		public DataTemplate Shutdown { get; set; }

		public override DataTemplate SelectTemplate(object obj, DependencyObject container)
		{
			var item = obj as Lib.Tracked.Item;
			if (item == null)
			{
				return base.SelectTemplate(obj, container);
			}

			switch (item.Type)
			{
				case Type.Downtime:
					return Downtime;
				case Type.Uptime:
					return Uptime;
				case Type.Shutdown:
					return Shutdown;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}