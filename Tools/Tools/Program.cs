using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using Tools.Host;
using Tools.Modules;
using Tools.Structures;

namespace Tools
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var result = Reconnaissance.ARP();

            foreach (AdapterInterface @interface in result)
            {
                Console.WriteLine(@interface.Adapter.Description);
                Console.WriteLine(string.Join("\n", @interface.Addresses.Select(x => x.IP)));
            }

            Console.ReadLine();
        }

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
    }
}
