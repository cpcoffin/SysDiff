using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SysDiff_Test
{
    class Util
    {
        private TestContext TestContext;

        public Util(TestContext tc)
        {
            this.TestContext = tc;
        }

        public string GetTestValue(string name)
        {
            Object value = this.TestContext.DataRow[name];
            if (value is System.DBNull)
            {
                return null;
            }
            else if ((string)value == "<empty>")
            {
                return "";
            }
            else
            {
                return (string)this.TestContext.DataRow[name];
            }
        }
    }
}
