using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SelfServe
{
    class Program
    {
        static HttpServer Server;

        static void Main(string[] args)
        {
            Console.WriteLine(
                string.Format("SelfServe v{0}\n", Assembly.GetExecutingAssembly().GetName().Version)
                );

            if (args.Length == 0)
            {
                args = new string[] { HttpServer.DEFAULT_PREFIX };
            }

            Server = new HttpServer(args);

            Server.Start();

            Console.ReadLine();
        }
    }
}
