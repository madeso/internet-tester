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

        private readonly List<ExceptionHistory> history = new List<ExceptionHistory>();

        public void UpdateStatistics(Exception x)
        {
            var now = DateTime.Now;
            if (this.hasException)
            {
                if (x == null)
                {
                    this.history.Add(new ExceptionHistory { Exception=this.lastException, Started=this.exceptionDate, Ended=now });
                    this.hasException = false;
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