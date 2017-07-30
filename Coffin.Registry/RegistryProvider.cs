using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coffin.Registry
{
    public abstract class RegistryProvider
    {
        protected Dictionary<string, RegistryKeyInfo> _Hives;
        public Dictionary<string, RegistryKeyInfo> Hives { get { return _Hives;  } }

        public RegistryProvider()
        {
            _Hives = new Dictionary<string, RegistryKeyInfo>();
        }

        public virtual RegistryKeyInfo this[string hive]
        {
            get { return _Hives[hive]; }
        }

        public abstract void SetDwordData(RegistryValueInfo value, uint data);
        public abstract void SetKeyName(RegistryKeyInfo key, string name);

        public virtual void AddSubKey(RegistryKeyInfo parent, RegistryKeyInfo child)
        {
            // I think only the provider should be adding subkeys to the parent
            // so parent calls this on any add and this can object?
            // or actually no maybe other things can add, but provider would need to write those changes back to the source
            throw new NotImplementedException();
        }
    }
}
