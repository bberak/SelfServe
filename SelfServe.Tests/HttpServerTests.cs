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
            HttpServer server = CreateServer();
            WebRequest request = CreateRequest(file: "Missing-File.txt");
      
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
