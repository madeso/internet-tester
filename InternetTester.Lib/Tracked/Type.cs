using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace InternetTester.Lib.Tracked
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum Type
	{
		[EnumMember(Value=TypeJson.Downtime)]
		Downtime,

		[EnumMember(Value = TypeJson.Uptime)]
		Uptime,

		[EnumMember(Value = TypeJson.Shutdown)]
		Shutdown
	}
}