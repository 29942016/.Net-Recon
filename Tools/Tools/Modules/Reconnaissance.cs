using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using Tools.Host;
using Tools.Structures;
using Tools.Static;
using System.Text.RegularExpressions;

namespace Tools.Modules
{
    public static class Reconnaissance
    {
        private static bool _HasAdmin => (LocalMachine.Privilege == Enumerations.ExecutionLevel.Administrator);

        public static PingReply Ping(IPAddress ip, string data = "", int timeout = 128)
        {
            byte[] packetData = Encoding.ASCII.GetBytes(data);
            Ping ping = new Ping();
            PingReply reply = ping.Send(ip, timeout, packetData);

            return reply;
        }

        public static HashSet<AdapterInterface> ARP(string args)
        {
            Process cmdlet = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = "/c arp -a",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = false
                }
            };

            cmdlet.Start();

            string stdOut = string.Empty;

            while (!cmdlet.StandardOutput.EndOfStream)
                stdOut += cmdlet.StandardOutput.ReadLine();

            HashSet<AdapterInterface> arpTable = ParseArpString(stdOut);
            return arpTable;
        }

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
