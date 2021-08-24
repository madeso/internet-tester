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
        private readonly List<ExceptionHistory> _history = new List<ExceptionHistory>();

        [JsonProperty(PropertyName = "exception_date")]
        private DateTime _exceptionDate;

        [JsonProperty(PropertyName = "has_exception")]
        private bool _hasException;

        [JsonProperty(PropertyName = "last_exception")]
        private string _lastException;

        [JsonProperty(PropertyName = "last_time")]
        private DateTime? _lastTime;

        public static string DefaultFilePath => Json.GetPathTo("app_data.json");

        public TimeSpan? TotalDowntime
        {
            get
            {
	            if (!this._hasException) return null;

	            var now = DateTime.Now;
                var dt = now.Subtract(this._exceptionDate);
                return dt;
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
            return string.Join("\r\n", this._history.Select(x => x.ToString()));
        }

        public void UpdateStatistics(Exception x)
        {
            var now = DateTime.Now;
            if (this._hasException)
            {
                if (x == null)
                {
                    var ended = now;

                    var dt = now.Subtract(this._exceptionDate);
                    if (dt.TotalSeconds > 3)
                    {
                        if (this._lastTime.HasValue) { 
                            ended = this._lastTime.Value;
                        }
                    }

                    var h = new ExceptionHistory { Exception = this._lastException, Started = this._exceptionDate, Ended = ended };
                    if (h.Span.TotalSeconds >= 2) { 
                        this._history.Add(h);
                    }
                    this._hasException = false;
                    this._lastTime = null;
                }
                else
                {
                    this._lastTime = now;
                }
            }
            else
            {
                if (x != null)
                {
                    this._hasException = true;
                    this._lastException = x.Message;
                    this._exceptionDate = now;
                }
            }
        }
    }
}