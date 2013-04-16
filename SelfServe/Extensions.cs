using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace SelfServe
{
    public static class Extensions
    {
        public static string UrlDecode(this string me)
        {
            return HttpUtility.UrlDecode(me);
        }

        public static string UrlPathEncode(this string me)
        {
            return HttpUtility.UrlPathEncode(me);
        }

        public static string ToLocalPath(this string rawUrl, string rootPath)
        {
            return rootPath + rawUrl.UrlDecode().Replace("/", @"\");
        }

        public static string ToUrl(this string path)
        {
            return path.UrlPathEncode().Replace(@"\", "/");
        }

        public static string ToUrl(this string fullPath, string rootPath)
        {
            return fullPath.Replace(rootPath, string.Empty).ToUrl();
        }

        public static void WriteBytes(this HttpListenerResponse response, byte[] bytes)
        {
            response.ContentLength64 = bytes.Length;

            using (Stream s = response.OutputStream)
            {
                try
                {
                    s.Write(bytes, 0, bytes.Length);
                }
                catch (HttpListenerException)
                {
                    //-- Apparently, these exceptions are to be expected..
                    //-- See http://stackoverflow.com/questions/4801868/c-sharp-problem-with-httplistener
                }
            }
        }

        public static void WriteText(this HttpListenerResponse response, string output)
        {
            WriteBytes(response, ToBytes(output));
        }

        public static void WriteText(this HttpListenerResponse response, StringBuilder sb)
        {
            WriteText(response, sb.ToString());
        }

        public static byte[] ToBytes(this string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str); ;
            return bytes;
        }

        public static string GetContents(this WebResponse response)
        {
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                StringBuilder sb = new StringBuilder();
                string line = "";

                while ((line = reader.ReadLine()) != null)
                {
                    sb.Append(line);
                }

                return sb.ToString();
            }
        }

        public static void Fire(this EventHandler me, object sender, EventArgs e)
        {
            var myEvent = me;

            if (myEvent != null)
                myEvent(sender, e);
        }

        public static void Fire<T>(this EventHandler<T> me, object sender, T eventArgs) where T : EventArgs
        {
            var myEvent = me;

            if (myEvent != null)
                myEvent(sender, eventArgs);
        }
    }
}
