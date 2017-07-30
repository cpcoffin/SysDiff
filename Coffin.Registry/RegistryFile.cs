using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Coffin.Registry
{
    /// <summary>
    /// RegistryProvider that loads keys from a .reg file.
    /// </summary>
    public class RegistryFile : RegistryProvider
    {
        /// <summary>
        /// Path to the associated .reg file.
        /// </summary>
        public string FilePath { get; set; }

        static private Regex _RegexWhitespaceOnly = new Regex(@"^\s*$", RegexOptions.Compiled);
        static private Regex _RegexNewKey = new Regex(@"^\s*\[(" + Strings.HKEY_LOCAL_MACHINE + "|" + Strings.HKEY_CURRENT_USER + "|" +
                                                                   Strings.HKEY_CLASSES_ROOT + "|" + Strings.HKEY_USERS + "|" +
                                                                   Strings.HKEY_CURRENT_CONFIG + @")\\(.{1,})\]$", RegexOptions.Compiled);

        public RegistryFile(string filePath)
        {
            FilePath = filePath;
            if (System.IO.File.Exists(filePath))
            {
                this.Load();
            }
            else
            {
                System.IO.File.Create(filePath);
            }
        }
        
        private enum ParserState { Start, FirstKey, NextItem };

        /// <summary>
        /// Reads the .reg file located at FilePath into memory.
        /// </summary>
        private void Load()
        {
            RegistryKeyInfo key = null;
            string line;
            ParserState state = ParserState.Start;

            System.IO.StreamReader file = new System.IO.StreamReader(this.FilePath);
            try
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (_RegexWhitespaceOnly.IsMatch(line))
                        continue;

                    switch (state)
                    {
                        case ParserState.Start:
                            if (line == Strings.RegFileHeader)
                            {
                                state = ParserState.FirstKey;
                            }
                            else
                            {
                                throw new RegistryFileParsingException(line, Strings.EXCEPT_BADHEADER);
                            }
                            break;
                        case ParserState.FirstKey:
                            key = _ParseKey(line);
                            if (key == null)
                            {
                                throw new RegistryFileParsingException(line, Strings.EXCEPT_FIRSTKEY);
                            }
                            break;
                        case ParserState.NextItem:
                            key = _ParseKey(line);
                            if (key == null)
                            {

                            }
                    }
                }
            }
            finally
            {
                file.Close();
            }
        }

        /// <summary>
        /// Internal method to return the the RegistryKeyInfo representing the specified key
        /// path, creating it if it doesn't already exist.
        /// </summary>
        /// <param name="hive">Hive name</param>
        /// <param name="path">Key path</param>
        private RegistryKeyInfo _ParseKey(string line)
        {
            // Parse line
            Match newKeyMatch = _RegexNewKey.Match(line);
            if (!newKeyMatch.Success)
            {
                return null;
            }
            string hive = newKeyMatch.Groups[1].Value;
            string keypath = newKeyMatch.Groups[2].Value;

            // Get hive
            if (!this._Hives.ContainsKey(hive))
            {
                this._Hives.Add(hive, new RegistryKeyInfo(this, null, hive));
            }
            RegistryKeyInfo currentKey = this._Hives[hive];

            // Traverse subkeys for target key, creating any that don't exist
            string[] splitPath = keypath.Split('\\');
            foreach (string nextKeyName in splitPath)
            {
                if (currentKey.ContainsSubKey(nextKeyName))
                {
                    currentKey = currentKey.GetSubKey(nextKeyName);
                }
                else
                {
                    currentKey.AddSubKey(new Registry.RegistryKeyInfo(this, currentKey, nextKeyName));
                    currentKey = currentKey.GetSubKey(nextKeyName);
                }
            }
            return currentKey;
        }
        
        public override void SetDwordData(RegistryValueInfo value, uint data)
        {
            throw new NotImplementedException();
        }

        public override void SetKeyName(RegistryKeyInfo key, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Helper class for parsing registry files. Represents a value that may only be
        /// partially loaded (in the case of values that span multiple lines).
        /// </summary>
        private class RegistryFileValueParser
        {
            private RegistryValueType _Type;
            public RegistryValueType Type { get; }
            /*
            private static void _ParseFirstLine(string line, out string name, out RegistryValueType type, out string data)
            {
                enum ValueParserState { BeforeName, InName, AfterName };
                // Get name (quoted value preceding '=')
                name = "";
                int separatorIndex = -1;
                bool escaping = false;

                foreach (char c in line)
                {

                }
            }

            public static RegistryFileValueParser GetRegistryFileValueParser(string firstLine)
            {

            }

            public RegistryFileValueParser(string firstLine)
            {

            }*/
        }

        /// <summary>
        /// Returns True if and only if the provided string contains an odd number of unescaped quotes.
        /// </summary>
        /// <param name="str">String to check</param>
        public static bool ContainsUnmatchedQuote(string str)
        {
            return (str.Replace(@"\\", "").Replace("\\\"", "").Count(f => f == '"') % 2) == 1;
        }

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
