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
            Console.WriteLine("SelfServe v{0}\n",GetVersion()); 

            if (HttpListener.IsSupported)
            {
                using (HttpServer Server = new HttpFileServer(GetConfig(args)))
                {
                    Server.Start();

                    Console.WriteLine("Your current IP addresses are: {0}", String.Join(", ", IPAddressHelper.GetAddresses()));
                    Console.WriteLine("Listening on the following bindings: {0}", String.Join(", ", Server.Config.Bindings));
                    Console.WriteLine("Hit Enter to close\n");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                Console.WriteLine("Hit Enter to close");
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
                new HttpServerConfig(prefixes.Distinct().ToArray(), Environment.CurrentDirectory, addFirewallAuthorization: false) : 
                new DefaultConfig();
        }
    }
}
