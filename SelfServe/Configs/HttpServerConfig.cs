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

        public bool AddFirewallAuthorization { get; private set; }

        public HttpServerConfig(string[] prefixes, string rootPath, bool addFirewallAuthorization)
        {
            Bindings = prefixes;
            RootPath = rootPath;
            AddFirewallAuthorization = addFirewallAuthorization;
        }
    }
}
