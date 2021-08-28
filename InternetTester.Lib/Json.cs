using System;
using System.Globalization;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace InternetTester.Lib
{
	public static class Json
	{
		public static JsonSerializerSettings DefaultJsonSerializerSettings()
		{
			var serializer = new JsonSerializerSettings{};
			serializer.Converters.Add(
				new Newtonsoft.Json.Converters.StringEnumConverter { AllowIntegerValues = true, NamingStrategy = new CamelCaseNamingStrategy() });
			return serializer;
		}

		public static string GetPathTo(string file)
		{
			var dir = OnlyGetPathTo("");
			if (false == Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}

			var path = OnlyGetPathTo(file);
			return path;
		}

		public static TData Load<TData>(string file)
		{
			var str = File.ReadAllText(file, Encoding.UTF8);
			var obj = JsonConvert.DeserializeObject<TData>(str, DefaultJsonSerializerSettings());
			return obj;
		}

		public static string OnlyGetPathTo(string file)
		{
			var sep = Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);
			var dir = string.Join(
				sep,
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				"madeso",
				"InternetTester");
			return string.IsNullOrEmpty(file) ? dir : string.Join(sep, dir, file);
		}

		public static void Save<TData>(TData data, string file)
		{
			var s = JsonConvert.SerializeObject(data, Formatting.Indented, DefaultJsonSerializerSettings());
			File.WriteAllText(file, s, Encoding.UTF8);
		}
	}
}