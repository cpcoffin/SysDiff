using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysDiff
{
    /// <summary>
    /// Represents observed data over a set of entities, which can then be compared to another
    /// dataset to determine the attributes that distinguish the two sets from each other.
    /// </summary>
    public class DataSet
    {
        public Dictionary<string, DataPoint> DataPoints { get; }
        public string Type { get; }
        public string Name { get; }
        private int _Instances;  // compare to datapoint count to see if a given key always exists
        public int Instances { get { return _Instances; } }

        public DataSet(string Name, string Type)
        {
            this.Name = Name;
            this.Type = Type;
            this._Instances = 0;
            this.DataPoints = new Dictionary<string, DataPoint>();
        }

        public void NewInstance()
        {
            this._Instances++;
        }
        
        public void NewDataPoint(string Key, string Value)
        {
            if (this.Instances == 0)
            {
                // Callers must call NewInstance() before adding data points
                throw new InstancesNotIncrementedException();
            }
            if (this.DataPoints.ContainsKey(Key))
            {
                if (this.Instances == 1)
                {
                    // Should not be getting multiple datapoints for a single key on the first instance
                    throw new InstancesNotIncrementedException();
                }
                this.DataPoints[Key].AddValue(Value);
            }
            else
            {
                this.DataPoints[Key] = new DataPoint(this, this.Type, Key, Value);
            }
        }

        public List<DataPointComparison> Compare(DataSet other)
        {
            List<DataPointComparison> result = new List<DataPointComparison>();
            IEnumerable<string> Keys = this.DataPoints.Keys.Union(other.DataPoints.Keys);
            foreach (string key in Keys)
            {
                DataPoint thisDP, otherDP;
                this.DataPoints.TryGetValue(key, out thisDP);
                other.DataPoints.TryGetValue(key, out otherDP);
                result.Add(new DataPointComparison(thisDP, otherDP));
            }
            return result;
        }

        [Serializable]
        public class InstancesNotIncrementedException : Exception
        {
            public InstancesNotIncrementedException()
            { }
        }
    }
}
