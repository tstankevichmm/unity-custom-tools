using System.Linq;

namespace CustomTools.SystemTools
{
    public static class CommandLine
    {
        public static bool HasCommandLineArgument(string argument, out string data)
        {
            string[] args = System.Environment.GetCommandLineArgs();
            data = null;

            for (var i = 0; i < args.Length; i++)
            {
                if (args[i] != argument) 
                    continue;
                
                data = args[i + 1];
                return true;
            }

            return false;
        }
    }
}