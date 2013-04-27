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
            using (HttpServer server = CreateServer())
            {
                //-- Arrange       
                server.RequestReceived += (s, e) =>
                {
                    e.Response.WriteText("You have requested the default page");
                };
                server.Start();
                WebRequest request = CreateRequest(path: "default");      
                
                //-- Act 
                var response = request.GetResponse();

                //-- Assert
                Assert.AreEqual(response.GetContents(), "You have requested the default page");
            }
        }

        [TestMethod]
        public void Check_Server_HandlesException()
        {
            using (HttpServer server = CreateServer().ThatThrowsException().AndCatchesException("Exception Handled").AndStart())
            {
                var request = CreateRequest(path: "default");
                var response = request.GetResponse();

                Assert.AreEqual("Exception Handled", response.GetContents());
            }
        }
    }
}
