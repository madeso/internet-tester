namespace InternetTester
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Cache;

    /// <summary> Utility functions for web handling. </summary>
    public static class Web
    {
        /// <summary>
        /// Downloads and saves a file. 
        /// </summary>
        /// <param name="url">
        /// what file to download 
        /// </param>
        /// <param name="target">
        /// where to save the file 
        /// </param>
        /// <param name="progress">
        /// The progress.
        /// </param>
        public static void DownloadFile( string url, string target, Action<long, long> progress )
        {
            progress(-1, -1);
            var request = GetWebRequest(url);

            // execute the request
            using( var response = (HttpWebResponse)request.GetResponse() )
            {
                // we will read data via the response stream
                using( var resStream = response.GetResponseStream() )
                {
                    if( resStream == null )
                    {
                        throw new Exception( "No response stream present" );
                    }

                    var total = resStream.CanSeek ? resStream.Length : response.ContentLength;

                    using( var fs = File.Open( target, FileMode.Create ) )
                    {
                        byte[] buf = new byte[1024]; // changed from 2^13=8192 to 2^10=1024, because the server might choke on large data for certain customers #11729
                        int count = 0;
                        long current = 0;
                        do
                        {
                            count = resStream.Read( buf, 0, buf.Length );
                            current += count;
                            if( progress != null )
                            {
                                progress( current, total );
                            }

                            if( count != 0 )
                            {
                                fs.Write( buf, 0, count );
                            }
                        }
                        while( count > 0 );
                    }
                }
            }
        }

        /// <summary>
        /// Escape html specific characters 
        /// </summary>
        /// <param name="str">
        /// normal string 
        /// </param>
        /// <returns>
        /// the html escaped string 
        /// </returns>
        public static string Escape( string str )
        {
            return str.Replace( " ", "%20" );
        }

        /// <summary>
        /// The url fix.
        /// </summary>
        /// <param name="abadurl">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string AddSlashAtTheEnd(string abadurl)
        {
            var badurl = abadurl.Trim();
            var sep = IsUrl(badurl) ? "/" : "\\";
            if (badurl.EndsWith(sep))
            {
                return badurl;
            }
            else
            {
                return badurl + sep;
            }
        }

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

            WebResponse response;
            WebException exp = null;

            var responseFromServer = "";

            try
            {
                response = request.GetResponse();
            }
            catch( WebException web )
            {
                exp = web;
                response = web.Response;
            }

            if (response != null)
            {
                var dataStream = response.GetResponseStream();
                if (dataStream != null)
                {
                    var reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                    reader.Close();
                    response.Close();
                }
            }

            return new FetchStringResult { Text = responseFromServer, Error = exp };
        }

        /// <summary>
        /// Checks if a text is a url or not 
        /// </summary>
        /// <param name="text">
        /// The text 
        /// </param>
        /// <returns>
        /// true if it is an url, false if not 
        /// </returns>
        public static bool IsUrl( string text )
        {
            return Uri.IsWellFormedUriString( text, UriKind.Absolute );
        }

        /// <summary>
        /// Launches the default browser navigating to the url. 
        /// </summary>
        /// <param name="url">
        /// The url 
        /// </param>
        public static void OpenUrl( string url )
        {
            try
            {
                Process.Start( url );
            }
            catch( Exception xx )
            {
                throw new Exception("While opening URL " + url, xx);
            }
        }

        /// <summary>
        /// Checks if a page exist 
        /// </summary>
        /// <param name="url">
        /// the url to test 
        /// </param>
        /// <returns>
        /// true if the page exist, false if not 
        /// </returns>
        public static bool PageExist( string url )
        {
            var request = GetWebRequest(url);

            // execute the request
            try
            {
                using( var response = (HttpWebResponse)request.GetResponse() )
                {
                    // we will read data via the response stream
                    using( var s = response.GetResponseStream() )
                    {
                        return true;
                    }
                }
            }
            catch( WebException ex )
            {
                if( ex.Status == WebExceptionStatus.ProtocolError )
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets a page as a string 
        /// </summary>
        /// <param name="url">
        /// The page 
        /// </param>
        /// <returns>
        /// The page that was returned 
        /// </returns>
        public static string FetchString(string url)
        {
            return FetchStringAdvanced(url).Text;
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

            // increase the timeout for sucky connections?
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
            /// The text.
            /// </summary>
            public string Text;

            /// <summary>
            /// The error.
            /// </summary>
            public WebException Error;
        }
    }
}
