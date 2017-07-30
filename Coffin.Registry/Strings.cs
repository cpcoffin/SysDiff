using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coffin.Registry
{
    internal static class Strings
    {
        internal const string RegFileHeader = "Windows Registry Editor Version 5.00";
        internal const string HKLM = "HKLM";
        internal const string HKEY_LOCAL_MACHINE  = "HKEY_LOCAL_MACHINE";
        internal const string HKEY_CURRENT_USER   = "HKEY_LOCAL_MACHINE";
        internal const string HKEY_CLASSES_ROOT   = "HKEY_LOCAL_MACHINE";
        internal const string HKEY_USERS          = "HKEY_LOCAL_MACHINE";
        internal const string HKEY_CURRENT_CONFIG = "HKEY_LOCAL_MACHINE";

        internal const string EXCEPT_BADHEADER = "Bad header.";
        internal const string EXCEPT_FIRSTKEY = "Expected first key name.";
    }
}
