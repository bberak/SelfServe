using System;
using System.Collections.Generic;
using System.Linq;
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

        public static string ToLocalPath(this string rawUrl, string startupPath)
        {
            return startupPath + rawUrl.UrlDecode().Replace("/", @"\");
        }

        public static string ToUrl(this string path)
        {
            return path.UrlEncode().Replace(@"\", "/");
        }

        public static string ToUrl(this string fullPath, string startUpPath)
        {
            return fullPath.Replace(startUpPath, string.Empty).ToUrl();
        }
    }
}
