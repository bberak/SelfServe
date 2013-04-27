using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SelfServe
{
    public class ExceptionCaughtEventArgs : RequestReceivedArgs
    {
        public Exception Exception { get; protected set; }

        public ExceptionCaughtEventArgs(HttpListenerContext context, Exception ex)
            :base(context)
        {
            Exception = ex;
        }
    }
}
