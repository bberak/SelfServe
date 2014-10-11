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
        public event EventHandler<RequestReceivedArgs> RequestReceived;
        public event EventHandler<ExceptionCaughtEventArgs> ExceptionCaught;
        protected readonly string RootPath;
        private readonly HttpListener Listener;
        public HttpServerConfig Config { get; private set; }

        public HttpServer(HttpServerConfig config)
        {
            Config = config;
            Listener = new HttpListener();
            Config.Bindings.ToList().ForEach(x => Listener.Prefixes.Add(x));
            RootPath = config.RootPath;
            AddFirewallAuthorization();
        }

        public void Start()
        {
            Listener.Start();

            new Action(ListenerLoop).BeginInvoke(null, null);

            Log("OK, server is ready!");
        }

        private void ListenerLoop()
        {
            while (Listener.IsListening)
            {
                IAsyncResult result = Listener.BeginGetContext(new AsyncCallback(ListenerCallback), Listener);
                result.AsyncWaitHandle.WaitOne();
            }
        }

        private void ListenerCallback(IAsyncResult result)
        {
            if (Listener.IsListening)
            {
                HttpListener listener = (HttpListener)result.AsyncState;
                HttpListenerContext context = listener.EndGetContext(result);

                using (context.Response)
                {
                    try
                    {
                        ProccessContext(context);
                    }
                    catch (Exception ex)
                    {
                        ProcessException(context, ex);
                    }
                }
            }
        }

        protected void AddFirewallAuthorization()
        {
            if (Config.AddFirewallAuthorization)
            {
                var currentLocation = System.Reflection.Assembly.GetEntryAssembly().Location;

                if (!FirewallHelper.Instance.HasAuthorization(currentLocation))
                    FirewallHelper.Instance.GrantAuthorization(currentLocation, AppDomain.CurrentDomain.FriendlyName);
            }
        }

        protected void RemoveFirewallAuthorization()
        {
            var currentLocation = System.Reflection.Assembly.GetEntryAssembly().Location;

            if (FirewallHelper.Instance.HasAuthorization(currentLocation))
                FirewallHelper.Instance.RemoveAuthorization(currentLocation);
        }

        protected virtual void ProccessContext(HttpListenerContext context)
        {
            RequestReceived.Fire(this, new RequestReceivedArgs(context));
        }

        protected virtual void ProcessException(HttpListenerContext context, Exception ex)
        {
            ExceptionCaught.Fire(this, new ExceptionCaughtEventArgs(context, ex));

            OnException(context.Request, context.Response, ex);
        }

        protected virtual void OnException(HttpListenerRequest request, HttpListenerResponse response, Exception ex)
        {
            Log("Exception has been caught... {0}", ex.Message);
        }

        protected virtual void Log(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        public virtual void Dispose()
        {
            try
            {
                Listener.Stop();
                Listener.Close();

                Log("Server has stopped!");
            }
            catch (Exception ex)
            {
                Log("The following issue was encountered when stopping the server: {0}", ex);
            }
            finally
            {
                RemoveFirewallAuthorization();
            }
        }     
    }
}
