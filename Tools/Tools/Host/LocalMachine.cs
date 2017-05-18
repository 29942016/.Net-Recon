using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Tools.Host
{
    /// <summary>
    /// A static object to store all the information of the 
    /// current executing machine.
    /// </summary>
    public static class LocalMachine
    {
        public static string Hostname { get; private set; }
        public static OS OperatingSystem { get; private set; }
        public static string Domain { get; private set; }

        public static IPAddress ExternalIP { get; private set; }
        public static IPAddress InternalIP { get; private set; }

        static LocalMachine()
        {
            Hostname = Dns.GetHostEntry("").HostName;
            OperatingSystem = new OS();
            Domain = Environment.UserDomainName;
            ExternalIP = IPAddress.Parse(GetExternalIP());
            InternalIP = Dns.GetHostEntry("").AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
        }

        private static string GetExternalIP()
        {
            return new WebClient().DownloadString("http://icanhazip.com").TrimEnd('\n');
        }
    }
}
