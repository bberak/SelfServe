using System;
using System.Net;
using System.Reflection;
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
        public void Request_MissingFolder_404()
        {
            //-- Arrange
            WebRequest request = CreateRequest(path: "X/Y/Z");
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
        public void Request_RootFile_200()
        {
            //-- Arrange
            string dllName = Assembly.GetExecutingAssembly().GetName().Name + ".dll";
            WebRequest request = CreateRequest(path: dllName);
            using (HttpServer server = CreateServer())
            {
                try
                {
                    //-- Act
                    server.Start();
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                    //-- Assert
                    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                }
                catch
                {
                    //-- Assert
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void Request_NestedFile_200()
        {
            //-- Arrange
            WebRequest request = CreateRequest(path: "TestFolders/A/C/D/TestFile3.html");
            using (HttpServer server = CreateServer())
            {
                try
                {
                    //-- Act
                    server.Start();
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                    //-- Assert
                    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                }
                catch
                {
                    //-- Assert
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void Request_RootFolder_200()
        {
            //-- Arrange
            WebRequest request = CreateRequest(path: "TestFolders");
            using (HttpServer server = CreateServer())
            {
                try
                {
                    //-- Act
                    server.Start();
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                    //-- Assert
                    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                }
                catch
                {
                    //-- Assert
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void Request_NestedFolder_200()
        {
            //-- Arrange
            WebRequest request = CreateRequest(path: "TestFolders/A/C/D");
            using (HttpServer server = CreateServer())
            {
                try
                {
                    //-- Act
                    server.Start();
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                    //-- Assert
                    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
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
