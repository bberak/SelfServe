using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SelfServe
{
    public class RequestReceivedArgs : EventArgs
    {
        private HttpListenerContext Context;

        public HttpListenerRequest Request { get { return Context.Request; } }

        public HttpListenerResponse Response { get { return Context.Response; } }

        public RequestReceivedArgs(HttpListenerContext context)
        {
            Context = context;
        }
    }
}
