using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace InternetTester.Lib
{
	[JsonObject(MemberSerialization.OptIn)]
    public class Data
    {
        [JsonProperty(PropertyName = "web")]
        public Tracked.Container Web { get; } = new Tracked.Container();

        [JsonProperty(PropertyName = "ping")]
        public Tracked.Container Ping { get; } = new Tracked.Container();

        public IEnumerable<Tracked.Container> Containers
        {
	        get
	        {
		        yield return Web;
		        yield return Ping;
	        }
        }

        public static string DefaultFilePath => Json.GetPathTo("history.json");

        public static Data Restore()
        {
            var f = DefaultFilePath;
            return false == File.Exists(f) ? null : Json.Load<Data>(f);
        }

        public void Backup()
        {
            Json.Save(this, DefaultFilePath);
        }

        public static Data CreateData()
        {
	        var d = Restore() ?? new Data();
	        return d;
        }
    }
}