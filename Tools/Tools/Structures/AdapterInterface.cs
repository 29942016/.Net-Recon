using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using static Tools.Static.Enumerations;

namespace Tools.Structures
{
    /// <summary>
    /// Stores an adapter and every address that can be resolved using that adapter.
    /// </summary>
    public class AdapterInterface
    {
        public NetworkInterface Adapter { get; private set; }
        public BindingList<ARP> Addresses = new BindingList<ARP>();

        public AdapterInterface(IPAddress ip, byte pid)
        {
            SetAdapter(ip);
        }

        public AdapterInterface(string unparsedAdapterString)
        {
            string adapterInfoFilter   = @"(\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b)|(0[xX][1-9a-zA-Z]+)";
            string adapterHeaderFilter = @"Interface.*?(Type)";
            string addressFilter       = @"(\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b).*?(static|dynamic)";

            var IP         = IPAddress.Parse(Regex.Matches(unparsedAdapterString, adapterInfoFilter)[0].Value);

            if (IP != null)
                SetAdapter(IP);

            unparsedAdapterString = Regex.Replace(unparsedAdapterString, adapterHeaderFilter, string.Empty);

            foreach (Match addressString in Regex.Matches(unparsedAdapterString, addressFilter))
            {
                string[] entry = addressString.Value.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                var ip  = IPAddress.Parse(entry[0]);
                var mac = PhysicalAddress.Parse(entry[1].ToUpper());
                var type = (IPType)Enum.Parse(typeof(IPType), entry[2]);

                ARP newArp = new ARP(ip, mac, type);

                Addresses.Add(newArp);
            }
        }

        private void SetAdapter(IPAddress ip)
        {
            byte[] ipBytes = ip.GetAddressBytes();

            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (IPAddress address in adapter.GetIPProperties().UnicastAddresses.Select(x => x.Address))
                {
                    if (address.GetAddressBytes().SequenceEqual(ipBytes))
                        this.Adapter = adapter;
                }
            }
               
        }
    }
}
