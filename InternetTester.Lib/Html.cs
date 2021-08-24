using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetTester.Lib
{
	public class Html
	{
		public static string GetTitle(string um)
		{
			if (um == null) return string.Empty;

			var t = "<title>";
			var s = um.IndexOf(t, StringComparison.Ordinal);
			var e = um.IndexOf("</title>", s, StringComparison.Ordinal);

			if (s != -1 && e != -1)
			{
				s = s + t.Length;
				return um.Substring(s, e-s);
			}

			return null;
		}
	}
}
