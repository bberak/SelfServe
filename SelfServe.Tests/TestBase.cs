using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SelfServe;

namespace SelfServe.Tests
{
    public abstract class TestBase
    {
        protected HttpServer CreateServer()
        {
            return new HttpServer();
        }

        protected HttpServer CreateServer(string[] prefixes)
        {
            return new HttpServer(prefixes);
        }

        protected WebRequest CreateRequest(string host = "http://localhost/", string path = "", int timeout = 10000)
        {
            WebRequest request = HttpWebRequest.Create(host + path);
            request.Timeout = timeout;

            return request;
        }
    }
}
