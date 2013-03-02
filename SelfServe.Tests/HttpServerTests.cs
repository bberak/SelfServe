using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace SelfServe.Tests
{
    [TestClass]
    public class HttpServerTests : TestBase
    {
        [TestMethod]
        public void Request_Default_200()
        {
            //-- Arrange  
            WebRequest request = CreateRequest(path: "default");
            using (HttpServer server = CreateServer())
            {
                server.RequestReceived += (s, e) =>
                {
                    e.Response.WriteHtml("You have requested the default page");
                };
                
                //-- Act
                server.Start();
                var response = request.GetResponse();

                //-- Assert
                Assert.AreEqual(response.GetContents(), "You have requested the default page");
            }
        }
    }
}
