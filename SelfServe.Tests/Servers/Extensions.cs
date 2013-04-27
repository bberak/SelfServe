using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SelfServe.Tests
{
    public static class Extensions
    {
        public static HttpServer ThatThrowsException(this HttpServer server, string message = "")
        {
            server.RequestReceived += (s, e) =>
            {
                throw new Exception(message);
            };

            return server;
        }

        public static HttpServer AndCatchesException(this HttpServer server, string contents = "The server handled an exception.")
        {
            server.ExceptionCaught += (s, e) =>
            {
                e.Response.WriteText(contents);
            };

            return server;
        }

        public static HttpServer AndStart(this HttpServer server)
        {
            server.Start();

            return server;
        }
    }
}
