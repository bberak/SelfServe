using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;

namespace SelfServe
{
    public class HttpFileServer : HttpServer
    {
        public HttpFileServer(params string[] prefixes)
            : base(prefixes) { }

        protected override void ProccessContext(HttpListenerContext context)
        {
            HttpListenerResponse response = context.Response;

            string path = context.Request.RawUrl.ToLocalPath(StartUpPath);

            if (File.Exists(path))
            {
                OnFileFound(response, path);
            }
            else if (Directory.Exists(path))
            {
                OnDirectoryFound(response, path);
            }
            else
            {
                OnNotFound(response, path);
            }
        }

        protected virtual void OnFileFound(HttpListenerResponse response, string filePath)
        {
            Console.WriteLine(string.Format("Client requested file ({0})... Found", filePath));

            var file = File.ReadAllBytes(filePath);
            response.WriteBytes(file);
        }

        protected virtual void OnDirectoryFound(HttpListenerResponse response, string currentDirPath)
        {
            Console.WriteLine(string.Format("Client requested directory ({0})... Found", currentDirPath));

            StringBuilder sb = new StringBuilder();
            sb.Append("<!DOCTYPE HTML><html><head><title>Directory Listing</title></head><body><ul>");

            foreach (string subDirPath in Directory.EnumerateDirectories(currentDirPath))
            {
                string subDirUrl = subDirPath.ToUrl(StartUpPath);
                string subDirName = Path.GetFileName(subDirPath);

                sb.Append(
                    string.Format("<li><b><a href=\"{0}\">{1}</a></b></li>",
                        subDirUrl,
                        subDirName)
                );
            }

            foreach (string filePath in Directory.EnumerateFiles(currentDirPath))
            {
                string filePathUrl = filePath.ToUrl(StartUpPath);
                string filenName = Path.GetFileName(filePath);

                sb.Append(
                    string.Format("<li><a href=\"{0}\">{1}</a></li>",
                        filePathUrl,
                        filenName)
                );
            }

            sb.Append("</ul></body></html>");
            response.WriteHtml(sb);
        }

        protected virtual void OnNotFound(HttpListenerResponse response, string path)
        {
            Console.WriteLine(string.Format("Client requested path ({0})... Not found", path));

            var error = "<!DOCTYPE HTML><html><head><title>Path Not Found</title></head><body><h1>Path Not Found</h1></body></html>";
            response.WriteHtml(error, HttpStatusCode.NotFound);
        }
    }
}
