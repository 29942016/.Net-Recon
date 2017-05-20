using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using Tools.Host;
using Tools.Structures;
using Tools.Static;
using Tools.Modules.Internal;

namespace Tools.Modules
{
    public static class Reconnaissance
    {
        private static bool _HasAdmin => (LocalMachine.Privilege == Enumerations.ExecutionLevel.Administrator);

        #region Networking

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

            HashSet<AdapterInterface> arpTable = Functions.ParseArpString(stdOut);
            return arpTable;
        }

        #endregion

        #region File System

        #endregion

        #region 

    }
}
