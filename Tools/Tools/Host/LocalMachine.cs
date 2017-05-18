using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Principal;
using static Tools.Static.Enumerations;

namespace Tools.Host
{
    /// <summary>
    /// A static object to store all the information of the 
    /// current executing machine.
    /// </summary>
    public static class LocalMachine
    {
        public static string User { get; private set; }
        public static string Hostname { get; private set; }
        public static OS     OperatingSystem { get; private set; }
        public static string Domain { get; private set; }

        public static IPAddress ExternalIP { get; private set; }
        public static IPAddress InternalIP { get; private set; }

        public static ExecutionLevel Privilege;

        static LocalMachine()
        {
            User            = Environment.UserName;
            Hostname        = Dns.GetHostEntry("").HostName;
            OperatingSystem = new OS();
            Domain          = (Environment.UserDomainName == Hostname) ? "N/A" : Environment.UserDomainName;
            ExternalIP      = IPAddress.Parse(GetExternalIP());
            InternalIP      = Dns.GetHostEntry("").AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
            Privilege       = (IsElevated()) ? ExecutionLevel.Administrator : ExecutionLevel.User;
        }

        private static string GetExternalIP()
        {
            return new WebClient().DownloadString("http://bot.whatismyipaddress.com").TrimEnd('\n');
        }

        private static bool IsElevated()
        {
            using (WindowsIdentity id = WindowsIdentity.GetCurrent())
            {
                var principal = new WindowsPrincipal(id);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }
    }
}
