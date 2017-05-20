using System;
using System.Reflection;
using Tools.Host;
using Tools.Modules;

namespace Tools
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var result = Reconnaissance.SendARP();
            var drives = Reconnaissance.GetMounts();
            Console.WriteLine(drives);

//            List<IPAddress> ips = new List<IPAddress>();

//            foreach (var adapter in result)
//                ips.AddRange(adapter.Addresses.Select(x => x.IP).Where(x => x.IsOnline()));

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
