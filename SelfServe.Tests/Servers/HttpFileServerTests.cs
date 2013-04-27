using System;
using System.Net;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SelfServe;

namespace SelfServe.Tests
{
    [TestClass]
    public class HttpFileServerTests : TestBase
    {
        [TestMethod]
        public void Request_MissingFile_404()
        {          
            using (HttpServer server = CreateFileServer())
            {
                try
                {
                    //-- Arrange
                    server.Start();
                    WebRequest request = CreateRequest(path: "Missing-File.txt");             

                    //-- Act          
                    request.GetResponse();

                    //-- Assert
                    Assert.Fail();
                }
                catch (WebException wex)
                {
                    //-- Assert
                    HttpWebResponse response = wex.Response as HttpWebResponse;
                    Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
                }
            }
        }

        [TestMethod]
        public void Request_MissingFolder_404()
        {          
            using (HttpServer server = CreateFileServer())
            {
                try
                {
                    //-- Arrange
                    server.Start();
                    WebRequest request = CreateRequest(path: "X/Y/Z");          

                    //-- Act           
                    request.GetResponse();

                    //-- Assert
                    Assert.Fail();
                }
                catch (WebException wex)
                {
                    //-- Assert
                    HttpWebResponse response = wex.Response as HttpWebResponse;
                    Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
                }
            }
        }

        [TestMethod]
        public void Request_RootFile_200()
        {        
            using (HttpServer server = CreateFileServer())
            {
                try
                {
                    //-- Arrange
                    server.Start();
                    string dllName = Assembly.GetExecutingAssembly().GetName().Name + ".dll";
                    WebRequest request = CreateRequest(path: dllName);        

                    //-- Act            
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                    //-- Assert
                    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                    Assert.IsTrue(response.ContentLength > 0);
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
            using (HttpServer server = CreateFileServer())
            {
                try
                {
                    //-- Arrange
                    server.Start();
                    WebRequest request = CreateRequest(path: "TestFolders/A/C/D/TestFile3.html");       

                    //-- Act
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                    //-- Assert
                    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                    Assert.IsTrue(response.ContentLength > 0);
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
            using (HttpServer server = CreateFileServer())
            {
                try
                {
                    //-- Arrange
                    server.Start();
                    WebRequest request = CreateRequest(path: "TestFolders");                

                    //-- Act            
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                    //-- Assert
                    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                    Assert.IsTrue(response.ContentLength > 0);
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
            
            using (HttpServer server = CreateFileServer())
            {
                try
                {
                    //-- Arrange
                    server.Start();
                    WebRequest request = CreateRequest(path: "TestFolders/A/C/D");             

                    //-- Act                 
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                    //-- Assert
                    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                    Assert.IsTrue(response.ContentLength > 0);
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
