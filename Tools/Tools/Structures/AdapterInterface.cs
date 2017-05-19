using System;
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using static Tools.Static.Enumerations;

namespace Tools.Structures
{
    public class AdapterInterface
    {
        public IPAddress IP { get; private set; }
        public byte PhysicalID { get; private set; }
        public BindingList<ARP> Addresses = new BindingList<ARP>();

        public AdapterInterface(IPAddress ip, byte pid)
        {
            IP = ip;
            PhysicalID = pid;
        }

        public AdapterInterface(string unparsedAdapterString)
        {
            string adapterInfoFilter   = @"(\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b)|(0[xX][1-9a-zA-Z]+)";
            string adapterHeaderFilter = @"Interface.*?(Type)";
            string addressFilter       = @"(\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b).*?(static|dynamic)";

            IP         = IPAddress.Parse(Regex.Matches(unparsedAdapterString, adapterInfoFilter)[0].Value);
            PhysicalID = Convert.ToByte(Regex.Matches(unparsedAdapterString, adapterInfoFilter)[1].Value, 16);

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
    }
}
