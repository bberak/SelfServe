using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SelfServe
{
    public class DefaultConfig : HttpServerConfig
    {
        public DefaultConfig()
            : this(Environment.CurrentDirectory)
        {
        }

        public DefaultConfig(string rootPath)
            : base(new[] { "http://+/", "https://+/" }, rootPath)
        {
        }
    }
}
