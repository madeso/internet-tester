﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace InternetTester.Lib
{
	public class Data
    {
        [JsonProperty(PropertyName = "web")]
        public Tracked.Container Web { get; } = new Tracked.Container();

        [JsonProperty(PropertyName = "ping")]
        public Tracked.Container Ping { get; } = new Tracked.Container();

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
	        return Restore() ?? new Data();
        }
    }
}