using System.Linq;

namespace CustomTools.SystemTools
{
    public static class CommandLine
    {
        public static bool HasCommandLineArgument(string argument)
        {
            string[] args = System.Environment.GetCommandLineArgs();
            return args.Any(arg => arg == argument);
        }
    }
}