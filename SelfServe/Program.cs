using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SelfServe
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;

            Console.WriteLine(
                string.Format("SelfServe v{0}\n", Assembly.GetExecutingAssembly().GetName().Version)
                );

            using (HttpServer Server = new HttpFileServer(args))
            {
                Server.Start();

                Console.ReadLine();
            }
        }
    }
}
