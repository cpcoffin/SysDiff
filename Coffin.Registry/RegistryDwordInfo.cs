using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coffin.Registry
{
    /// <summary>
    /// RegistryValueInfo for type DWORD.
    /// </summary>
    public class RegistryDwordInfo : RegistryValueInfo
    {
        private uint _Value;
        public uint Value { get { return _Value; } set { throw new NotImplementedException(); } }
        public Microsoft.Win32.RegistryValueKind Type { get; }

        public RegistryDwordInfo(RegistryKeyInfo key, string name, uint value)
            : base(key, name)
        {
            _Value = value;
            Type = Microsoft.Win32.RegistryValueKind.DWord;
        }
    }
}
