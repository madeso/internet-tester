namespace InternetTester
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Newtonsoft.Json;

    public class Data
    {
        [JsonProperty(PropertyName = "exception")]
        public string Exception;

        [JsonProperty(PropertyName = "last_error_description")]
        public string LastError;

        [JsonProperty(PropertyName = "last_error_time")]
        public DateTime LastErrorTime;

        [JsonProperty(PropertyName = "output")]
        public string Output;

        [JsonProperty(PropertyName = "time")]
        public DateTime Time;

        [JsonProperty(PropertyName = "history")]
        private readonly List<ExceptionHistory> history = new List<ExceptionHistory>();

        [JsonProperty(PropertyName = "exception_date")]
        private DateTime exceptionDate;

        [JsonProperty(PropertyName = "has_exception")]
        private bool hasException;

        [JsonProperty(PropertyName = "last_exception")]
        private string lastException;

        [JsonProperty(PropertyName = "last_time")]
        private DateTime? lasttime;

        public static string DefaultFilePath
        {
            get
            {
                return Json.GetPathTo("app_data.json");
            }
        }

        public TimeSpan? TotalDowntime
        {
            get
            {
                if (this.hasException)
                {
                    var now = DateTime.Now;
                    var dt = now.Subtract(this.exceptionDate);
                    return dt;
                }
                return null;
            }
        }

        public static Data Restore()
        {
            var f = DefaultFilePath;
            return false == File.Exists(f) ? null : Json.Load<Data>(f);
        }

        public void Backup()
        {
            Json.Save(this, DefaultFilePath);
        }

        public string ToExceptionString()
        {
            return string.Join("\r\n", this.history.Select(x => x.ToString()));
        }

        public void UpdateStatistics(Exception x)
        {
            var now = DateTime.Now;
            if (this.hasException)
            {
                if (x == null)
                {
                    var ended = now;

                    var dt = now.Subtract(this.exceptionDate);
                    if (dt.TotalSeconds > 3)
                    {
                        if (this.lasttime.HasValue) { 
                            ended = this.lasttime.Value;
                        }
                    }

                    var h = new ExceptionHistory { Exception = this.lastException, Started = this.exceptionDate, Ended = ended };
                    if (h.Span.TotalSeconds >= 2) { 
                        this.history.Add(h);
                    }
                    this.hasException = false;
                    this.lasttime = null;
                }
                else
                {
                    this.lasttime = now;
                }
            }
            else
            {
                if (x != null)
                {
                    this.hasException = true;
                    this.lastException = x.Message;
                    this.exceptionDate = now;
                }
            }
        }
    }
}