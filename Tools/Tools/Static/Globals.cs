using System.Diagnostics;

namespace Tools.Static
{
    public static class Globals
    {
        public static ProcessStartInfo DefaultProcessStartInfo(string executable, string arguements)
        {
            ProcessStartInfo StartInfo = new ProcessStartInfo()
            {
                FileName = executable,
                Arguments = arguements,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false
            };

            return StartInfo;
        }

    }
}
