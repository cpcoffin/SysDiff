using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SysDiff;

namespace SysDiff_Test
{
    [TestClass]
    public class DataPoint_Test
    {
        public TestContext TestContext { get; set; }
        private Util Util;
        
        [TestInitialize]
        public void Init()
        {
            this.Util = new Util(this.TestContext);
        }

        [TestMethod]
        [DataSource(@"Microsoft.VisualStudio.TestTools.DataSource.CSV", @"TestData\DataPoint_OneValue.csv", "DataPoint_OneValue#csv", DataAccessMethod.Sequential)]
        public void TestConstructor()
        {
            // Arrange
            string type = this.Util.GetTestValue("type");
            string key = this.Util.GetTestValue("key");
            string value = this.Util.GetTestValue("value0");
            DataSet ds = new DataSet("TestDataSet", type);

            // Act
            ds.NewInstance();
            DataPoint dp = new DataPoint(ds, type, key, value);
            
            // Assert
            Assert.AreEqual(1, dp.ObservedValues.Count,
                "Value count should be 1 after DataPoint constructor.");
            Assert.AreEqual(key, dp.Key,
                "DataPoint constructor should set key to the provided one.");
            Assert.AreEqual(type, dp.Type,
                "DataPoint constructor should set type to the provided one.");
            Assert.AreEqual(value, dp.ObservedValues[0],
                "DataPoint constructor should set first ObservedValue to the provided one");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [DataSource(@"Microsoft.VisualStudio.TestTools.DataSource.CSV", @"TestData\DataPoint_Constructor_Nulls.csv", "DataPoint_Constructor_Nulls#csv", DataAccessMethod.Sequential)]
        public void TestConstructor_ThrowsArgumentNullException()
        {
            // Arrange
            string type = this.Util.GetTestValue("type");
            string key = this.Util.GetTestValue("key");
            string value = this.Util.GetTestValue("value0");

            DataSet ds = new DataSet("TestDataSet", type);
            ds.NewInstance();

            // Act
            DataPoint dp = new DataPoint(ds, type, key, value);

            // Assert - Expects exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [DataSource(@"Microsoft.VisualStudio.TestTools.DataSource.CSV", @"TestData\DataPoint_Constructor_Nulls.csv", "DataPoint_Constructor_Nulls#csv", DataAccessMethod.Sequential)]
        public void TestConstructor_ThrowsArgumentNullExceptionForDataSet()
        {
            // Arrange
            string type = this.Util.GetTestValue("type");
            string key = this.Util.GetTestValue("key");
            string value = this.Util.GetTestValue("value0");

            // Act
            DataPoint dp = new DataPoint(null, type, key, value);

            // Assert - Expects exception
        }

        [TestMethod]
        [DataSource(@"Microsoft.VisualStudio.TestTools.DataSource.CSV", @"TestData\DataPoint_TwoValues.csv", "DataPoint_TwoValues#csv", DataAccessMethod.Sequential)]
        public void TestAddValue()
        {
            // Arrange
            string type = this.Util.GetTestValue("type");
            string key = this.Util.GetTestValue("key");
            string value0 = this.Util.GetTestValue("value0");
            string value1 = this.Util.GetTestValue("value1");

            DataSet ds = new DataSet("TestDataSet", type);
            
            // Act
            ds.NewInstance();
            DataPoint dp = new DataPoint(ds, type, key, value0);
            ds.NewInstance();
            dp.AddValue(value0);
            ds.NewInstance();
            dp.AddValue(value1);
            ds.NewInstance();
            dp.AddValue(value1);

            // Assert
            const int expectedCount = 2;

            Assert.AreEqual(expectedCount, dp.ObservedValues.Count,
                "After one constructor call and three calls to DataPoint's AddValue, with 2 different values, value count should be 2");
            Assert.IsTrue(dp.ObservedValues.Contains(value0),
                "Values passed to DataPoint's AddValue should exist in ObservedValues");
            Assert.IsTrue(dp.ObservedValues.Contains(value1),
                "Values passed to DataPoint's AddValue should exist in ObservedValues");
        }

        [TestMethod]
        [DataSource(@"Microsoft.VisualStudio.TestTools.DataSource.CSV", @"TestData\DataPoint_TwoValues.csv", "DataPoint_TwoValues#csv", DataAccessMethod.Sequential)]
        public void TestAlwaysExists()
        {
            // Arrange
            string type = this.Util.GetTestValue("type");
            string key = this.Util.GetTestValue("key");
            string value0 = this.Util.GetTestValue("value0");
            string value1 = this.Util.GetTestValue("value1");

            DataSet ds = new DataSet("TestDataSet", type);
            
            ds.NewInstance();
            DataPoint dp = new DataPoint(ds, type, key, value0);
            ds.NewInstance();
            dp.AddValue(value1);

            // Act
            bool a = dp.AlwaysExists();
            ds.NewInstance();
            bool b = dp.AlwaysExists();

            // Assert
            Assert.IsTrue(a, "AlwaysExists should return true if DataPoint instance count matches DataSet instance count");
            Assert.IsFalse(b, "AlwaysExists should return false if DataPoint instance count does not match DataSet instance count");
        }


        [TestMethod]
        [DataSource(@"Microsoft.VisualStudio.TestTools.DataSource.CSV", @"TestData\DataPoint_OneValue.csv", "DataPoint_OneValue#csv", DataAccessMethod.Sequential)]
        public void TestCompareValues_OneValue_Identical()
        {
            // Arrange
            string type = this.Util.GetTestValue("type");
            string key = this.Util.GetTestValue("key");
            string value = this.Util.GetTestValue("value0");

            DataSet ds0 = new DataSet("TestDataSet0", type);
            DataSet ds1 = new DataSet("TestDataSet1", type);
            
            ds0.NewInstance();
            DataPoint dp0 = new DataPoint(ds0, type, key, value);

            ds1.NewInstance();
            DataPoint dp1 = new DataPoint(ds1, type, key, value);

            // Act
            CompareResult result0 = dp0.Compare(dp1);
            CompareResult result1 = dp1.Compare(dp0);

            // Assert
            Assert.AreEqual(CompareResult.Identical, result0,
                "Compare should return Identical when both datapoints have a single identical value.");
            Assert.AreEqual(CompareResult.Identical, result1,
                "Compare should return Identical when both datapoints have a single identical value.");
        }

        [TestMethod]
        [DataSource(@"Microsoft.VisualStudio.TestTools.DataSource.CSV", @"TestData\DataPoint_TwoValues.csv", "DataPoint_TwoValues#csv", DataAccessMethod.Sequential)]
        public void TestCompareValues_OneValue_Different()
        {
            // Arrange
            string type = this.Util.GetTestValue("type");
            string key = this.Util.GetTestValue("key");
            string value0 = this.Util.GetTestValue("value0");
            string value1 = this.Util.GetTestValue("value1");

            DataSet ds0 = new DataSet("TestDataSet0", type);
            DataSet ds1 = new DataSet("TestDataSet1", type);
            
            ds0.NewInstance();
            DataPoint dp0 = new DataPoint(ds0, type, key, value0);

            ds1.NewInstance();
            DataPoint dp1 = new DataPoint(ds1, type, key, value1);

            // Act
            CompareResult result0 = dp0.Compare(dp1);
            CompareResult result1 = dp1.Compare(dp0);

            // Assert
            Assert.AreEqual(CompareResult.NoOverlap, result0,
                "Compare should return NoOverlap when both datapoints have a single different value.");
            Assert.AreEqual(CompareResult.NoOverlap, result1,
                "Compare should return NoOverlap when both datapoints have a single different value.");
        }

        [TestMethod]
        [DataSource(@"Microsoft.VisualStudio.TestTools.DataSource.CSV", @"TestData\DataPoint_TwoValues.csv", "DataPoint_TwoValues#csv", DataAccessMethod.Sequential)]
        public void TestCompareValues_OneValue_Different_NotAlwaysExist()
        {
            // Arrange
            string type = this.Util.GetTestValue("type");
            string key = this.Util.GetTestValue("key");
            string value0 = this.Util.GetTestValue("value0");
            string value1 = this.Util.GetTestValue("value1");
            DataSet ds0 = new DataSet("TestDataSet0", type);
            DataSet ds1 = new DataSet("TestDataSet1", type);

            ds0.NewInstance();
            DataPoint dp0 = new DataPoint(ds0, type, key, value0);

            ds1.NewInstance();
            DataPoint dp1 = new DataPoint(ds1, type, key, value1);

            // Adding another instance means AlwaysExists() will return false
            ds0.NewInstance();
            ds1.NewInstance();

            // Act
            CompareResult result0 = dp0.Compare(dp1);
            CompareResult result1 = dp1.Compare(dp0);

            // Assert
            Assert.AreEqual(CompareResult.Overlap, result0,
                "Compare should return Overlap when values do not overlap but AlwaysExists() is false for both datapoints.");
            Assert.AreEqual(CompareResult.Overlap, result1,
                "Compare should return Overlap when values do not overlap but AlwaysExists() is false for both datapoints.");
        }

        [TestMethod]
        [DataSource(@"Microsoft.VisualStudio.TestTools.DataSource.CSV", @"TestData\DataPoint_TwoValues.csv", "DataPoint_TwoValues#csv", DataAccessMethod.Sequential)]
        public void TestCompareValues_TwoValues_Identical()
        {
            // Arrange
            string type = this.Util.GetTestValue("type");
            string key = this.Util.GetTestValue("key");
            string value0 = this.Util.GetTestValue("value0");
            string value1 = this.Util.GetTestValue("value1");

            DataSet ds0 = new DataSet("TestDataSet0", type);
            DataSet ds1 = new DataSet("TestDataSet1", type);

            ds0.NewInstance();
            DataPoint dp0 = new DataPoint(ds0, type, key, value0);
            dp0.AddValue(value1);

            ds1.NewInstance();
            DataPoint dp1 = new DataPoint(ds1, type, key, value0);
            dp1.AddValue(value1);

            // Act
            CompareResult result0 = dp0.Compare(dp1);
            CompareResult result1 = dp1.Compare(dp0);

            // Assert
            Assert.AreEqual(CompareResult.Identical, result0,
                "Compare should return Identical when both datapoints have the same values.");
            Assert.AreEqual(CompareResult.Identical, result1,
                "Compare should return Identical when both datapoints have the same values.");
        }

        [TestMethod]
        [DataSource(@"Microsoft.VisualStudio.TestTools.DataSource.CSV", @"TestData\DataPoint_TwoValues.csv", "DataPoint_TwoValues#csv", DataAccessMethod.Sequential)]
        public void TestCompareValues_TwoValues_NoOverlap()
        {
            // Arrange
            string type = this.Util.GetTestValue("type");
            string key = this.Util.GetTestValue("key");
            string value0 = this.Util.GetTestValue("value0");
            string value1 = this.Util.GetTestValue("value1");

            DataSet ds0 = new DataSet("TestDataSet0", type);
            DataSet ds1 = new DataSet("TestDataSet1", type);
            
            ds0.NewInstance();
            DataPoint dp0 = new DataPoint(ds0, type, key, value0);
            ds0.NewInstance();
            dp0.AddValue(value1);

            ds1.NewInstance();
            DataPoint dp1 = new DataPoint(ds1, type, key, value0 + "x");
            ds1.NewInstance();
            dp1.AddValue(value1 + "x");

            // Act
            CompareResult result0 = dp0.Compare(dp1);
            CompareResult result1 = dp1.Compare(dp0);

            // Assert
            Assert.AreEqual(CompareResult.NoOverlap, result0,
                "Compare should return NoOverlap when datapoints have all different values.");
            Assert.AreEqual(CompareResult.NoOverlap, result1,
                "Compare should return NoOverlap when datapoints have all different values.");
        }

        [TestMethod]
        [DataSource(@"Microsoft.VisualStudio.TestTools.DataSource.CSV", @"TestData\DataPoint_ThreeValues.csv", "DataPoint_ThreeValues#csv", DataAccessMethod.Sequential)]
        public void TestCompareValues_ThreeValues_Overlap()
        {
            // Arrange
            string type = this.Util.GetTestValue("type");
            string key = this.Util.GetTestValue("key");
            string value0 = this.Util.GetTestValue("value0");
            string value1 = this.Util.GetTestValue("value1");
            string value2 = this.Util.GetTestValue("value2");

            DataSet ds0 = new DataSet("TestDataSet0", type);
            DataSet ds1 = new DataSet("TestDataSet1", type);
            DataSet ds2 = new DataSet("TestDataSet2", type);
            DataSet ds3 = new DataSet("TestDataSet3", type);
            DataSet ds4 = new DataSet("TestDataSet3", type);

            ds0.NewInstance();
            DataPoint dp0 = new DataPoint(ds0, type, key, value0);
            ds0.NewInstance();
            dp0.AddValue(value1);
            ds0.NewInstance();
            dp0.AddValue(value2);

            ds1.NewInstance();
            DataPoint dp1 = new DataPoint(ds1, type, key, value0 + "x");
            ds1.NewInstance();
            dp1.AddValue(value1);
            ds1.NewInstance();
            dp1.AddValue(value2);

            ds2.NewInstance();
            DataPoint dp2 = new DataPoint(ds2, type, key, value0);
            ds2.NewInstance();
            dp2.AddValue(value1 + "x");
            ds2.NewInstance();
            dp2.AddValue(value2);

            ds3.NewInstance();
            DataPoint dp3 = new DataPoint(ds3, type, key, value0);
            ds3.NewInstance();
            dp3.AddValue(value1);
            ds3.NewInstance();
            dp3.AddValue(value2 + "x");

            ds4.NewInstance();
            DataPoint dp4 = new DataPoint(ds4, type, key, value0 + "x");
            ds4.NewInstance();
            dp4.AddValue(value1);
            ds4.NewInstance();
            dp4.AddValue(value2 + "x");

            // Act
            CompareResult result0 = dp0.Compare(dp1);
            CompareResult result1 = dp0.Compare(dp2);
            CompareResult result2 = dp0.Compare(dp3);
            CompareResult result3 = dp0.Compare(dp4);

            // Assert
            Assert.AreEqual(CompareResult.Overlap, result0,
                "Compare should return Overlap when datapoints have overlapping values (only first different).");
            Assert.AreEqual(CompareResult.Overlap, result1,
                "Compare should return Overlap when datapoints have overlapping values (only second different).");
            Assert.AreEqual(CompareResult.Overlap, result2,
                "Compare should return Overlap when datapoints have overlapping values (only last different).");
            Assert.AreEqual(CompareResult.Overlap, result3,
                "Compare should return Overlap when datapoints have overlapping values (first and last different).");
        }

    }
}

