using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SelfServe
{
    public class HttpServerConfig
    {
        public string[] Bindings { get; private set; }

        public string RootPath { get; private set; }

        public HttpServerConfig(string[] prefixes, string rootPath)
        {
            Bindings = prefixes;

            RootPath = rootPath;
        }
    }
}
