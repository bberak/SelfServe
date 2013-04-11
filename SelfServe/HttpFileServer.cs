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
        public HttpFileServer(string[] prefixes = null, string rootPath = "")
            : base(prefixes, rootPath) { }

        protected override void ProccessContext(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            string path = request.RawUrl.ToLocalPath(RootPath);

            if (File.Exists(path))
            {
                OnFileFound(path, response);
            }
            else if (Directory.Exists(path))
            {
                OnDirectoryFound(path, response);
            }
            else
            {
                OnPathNotFound(request, response);
            }
        }

        protected virtual void OnFileFound(string filePath, HttpListenerResponse response)
        {
            Log("Client requested file ({0})... Found", filePath);

            var file = File.ReadAllBytes(filePath);
            response.WriteBytes(file);
        }

        protected virtual void OnDirectoryFound(string currentDirPath, HttpListenerResponse response)
        {
            Log("Client requested directory ({0})... Found", currentDirPath);

            StringBuilder sb = new StringBuilder();
            sb.Append("<!DOCTYPE HTML><html><head><title>Directory Listing</title></head><body><ul>");

            foreach (string subDirPath in Directory.EnumerateDirectories(currentDirPath))
            {
                string subDirUrl = subDirPath.ToUrl(RootPath);
                string subDirName = Path.GetFileName(subDirPath);

                sb.Append(
                    string.Format("<li><b><a href=\"{0}\">{1}</a></b></li>",
                        subDirUrl,
                        subDirName)
                );
            }

            foreach (string filePath in Directory.EnumerateFiles(currentDirPath))
            {
                string filePathUrl = filePath.ToUrl(RootPath);
                string filenName = Path.GetFileName(filePath);

                sb.Append(
                    string.Format("<li><a href=\"{0}\">{1}</a></li>",
                        filePathUrl,
                        filenName)
                );
            }

            sb.Append("</ul></body></html>");

            response.ContentType = "text/html; charset=UTF-8";
            response.WriteText(sb);
        }

        protected virtual void OnPathNotFound(HttpListenerRequest request, HttpListenerResponse response)
        {
            Log("Client requested path ({0})... Not found", request.RawUrl);

            var error = @"<!DOCTYPE HTML>
                          <html>
                             <head>
                                <title>Path Not Found</title>
                             </head>
                             <body>
                                <h1>Path Not Found</h1>
                             </body>
                          </html>";

            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.WriteText(error);
        }
    }
}
