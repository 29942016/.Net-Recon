using System.Net;
using Tools.Modules;

namespace Tools.Static
{
    public static class Extensions
    {
        public static bool IsOnline(this IPAddress ip)
        {
            var result = (Reconnaissance.Ping(ip).Status == System.Net.NetworkInformation.IPStatus.Success);
            System.Console.WriteLine("[{0}]\t- {1}", (result) ? "ON" : "OFF", ip);
            return result;
        }
    }
}
