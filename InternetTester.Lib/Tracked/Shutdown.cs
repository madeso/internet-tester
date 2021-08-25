using System;

namespace InternetTester.Lib.Tracked
{
	public class Shutdown : Item
	{
		public Shutdown(DateTime t) : base(t)
		{
		}

		public override Type Type => Type.Shutdown;
	}
}