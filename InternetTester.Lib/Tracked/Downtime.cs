using System;
using Newtonsoft.Json;

namespace InternetTester.Lib.Tracked
{
	[JsonObject(MemberSerialization.OptIn)]
	public class Downtime : Item
	{
		[JsonProperty(PropertyName = "errors")]
		public CountedStrings Errors {get;} = new CountedStrings();

		public override Type Type => Type.Downtime;

		public Downtime(DateTime t) : base(t)
		{
		}
	}
}