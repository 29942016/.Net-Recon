using System.Net;
using System.Net.NetworkInformation;
using static Tools.Static.Enumerations;

namespace Tools.Structures
{
    public class ARP
    {
        public IPAddress IP { get; private set; }
        public PhysicalAddress MAC { get; private set; }
        public IPType AddressType { get; private set; }

        public ARP(IPAddress ip, PhysicalAddress mac, IPType type)
        {
            IP = ip;
            MAC = mac;
            AddressType = type;
        }
    }

}
