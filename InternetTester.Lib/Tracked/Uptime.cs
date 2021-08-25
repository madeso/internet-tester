using System;
using Newtonsoft.Json;

namespace InternetTester.Lib.Tracked
{
	public class Uptime : Item
	{
		[JsonProperty(PropertyName = "titles")]
		public CountedStrings Titles { get; } = new CountedStrings();

		public override Type Type => Type.Uptime;

		public Uptime(DateTime t) : base(t)
		{
		}
	}
}