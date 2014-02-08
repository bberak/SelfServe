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
            return new HttpServer(new DefaultConfig { });
        }

        protected HttpFileServer CreateFileServer()
        {
            return new HttpFileServer(new DefaultConfig { });
        }

        protected WebRequest CreateRequest(string host = "http://localhost/", string path = "", int timeout = 10000)
        {
            WebRequest request = HttpWebRequest.Create(host + path);
            request.Timeout = timeout;

            return request as WebRequest;
        }
    }
}
