namespace InternetTester
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Data
    {
        public string Output;
        public string Exception;
        public DateTime Time;

        public string LastError;
        public DateTime LastErrorTime;

        private bool hasException = false;
        private string lastException;
        private DateTime exceptionDate;
        DateTime? lasttime = null;

        private readonly List<ExceptionHistory> history = new List<ExceptionHistory>();

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
                else
                {
                    return null;
                }
            }

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

                    this.history.Add(new ExceptionHistory { Exception=this.lastException, Started=this.exceptionDate, Ended=ended });
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

        public string ToExceptionString()
        {
            return string.Join("\r\n", this.history.Select(x => x.ToString()));
        }

        public static string DefaultFilePath
        {
            get
            {
                return Json.GetPathTo("app_data.json");
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
    }
}