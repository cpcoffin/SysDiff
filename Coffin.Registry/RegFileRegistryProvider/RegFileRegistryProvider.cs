using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
namespace Coffin.Registry
{
    /// <summary>
    /// RegistryProvider that loads keys from a .reg file.
    /// </summary>
    public class RegFileRegistryProvider : RegistryProvider
    {
        /// <summary>
        /// Path to the associated .reg file.
        /// </summary>
        public string FilePath { get; set; }

        public RegFileRegistryProvider(string filePath)
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

    }
}
*/