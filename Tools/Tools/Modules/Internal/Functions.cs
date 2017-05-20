using System.Collections.Generic;
using System.Text.RegularExpressions;
using Tools.Structures;

namespace Tools.Modules.Internal
{
    internal static class Functions
    {
        /// <summary>
        /// Transforms the STDOut of a cmdlet "Arp -a" query
        /// into a datastructure of HashSet<AdapterInterface>
        /// </summary>
        internal static HashSet<AdapterInterface> ParseArpString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;

            string adapterFilter = @"Interface.*?(?=Interface|$)";
            HashSet<AdapterInterface> interfaces = new HashSet<AdapterInterface>();

            foreach (Match adapter in Regex.Matches(input, adapterFilter))
                interfaces.Add(new AdapterInterface(adapter.Value));

            return interfaces;
        }
    }
}
