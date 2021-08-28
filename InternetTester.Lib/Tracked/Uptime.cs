using System;
using Newtonsoft.Json;

namespace InternetTester.Lib.Tracked
{
	// [JsonObject(MemberSerialization.OptIn)]
	public class Uptime : Item
	{
		[JsonProperty(PropertyName = "titles")]
		public CountedStrings Titles { get; } = new CountedStrings();

		[JsonProperty(PropertyName = "times")]
		public RecordedTimes Times { get; } = new RecordedTimes();

		public override Type Type => Type.Uptime;

		public Uptime(DateTime t) : base(t)
		{
		}
	}
}