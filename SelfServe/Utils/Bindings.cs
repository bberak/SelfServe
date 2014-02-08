using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SelfServe
{
    public static class Bindings
    {
        private static readonly Regex BindingRegex = new Regex(@"\.(?<Ports>[\.0-9]+)\.(?<Protocol>(http|https)).exe$");

        public static bool CanBeReadFrom(string str)
        {
            return BindingRegex.IsMatch(str);
        }

        public static IEnumerable<string> Read(string str)
        {
            var match = BindingRegex.Match(str);

            if (match.Success)
            {
                var ports = match.Groups["Ports"].Value.Split('.');
                var protocol = match.Groups["Protocol"].Value;

                return ports.Select(p => string.Format("{0}://+:{1}/", protocol, p));
            }

            throw new InvalidOperationException("Bindings could not be read from: " + str);
        }
    }
}
