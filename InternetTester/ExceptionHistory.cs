namespace InternetTester
{
    using System;

    using Newtonsoft.Json;

    internal class ExceptionHistory
    {
        [JsonProperty(PropertyName = "ended")]
        public DateTime Ended { get; set; }

        [JsonProperty(PropertyName = "exception")]
        public string Exception { get; set; }

        [JsonProperty(PropertyName = "started")]
        public DateTime Started { get; set; }

        public TimeSpan Span => this.Ended.Subtract(this.Started);

        public override string ToString()
        {
            var s = this.Span;
            return string.Format("{1} {3:hh\\:mm\\:ss} and {2} days: {0}", this.Exception, this.Started, (int)(s.TotalDays), s);
        }
    }
}