using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SysDiff;

namespace SysDiff_Test
{
    [TestClass]
    public class DataSet_Test
    {
        public TestContext TestContext { get; set; }
        private Util Util;

        [TestInitialize]
        public void Init()
        {
            this.Util = new Util(this.TestContext);
        }

        [TestMethod]
        public void TestConstructor()
        {
            // Arrange
            string name = "TestDataSet";
            string type = "TestType";

            // Act
            DataSet ds = new DataSet(name, type);

            // Assert
            Assert.AreEqual(name, ds.Name, "DataSet constructor sets name");
            Assert.AreEqual(type, ds.Type, "DataSet constructor sets type");
            Assert.AreEqual(0, ds.Instances, "DataSet contsructor initializes Instances to 0");
        }

        [TestMethod]
        [ExpectedException(typeof(DataSet.InstancesNotIncrementedException))]
        public void TestNewDataPoint_NoInstances()
        {
            // Arrange
            string name = "TestDataSet";
            string type = "TestType";
            DataSet ds = new DataSet(name, type);
            string key = "TestKey";
            string value = "TestValue";

            // Act
            ds.NewDataPoint(key, value);

            // Assert - Expects exception
        }

        [TestMethod]
        [ExpectedException(typeof(DataSet.InstancesNotIncrementedException))]
        public void TestNewDataPoint_DupeKeyOnOneInstance()
        {
            // Arrange
            string name = "TestDataSet";
            string type = "TestType";
            DataSet ds = new DataSet(name, type);
            string key = "TestKey";
            string value = "TestValue";
            ds.NewInstance();

            // Act
            ds.NewDataPoint(key, value);
            ds.NewDataPoint(key, value);

            // Assert - Expects exception
        }

        [TestMethod]
        [DataSource(@"Microsoft.VisualStudio.TestTools.DataSource.CSV", @"TestData\DataPoint_ThreeValues.csv", "DataPoint_ThreeValues#csv", DataAccessMethod.Sequential)]
        public void TestCompare()
        {
            // Arrange
            string name0 = "TestDataSet";
            string name1 = "TestDataSet2";
            string type = this.Util.GetTestValue("type");
            string key0 = this.Util.GetTestValue("key");
            string key1 = key0 + "x";
            string key2 = "x" + key0;
            string key3 = "a" + key0;
            string key4 = key0 + "z";
            string value0 = this.Util.GetTestValue("value0");
            string value1 = this.Util.GetTestValue("value1");
            string value2 = this.Util.GetTestValue("value2");
            string value2x = value2 + "x";
            DataSet ds0 = new DataSet(name0, type);
            DataSet ds1 = new DataSet(name1, type);

            ds0.NewInstance();
            ds0.NewDataPoint(key0, value0);
            ds0.NewDataPoint(key1, value0);
            ds0.NewDataPoint(key4, value0);
            ds0.NewInstance();
            ds0.NewDataPoint(key0, value1);
            ds0.NewDataPoint(key1, value1);
            ds0.NewDataPoint(key4, value0);
            ds0.NewInstance();
            ds0.NewDataPoint(key0, value2);
            ds0.NewDataPoint(key1, value2x); // only difference for key1
            ds0.NewDataPoint(key4, value0); // key4 always value0
            // key2 never exist
            // key3 never exist

            ds1.NewInstance();
            ds1.NewDataPoint(key0, value0);
            ds1.NewDataPoint(key1, value0);
            ds1.NewDataPoint(key2, value0);
            ds1.NewDataPoint(key3, value0);
            ds1.NewInstance();
            ds1.NewDataPoint(key0, value1);
            ds1.NewDataPoint(key1, value1);
            ds1.NewDataPoint(key2, value1);
            ds1.NewDataPoint(key3, value1);
            ds1.NewDataPoint(key4, value1);
            ds1.NewInstance();
            ds1.NewDataPoint(key0, value2);
            ds1.NewDataPoint(key1, value2);
            ds1.NewDataPoint(key2, value2);
            ds1.NewDataPoint(key4, value2); // key4 value1 or value2, not always exist
                                            // key 3 not always exist

            // Act
            List<DataPointComparison> comparison = ds0.Compare(ds1);
            DataPointComparison c0 = null, c1 = null, c2 = null, c3 = null, c4 = null;
            foreach (DataPointComparison c in comparison)
            {
                if (c.Key == key0)
                    c0 = c;
                else if (c.Key == key1)
                    c1 = c;
                else if (c.Key == key2)
                    c2 = c;
                else if (c.Key == key3)
                    c3 = c;
                else if (c.Key == key4)
                    c4 = c;
            }

            // Assert
            Assert.AreEqual(5, comparison.Count,
                "Compare should return one ComparisonResult for each unique key");
            Assert.AreEqual(CompareResult.Identical, c0.Similarity,
                "Compare should return a ComparisonResult with Similarity of Identical for identical data points.");
            Assert.AreEqual(CompareResult.Overlap, c1.Similarity,
                "Compare should return a ComparisonResult with Similarity of Overlap for overlapping data points.");
            Assert.AreEqual(CompareResult.NoOverlap, c2.Similarity,
                "Compare should return a ComparisonResult with Similarity of NoOverlap when always exists compared to never exists.");
            Assert.AreEqual(CompareResult.Overlap, c3.Similarity,
                "Compare should return a ComparisonResult with Similarity of Overlap when sometimes exists compared to never exists. " + c3.Key);
            Assert.AreEqual(CompareResult.NoOverlap, c4.Similarity,
                "Compare should return a ComparisonResult with Similarity of NoOverlap when one always exists and no values overlap.");

        }
    }
}
