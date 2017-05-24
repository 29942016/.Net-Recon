using System.Diagnostics;
using System.Net;
using static Tools.Static.Enumerations;

namespace Tools.Structures
{
    public class Service
    {
        public Protocol Protocol { get; private set; }
        public IPAddress LocalAddress { get; private set; }
        public IPAddress ForeignAddress { get; private set; }
        public Process Process { get; private set; }
          
        private int _Pid;

        public sService(string unparsedNetstatCommand)
        {


        }


    }
}
