using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Net;

namespace SelfServe
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;

            Console.WriteLine(
                string.Format("SelfServe v{0}\n", GetVersion())
                );

            if (HttpListener.IsSupported)
            {
                using (HttpServer Server = new HttpFileServer(GetConfig(args)))
                {
                    Server.Start();
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                Console.ReadLine();
            }
        }

        static string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        static HttpServerConfig GetConfig(string[] args)
        { 
            var prefixes = args.ToList();

            var name = Assembly.GetExecutingAssembly().Location;

            if (Bindings.CanBeReadFrom(name))
                prefixes.AddRange(Bindings.Read(name));

            return prefixes.Any() ? 
                new HttpServerConfig(prefixes.Distinct().ToArray(), Environment.CurrentDirectory) : 
                new DefaultConfig();
        }
    }
}
