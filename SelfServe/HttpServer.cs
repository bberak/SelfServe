using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SelfServe
{
    public class HttpServer : IDisposable
    {
        public const string DEFAULT_PREFIX = "http://+/";
        public bool IsRunning { get; private set; }

        private readonly HttpListener Listener;
        private readonly string StartUpPath;

        public HttpServer(params string[] prefixes)
        {
            if (prefixes == null || prefixes.Length == 0)
            {
                throw new ArgumentException("The prefixes argument cannot be empty! Use the default constructor if you are stuck.");
            }

            Listener = new HttpListener();
            StartUpPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            IsRunning = false;

            foreach (string prefix in prefixes)
            {
                Listener.Prefixes.Add(prefix);
            } 
        }

        public HttpServer()
            : this(DEFAULT_PREFIX)
        {
        }

        public void Start()
        {
            Listener.Start();
            IsRunning = true;
            new TaskFactory().StartNew(BeginLoop);

            Console.WriteLine("OK, server is ready!");
        }

        private void BeginLoop()
        {
            while (IsRunning)
            {
                try
                {
                    ThreadPool.QueueUserWorkItem(ProcessObject, Listener.GetContext());
                }
                catch (Exception ex) 
                {
                    OnException(ex);
                }
            }
        }

        private void ProcessObject(object o)
        {
            var context = o as HttpListenerContext;

            ProcessRequest(context);
        }

        protected virtual void ProcessRequest(HttpListenerContext context)
        {
            string path = context.Request.RawUrl.MapPath(StartUpPath);

            using (HttpListenerResponse response = context.Response)
            {
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
        }

        protected virtual void OnFileFound(HttpListenerResponse response, string filePath)
        {
            Console.WriteLine(string.Format("Client requested file ({0})... Found", filePath));

            var file = File.ReadAllBytes(filePath);
            WriteBytes(response, file);
        }

        protected virtual void OnDirectoryFound(HttpListenerResponse response, string currentDirPath)
        {
            Console.WriteLine(string.Format("Client requested directory ({0})... Found", currentDirPath));

            StringBuilder sb = new StringBuilder();
            sb.Append("<!DOCTYPE HTML><html><head></head><body><ul>");

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

            WriteHtml(response, sb.ToString());
        }

        protected virtual void OnNotFound(HttpListenerResponse response, string path)
        {
            Console.WriteLine(string.Format("Client requested path ({0})... Not found", path));

            var error = "<!DOCTYPE HTML><html><head></head><body><h1>Path Not Found</h1></body></html>";
            WriteHtml(response, error, HttpStatusCode.NotFound);
        }

        protected virtual void OnException(Exception ex)
        {

        }

        protected virtual void WriteBytes(HttpListenerResponse response, byte[] file, HttpStatusCode status = HttpStatusCode.OK)
        {
            response.StatusCode = (int)status;
            response.ContentLength64 = file.Length;
            using (Stream s = response.OutputStream)
            {
                s.Write(file, 0, file.Length);
            }
        }

        protected virtual void WriteHtml(HttpListenerResponse response, string output, HttpStatusCode status = HttpStatusCode.OK)
        {
            response.KeepAlive = true;
            response.ContentType = "text/html; charset=UTF-8";
            WriteBytes(response, GetBytes(output), status);
        }

        protected virtual byte[] GetBytes(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str); ;
            return bytes;
        }

        protected virtual string UrlDecode(string value)
        {
            return HttpUtility.UrlDecode(value);
        }

        protected virtual string UrlEncode(string value)
        {
            return HttpUtility.UrlEncode(value);
        }

        public void Dispose()
        {
            IsRunning = false;
            Listener.Stop();
            Listener.Close();

            Console.WriteLine("Server has stopped!");
        }
    }
}
