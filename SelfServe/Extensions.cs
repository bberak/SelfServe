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

        public static string UrlEncode(this string me)
        {
            return HttpUtility.UrlPathEncode(me);
        }

        public static string ToLocalPath(this string rawUrl, string rootPath)
        {
            return rootPath + rawUrl.UrlDecode().Replace("/", @"\");
        }

        public static string ToUrl(this string path)
        {
            return path.UrlEncode().Replace(@"\", "/");
        }

        public static string ToUrl(this string fullPath, string rootPath)
        {
            return fullPath.Replace(rootPath, string.Empty).ToUrl();
        }

        public static void WriteBytes(this HttpListenerResponse response, byte[] bytes, HttpStatusCode status = HttpStatusCode.OK)
        {
            response.StatusCode = (int)status;
            response.ContentLength64 = bytes.Length;
            using (Stream s = response.OutputStream)
            {
                s.Write(bytes, 0, bytes.Length);
            }
        }

        public static void WriteHtml(this HttpListenerResponse response, string output, HttpStatusCode status = HttpStatusCode.OK)
        {
            response.ContentType = "text/html; charset=UTF-8";
            WriteBytes(response, ToBytes(output), status);
        }

        public static void WriteHtml(this HttpListenerResponse response, StringBuilder sb, HttpStatusCode status = HttpStatusCode.OK)
        {
            WriteHtml(response, sb.ToString(), status);
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
