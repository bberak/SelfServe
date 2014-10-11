using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SelfServe
{
    public class DefaultConfig : HttpServerConfig
    {
        public DefaultConfig()
            : this(Environment.CurrentDirectory, addFirewallAuthorization: false)
        {
        }

        public DefaultConfig(string rootPath, bool addFirewallAuthorization)
            : base(new[] { "http://+/", "https://+/" }, rootPath, addFirewallAuthorization)
        {
        }
    }
}
