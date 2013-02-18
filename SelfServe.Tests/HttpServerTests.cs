using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SelfServe;

namespace SelfServe.Tests
{
    [TestClass]
    public class HttpServerTests : TestBase
    {
        [TestMethod]
        public void Request_MissingFile_404()
        {
            //-- Arrange
            WebRequest request = CreateRequest(path: "Missing-File.txt");
            using (HttpServer server = CreateServer())
            {
                try
                {
                    //-- Act
                    server.Start();
                    request.GetResponse();
                }
                catch (WebException wex)
                {
                    //-- Assert
                    HttpWebResponse response = wex.Response as HttpWebResponse;
                    Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
                }
                catch
                {
                    //-- Assert
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void Request_MissingDirectory_404()
        {
            //-- Arrange
            WebRequest request = CreateRequest(path: "dirA/dirB/dirC");
            using (HttpServer server = CreateServer())
            {
                try
                {
                    //-- Act
                    server.Start();
                    request.GetResponse();
                }
                catch (WebException wex)
                {
                    //-- Assert
                    HttpWebResponse response = wex.Response as HttpWebResponse;
                    Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
                }
                catch
                {
                    //-- Assert
                    Assert.Fail();
                }
            }
        }
    }
}
