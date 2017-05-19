using System.ComponentModel;

namespace Tools.Static
{
    public static class Enumerations
    {
        public enum ExecutionLevel
        {
            [Description("Administrator")]
            Administrator,
            [Description("User")]
            User
        }

        public enum IPType
        {
            [Description("static")]
            @static,
            [Description("dynamic")]
            dynamic
        }
    }


}
