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

        public TimeSpan Span
        {
            get
            {
                return this.Ended.Subtract(this.Started);
            }
        }

        public override string ToString()
        {
            return string.Format("{1} {2}: {0}: ", this.Exception, this.Started, this.Span);
        }
    }
}