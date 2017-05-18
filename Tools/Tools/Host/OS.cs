using System;
using System.Collections.Generic;
using System.Linq;

namespace Tools.Host
{
    public class OS
    {
        public readonly string Name,
                               Edition,
                               ServicePack,
                               Version,
                               Bits;

        public OS()
        {
            OperatingSystem os = Environment.OSVersion;
            Version = string.Format("{0}.{1}", os.Version.Major, os.Version.Minor);
            ServicePack = os.ServicePack;
            Name = OperatingSystemLookup.FirstOrDefault(x => x.Key == Version).Value;
        }

        public override string ToString()
        {
            return Name;
        }

        private Dictionary<string, string> OperatingSystemLookup = new Dictionary<string, string>()
        {
            {"10","Windows 10"},
            {"6.3","Windows 8.1" },
            {"6.2","Windows 8"},
            {"6.1","Windows 7"},
            {"6.0","Windows Vista"},
            {"5.2","Windows XP 64-Bit Edition"},
            {"5.1","Windows XP"},
            {"5.0","Windows 2000"},
        };
    }
}
