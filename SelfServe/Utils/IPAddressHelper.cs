using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SelfServe
{
    public static class IPAddressHelper
    {
        public static IEnumerable<IPAddress> GetAddresses(AddressFamily addressFamily = AddressFamily.InterNetwork)
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            return host.AddressList.Where(x => x.AddressFamily == addressFamily);
        }
    }
}
