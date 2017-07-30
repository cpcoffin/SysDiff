using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coffin.Registry
{
    public enum RegistryValueType { REG_DWORD, REG_SZ }

    /// <summary>
    /// Superclass for in-memory representations of registry values 
    /// </summary>
    public abstract class RegistryValueInfo
    {
        private RegistryKeyInfo _Key;
        public RegistryKeyInfo Key { get { return _Key; } }
        private string _Name;
        public string Name { get { return _Name; } set { throw new NotImplementedException(); } }

        public RegistryValueInfo(RegistryKeyInfo key, string name)
        {
            _Name = name;
            _Key = key;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
