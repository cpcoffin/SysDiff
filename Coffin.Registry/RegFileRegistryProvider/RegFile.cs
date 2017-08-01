using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace Coffin.Registry
{
    /// <summary>
    /// Reads .reg files.
    /// </summary>
    public static class RegFile
    {
        static private Regex _RegexWhitespaceOnly = new Regex(@"^\s*$", RegexOptions.Compiled);
        static private Regex _RegexNewKey = new Regex(@"^\s*\[(" + Strings.HKEY_LOCAL_MACHINE + "|" + Strings.HKEY_CURRENT_USER + "|" +
                                                                   Strings.HKEY_CLASSES_ROOT + "|" + Strings.HKEY_USERS + "|" +
                                                                   Strings.HKEY_CURRENT_CONFIG + @")\\(.{1,})\]$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static Dictionary<Microsoft.Win32.RegistryHive, RegistryKeyInfo> ReadRegFile(string filepath)
        {
            Dictionary<Microsoft.Win32.RegistryHive, RegistryKeyInfo> Hives = new Dictionary<Microsoft.Win32.RegistryHive, RegistryKeyInfo>();
            return Hives;
        }

        public static bool ParseKey(string line, out string hive, out string key)
        {
            Match newKeyMatch = _RegexNewKey.Match(line);
            if (!newKeyMatch.Success)
            {
                hive = "";
                key = "";
                return false;
            }
            hive = newKeyMatch.Groups[1].Value;
            key = newKeyMatch.Groups[2].Value;
            return true;
        }

        private static RegFileValueBuilder ParseFirstLine(string line)
        {
            // Get name/value separator (unquoted '=')
            int sep;
            do
            {
                sep = line.IndexOf('=');
                if (sep == -1)
                {
                    // Separator not found
                    return null;
                }
            }
            while (ContainsUnmatchedQuote(line.Substring(0, sep)));

            // Get name
            string name = line.Substring(0, sep).Trim();
            if (name[0] != '"' || name[name.Length - 1] != '"')
            {
                // Name should be in quotes
                return null;
            }
            name = name.Substring(1, name.Length - 2);

            // Get type
            string data = line.Substring(sep).Trim();
            if (data[0] == '"')
            {
                if (data[data.Length - 1] != '"' || ContainsUnmatchedQuote(data))
                {
                    // String value should be quoted
                    return null;
                }
                data = data.Substring(1, name.Length - 2);
                return new RegFileValueBuilder(name, RegistryValueKind.String, data);
            }
            else
            {
                sep = data.IndexOf(':');
                if (sep == -1)
                {
                    // Can not determine data type
                    return null;
                }
                // TODO left off here
            }
        }

        /// <summary>
        /// Helper class for parsing registry files. Represents a value that may only be
        /// partially loaded (in the case of values that span multiple lines).
        /// </summary>
        private class RegFileValueBuilder
        {
            private RegistryValueType _Type;
            public RegistryValueType Type { get; }
            

            /*
            public static RegistryFileValueParser GetRegistryFileValueParser(string firstLine)
            {

            }*/

            public RegFileValueBuilder(string name, RegistryValueKind type, string firstLineData)
            {

            }
        }

        /// <summary>
        /// Returns True if and only if the provided string contains an odd number of unescaped quotes.
        /// </summary>
        /// <param name="str">String to test</param>
        public static bool ContainsUnmatchedQuote(string str)
        {
            return (str.Replace(@"\\", "").Replace("\\\"", "").Count(f => f == '"') % 2) == 1;
        }

        /// <summary>
        /// General exception for failure while parsing a .reg file.
        /// </summary>
        [Serializable]
        public class RegistryFileParsingException : Exception
        {
            public string Line { get; set; }
            public RegistryFileParsingException(string line, string message) : base(message)
            {
                Line = line;
            }
        }
    }
}
