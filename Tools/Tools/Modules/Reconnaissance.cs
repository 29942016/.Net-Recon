using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using Tools.Host;
using Tools.Structures;
using Tools.Static;
using Tools.Modules.Internal;
using System.Linq;
using System.ComponentModel;
using System;

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

        public static HashSet<AdapterInterface> ARP()
        {
            Process cmdlet = new Process() { StartInfo = Globals.DefaultProcessStartInfo("cmd.exe", "/c arp -a") };

            cmdlet.Start();

            string stdOut = string.Empty;

            while (!cmdlet.StandardOutput.EndOfStream)
                stdOut += cmdlet.StandardOutput.ReadLine();

            HashSet<AdapterInterface> arpTable = Functions.ParseArpString(stdOut);
            return arpTable;
        }

        #endregion

        #region File System

        public static List<RemoteShare> GetShares(IPAddress[] addresses)
        {
            List<RemoteShare> shares = new List<RemoteShare>();
            ProcessStartInfo procInfo = Globals.DefaultProcessStartInfo("net", "use");

            string stdOutput;
            using (Process cmdlet = Process.Start(procInfo))
                stdOutput = cmdlet.StandardOutput.ReadToEnd();

            foreach (IPAddress ip in addresses)
            {
                string availableShare = stdOutput.Split('\n').FirstOrDefault(x => x.Contains("OK") && x.Contains(ip.ToString()));

                if(!string.IsNullOrEmpty(availableShare))
                    shares.Add(new RemoteShare(availableShare));
            }

            return shares;
        }

        #endregion


        #region Hide default class comparitors
        /// Notes
        /// These should be hidden from when including 
        /// this application as a DLL reference.
        ///

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static new bool Equals(object obj)
        {
            throw new Exception("Assertion does not implement Equals, use Ensure or Require");
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static new bool ReferenceEquals(object objA, object objB)
        {
            throw new Exception("Assertion does not implement ReferenceEquals, use Ensure or Require");
        }

        #endregion
    }
}
