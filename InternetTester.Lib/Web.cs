using System;
using System.IO;
using System.Net;
using System.Net.Cache;

namespace InternetTester.Lib
{
	/// <summary> Utility functions for web handling. </summary>
    public static class Web
    {
	    /// <summary>
        /// Gets a page as a string + result
        /// </summary>
        /// <param name="url">
        /// The page 
        /// </param>
        /// <returns>
        /// The page that was returned 
        /// </returns>
        public static FetchStringResult FetchStringAdvanced( string url )
        {
            var request = GetWebRequest(url);

            WebException exp = null;

            var responseFromServer = "";

            try
            {
	            var response = request.GetResponse();
	            var dataStream = response.GetResponseStream();

	            if (dataStream != null)
	            {
		            var reader = new StreamReader(dataStream);
		            responseFromServer = reader.ReadToEnd();
		            reader.Close();
		            response.Close();
	            }
            }
            catch (WebException web)
            {
                exp = web;
            }

            return new FetchStringResult { Text = responseFromServer, Error = exp };
        }

	    /// <summary>
        /// Creates a web request with our default parameters.
        /// </summary>
        /// <param name="url">the url</param>
        /// <returns>the created web request</returns>
        private static HttpWebRequest GetWebRequest(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            request.AllowAutoRedirect = true;
            request.Credentials = CredentialCache.DefaultCredentials;

            // might fix "the underlying connection was closed"
            // http://geekswithblogs.net/Denis/archive/2005/08/16/50365.aspx
            request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version10;
            
            // another solution to the exception problem
            // assigning a random connection group name solved it for some...
            request.ConnectionGroupName = Guid.NewGuid().ToString();

            // the exception might also be b/c a bad proxy, try disabling it

            // increase the timeout for bad connections?
            request.Timeout = 1 * 1000;
            request.ReadWriteTimeout = request.Timeout;

            return request;
        }

        /// <summary>
        /// The fetch string result.
        /// </summary>
        public struct FetchStringResult
        {
            /// <summary>
            /// The error.
            /// </summary>
            public WebException Error;

            /// <summary>
            /// The text.
            /// </summary>
            public string Text;
        }
    }
}
