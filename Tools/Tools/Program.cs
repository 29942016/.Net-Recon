using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using Tools.Host;
using Tools.Modules;
using Tools.Static;

namespace Tools
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var result = Reconnaissance.ARP();

            List<IPAddress> ips = new List<IPAddress>();

            foreach (var adapter in result)
                ips.AddRange(adapter.Addresses.Select(x => x.IP).Where(x => x.IsOnline()));

            var shares = Reconnaissance.GetShares(ips.ToArray());

            Console.ReadLine();
        }

        #region DEBUGGING

        private static void OutputLocalMachineInfo()
        {
            Type type = typeof(LocalMachine);
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                var propertyValue = property.GetValue(null);
                Console.WriteLine("{0}: {1}",property.Name, propertyValue);
            }

            Console.WriteLine("RunLevel: {0}", LocalMachine.Privilege);
        }

        #endregion
    }
}
