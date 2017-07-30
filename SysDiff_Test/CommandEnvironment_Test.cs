using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SysDiff;

namespace SysDiff_Test
{
    /// <summary>
    /// Summary description for te
    /// </summary>
    [TestClass]
    public class CommandEnvironment_Test
    {
        public TestContext TestContext { get; set; }
        private Util Util;

        [TestInitialize]
        public void Init()
        {
            this.Util = new Util(this.TestContext);
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestConstructor()
        {
            // Arrange
            CommandEnvironment ci;

            // Act
            ci = new CommandEnvironment();

            // Assert
            Assert.AreEqual(0, ci.DataSets.Count, "Command Environment starts with empty DataSets list");
            Assert.AreEqual(0, ci.Comparisons.Count, "Command Environment starts with empty Comparisons list");
        }

        [TestMethod]
        public void TestTokenize()
        {
            // Arrange
            CommandEnvironment ci = new CommandEnvironment();
            string s1 = "command";
            string s2 = "command arg1";
            string s3 = "command \"arg 1\" \"arg 2\"";

            // Act
            List<string> r1 = ci.tokenize(s1);
            List<string> r2 = ci.tokenize(s2);
            List<string> r3 = ci.tokenize(s3);

            // Assert
            Assert.AreEqual(1, r1.Count, "Single token - token count");
            Assert.AreEqual(2, r2.Count, "Two tokens - token count");
            Assert.AreEqual(3, r3.Count, "Three tokens with quotes - token count");
            Assert.AreEqual("command", r1[0], "Single token - value");
            Assert.AreEqual("command", r2[0], "Two tokens - value 1");
            Assert.AreEqual("arg1", r2[1], "Two tokens - value 1");
            Assert.AreEqual("command", r3[0], "Three tokens with quotes - value 1");
            Assert.AreEqual("arg 1", r3[1], "Three tokens with quotes - value 2");
            Assert.AreEqual("arg 2", r3[2], "Three tokens with quotes - value 3");

        }

        [TestMethod]
        public void TestParse_Quit()
        {
            // Arrange
            CommandEnvironment ci = new CommandEnvironment();

            // Act
            string cmd1, cmd2;
            string[] args1, args2;
            //ci.parse("quit", out cmd1, out args1);
            //ci.parse("exit", out cmd2, out args2);


        }
    }
}
