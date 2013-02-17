using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SelfServe
{
    class Program
    {
        private static HttpServer Server;

        static void Main(string[] args)
        {
            Console.WriteLine(
                string.Format("SelfServe v{0}\n", Assembly.GetExecutingAssembly().GetName().Version)
                );

            Server = args.Length > 0 ? new HttpServer(args) : new HttpServer();

            Server.Start();

            Console.ReadLine();
        }
    }
}
