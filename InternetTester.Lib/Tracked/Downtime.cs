using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace InternetTester.Lib.Tracked
{
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