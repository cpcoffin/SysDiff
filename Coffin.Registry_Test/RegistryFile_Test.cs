using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Coffin.Registry;

namespace Coffin.Registry_Test
{
    [TestClass]
    public class RegistryFile_Test
    {
        [TestMethod]
        public void TestConstructor_NewFile()
        {
            // Arrange
            string tempfile = System.IO.Path.GetTempFileName();

            // Act
            RegistryFile r = new RegistryFile(tempfile);

            // Assert
            Assert.IsTrue(System.IO.File.Exists(tempfile));
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
            RegistryFile r = new RegistryFile(tempfile);

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
            RegistryFile r = new RegistryFile(tempfile);

            // Assert
            //r[Microsoft.Win32.RegistryHive.LocalMachine]

        }
    }
}
