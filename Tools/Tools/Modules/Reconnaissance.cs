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
using System.IO;

namespace Tools.Modules
{
    public static class Reconnaissance
    {
        private static bool _HasAdmin => (LocalMachine.Privilege == Enumerations.ExecutionLevel.Administrator);

        #region Networking

        /// <summary>
        /// Sends a standard ping request to a host, packet data and timeout are optional.
        /// </summary>
        public static PingReply SendPing(IPAddress ip, string data = "", int timeout = 128)
        {
            byte[] packetData = Encoding.ASCII.GetBytes(data);
            Ping ping = new Ping();
            PingReply reply = ping.Send(ip, timeout, packetData);

            return reply;
        }

        /// <summary>
        /// Sends a 'arp -a' from the local machine, returning
        /// a structured list of adapters and resolved IPs.
        /// </summary>
        public static HashSet<AdapterInterface> SendARP()
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

        /// <summary>
        /// Returns a list of mounted drives, filtered by the passed parameter drive type.
        /// if none is specified, return all drives.
        /// </summary>
        public static List<DriveInfo> GetMountedDrives(DriveType type = DriveType.Unknown)
        {
            if (type == DriveType.Unknown)
                return DriveInfo.GetDrives().ToList();
            else
                return DriveInfo.GetDrives().Where(x => x.DriveType == type).ToList();
        }

        /// <summary>
        /// Given a list of ips, check if they have remote sharing enabled,
        /// if so, return that as a new list of RemoteShare objects.
        /// </summary>
        public static List<RemoteShare> GetRemoteShares(IPAddress[] addresses)
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
