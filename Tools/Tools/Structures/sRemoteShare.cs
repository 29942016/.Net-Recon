using System;
using System.Linq;

namespace Tools.Structures
{
    public class RemoteShare
    {
        public bool Available {get; private set;}
        public string Host { get; private set; }
        public string Network { get; private set; }

        public RemoteShare(string unparsedRemoteShare)
        {
            string[] result = unparsedRemoteShare.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            if (result.Length >= 2)
            {
                Available = result[0].Equals("OK");
                Host = result[1];
                Network = string.Join(" ",result.Skip(2));
            }
        }
    }
}
