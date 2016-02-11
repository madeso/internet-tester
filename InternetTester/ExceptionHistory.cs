namespace InternetTester
{
    using System;

    internal class ExceptionHistory
    {
        public string Exception { get; set; }

        public DateTime Started { get; set; }

        public DateTime Ended { get; set; }

        public override string ToString()
        {
            return string.Format("{1} {2}: {0}: ", this.Exception, this.Started, this.Ended.Subtract(this.Started));
        }
    }
}