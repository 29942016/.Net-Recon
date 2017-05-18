using System;
using System.Reflection;
using Tools.Host;

namespace Tools
{
    class Program
    {
        static void Main(string[] args)
        {
            OutputLocalMachineInfo();
            Console.WriteLine("----");
            System.Threading.Thread.Sleep(50000);
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

        }
    }
}
