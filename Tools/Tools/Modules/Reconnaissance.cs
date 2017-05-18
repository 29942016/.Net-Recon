using Tools.Host;
using Tools.Static;

namespace Tools.Modules
{
    public static class Reconnaissance
    {
        private static bool _HasAdmin => (LocalMachine.Privilege == Enumerations.ExecutionLevel.Administrator);



    }
}
