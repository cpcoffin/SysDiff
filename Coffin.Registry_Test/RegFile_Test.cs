using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Coffin.Registry;

namespace Coffin.Registry_Test
{
    [TestClass]
    public class RegFile_Test
    {
        [TestMethod]
        public void TestParseKey()
        {
            // Arrange
            string h1 = "HKEY_LOCAL_MACHINE";
            string k1 = "SOFTWARE\\Test";
            string h2 = "hkey_current_user";
            string k2 = "toplevelkey";
            string h3 = "HKEY_Classes_Root";
            string k3 = "key\\with\\\"quotes\"";
            string fail1 = "No\\Brackets";
            string fail2 = "[NoBackslash]";
            string fail3 = "Neither";
            string hive1, key1, hive2, key2, hive3, key3, hive4, key4, hive5, key5, hive6, key6;
            bool r1, r2, r3, r4, r5, r6;

            // Act
            r1 = Coffin.Registry.RegFile.ParseKey("[" + h1 + "\\" + k1 + "]", out hive1, out key1);
            r2 = Coffin.Registry.RegFile.ParseKey("[" + h2 + "\\" + k2 + "]", out hive2, out key2);
            r3 = Coffin.Registry.RegFile.ParseKey("[" + h3 + "\\" + k3 + "]", out hive3, out key3);
            r4 = Coffin.Registry.RegFile.ParseKey(fail1, out hive4, out key4);
            r5 = Coffin.Registry.RegFile.ParseKey(fail2, out hive5, out key5);
            r6 = Coffin.Registry.RegFile.ParseKey(fail3, out hive6, out key6);

            // Assert
            Assert.IsTrue(r1, h1);
            Assert.AreEqual(h1, hive1, h1);
            Assert.AreEqual(k1, key1, k1);
            Assert.IsTrue(r2, h2);
            Assert.AreEqual(h2, hive2, h2);
            Assert.AreEqual(k2, key2, k2);
            Assert.IsTrue(r3, h3);
            Assert.AreEqual(h3, hive3, h3);
            Assert.AreEqual(k3, key3, k3);
            Assert.IsFalse(r4);
            Assert.AreEqual("", hive4, fail1);
            Assert.AreEqual("", key4, fail1);
            Assert.IsFalse(r5);
            Assert.AreEqual("", hive5, fail2);
            Assert.AreEqual("", key5, fail2);
            Assert.IsFalse(r6);
            Assert.AreEqual("", hive6, fail3);
            Assert.AreEqual("", key6, fail3);

        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestLoad_ThrowsOnBadHeader()
        {
            // Arrange
            string tempfile = System.IO.Path.GetTempFileName();
            string contents = @"Not a registry file

[HKEY_LOCAL_MACHINE\SOFTWARE\Test]
@=""""
""test_dword""=dword:0000000f

";
            System.IO.File.WriteAllText(tempfile, contents);

            // Act
            //RegFileRegistryProvider r = new RegFileRegistryProvider(tempfile);

            // Assert - Expects exception
        }

        [TestMethod]
        public void TestLoad_Dword()
        {
            // Arrange
            string tempfile = System.IO.Path.GetTempFileName();
            string contents = @"Windows Registry Editor Version 5.00

[HKEY_LOCAL_MACHINE\SOFTWARE\Test]
@=""""
""test_dword""=dword:0000000f

";
            System.IO.File.WriteAllText(tempfile, contents);

            // Act
            //RegFileRegistryProvider r = new RegFileRegistryProvider(tempfile);

            // Assert
            //r[Microsoft.Win32.RegistryHive.LocalMachine]

        }
    }
}

