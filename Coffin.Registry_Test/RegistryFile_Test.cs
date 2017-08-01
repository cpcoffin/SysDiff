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
            //RegFileRegistryProvider r = new RegFileRegistryProvider(tempfile);

            // Assert
            Assert.IsTrue(System.IO.File.Exists(tempfile));
        }


    }
}
