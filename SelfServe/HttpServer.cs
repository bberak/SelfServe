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
    public class HttpServer : IDisposable
    {
        public const string WILDCARD_PREFIX = "http://+/";

        private readonly HttpListener Listener;
        protected readonly string RootPath;

        public HttpServer(string[] prefixes = null, string rootPath = "")
        {
            if (prefixes == null || prefixes.Length == 0)
            {
                prefixes = new string[] { WILDCARD_PREFIX };
            }

            if (string.IsNullOrEmpty(rootPath))
            {
                rootPath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            }

            Listener = new HttpListener();
            prefixes.ToList().ForEach(x => Listener.Prefixes.Add(x));
            RootPath = rootPath;
        }

        public void Start()
        {
            Listener.Start();

            new Action(ListenerLoop).BeginInvoke(null, null);

            Console.WriteLine("OK, server is ready!");
        }

        private void ListenerLoop()
        {
            try
            {
                while (Listener.IsListening)
                {
                    IAsyncResult result = Listener.BeginGetContext(new AsyncCallback(ListenerCallback), Listener);
                    result.AsyncWaitHandle.WaitOne();
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }

        private void ListenerCallback(IAsyncResult result)
        {
            if (Listener.IsListening)
            {
                HttpListener listener = (HttpListener)result.AsyncState;
                HttpListenerContext context = listener.EndGetContext(result);

                using (HttpListenerResponse response = context.Response)
                {
                    ProccessContext(context);
                }
            }
        }

        protected virtual void ProccessContext(HttpListenerContext context)
        {
            RequestReceived.Fire(this, new RequestReceivedArgs(context));
        }

        protected virtual void OnException(Exception ex)
        {
            Console.WriteLine(string.Format("Exception has been caught... {0}", ex.Message));
        }

        public void Dispose()
        {
            Listener.Stop();
            Listener.Close();

            Console.WriteLine("Server has stopped!");
        }

        public event EventHandler<RequestReceivedArgs> RequestReceived;
    }
}
