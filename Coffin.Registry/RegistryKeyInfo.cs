using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coffin.Registry
{
    public enum SourceType { LocalRegistry, RemoteRegistry, RegFile, DatFile };
    
    /// <summary>
    /// In-memory representation of a registry key.
    /// </summary>
    public class RegistryKeyInfo
    {
        public RegistryProvider Provider { get; }
        private string _Name;
        public string Name {
            get
            {
                return _Name;
            }
            set
            {
                this.Provider.SetKeyName(this, value);
                this._Name = value;
            }
        }
        public string Path
        {
            get
            {
                if (this.Parent == null)
                {
                    return this.Name;
                }
                else
                {
                    return this.Parent.Path + "\\" + this.Name;
                }
            }
        }
        public RegistryKeyInfo Parent { get; }
        private Dictionary<string, RegistryKeyInfo> _SubKeys;


        public RegistryKeyInfo(RegistryProvider provider, RegistryKeyInfo parent, string name)
        {
            Provider = provider;
            _Name = name;
            Parent = parent;
            _SubKeys = new Dictionary<string, RegistryKeyInfo>();
        }

        public List<string> GetSubKeyNames()
        {
            return _SubKeys.Keys.ToList<string>();
        }

        public bool ContainsSubKey(string name)
        {
            return _SubKeys.Keys.Contains<string>(name);
        }

        public RegistryKeyInfo GetSubKey(string name)
        {
            return _SubKeys[name];
        }

        public void AddSubKey(RegistryKeyInfo subKey)
        {
            this.Provider.AddSubKey(this, subKey);
            this._SubKeys.Add(subKey.Name, subKey);
        }

        public override string ToString()
        {
            return this.Path;
        }
    }
}
