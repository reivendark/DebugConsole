using System.Collections.Generic;
using System.Linq;

namespace DebugConsole
{
    public static class DebugConsoleParser
    {
        private static readonly char[] s_TrimChars = { '\n', '>' };

        public static List<string> Parse(string command)
        {
            command = command.Trim(s_TrimChars);
            command = command.Trim();

            List<string> parsedCommand = command.Split((char)32).ToList();
            return parsedCommand;
        }
    }
}