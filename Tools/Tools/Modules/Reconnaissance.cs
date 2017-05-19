using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using Tools.Host;
using Tools.Static;

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

        public static HashSet<ARP> ARP(string args)
        {
            HashSet<ARP> arpTable = new HashSet<ARP>();
            Process arpCmdlet = new Process()
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

            arpCmdlet.Start();

            //while (!arpCmdlet.StandardOutput.EndOfStream)
            //{
            //    string line = arpCmdlet.StandardOutput.ReadLine();
            //}

            return arpTable;
        }


    }
}
