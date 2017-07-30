using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysDiff
{
    /// <summary>
    /// Represents all observed values for a particular data type and key/ID within a dataset.
    /// </summary>
    public class DataPoint
    {
        public string Type { get; }
        public string Key { get; }
        private int _Instances;
        public List<string> ObservedValues { get; }
        public DataSet DataSet { get; }

        public DataPoint(DataSet dataset, string type, string key, string value)
        {
            if (type == null || key == null || value == null)
            {
                throw new ArgumentNullException();
            }
            this.DataSet = dataset;
            this.Type = type;
            this.Key = key;
            this.ObservedValues = new List<string>();
            this._Instances = 0;
            this.AddValue(value);
        }

        public void AddValue(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }
            this._Instances++;
            if (!this.ObservedValues.Contains(value))
            {
                this.ObservedValues.Add(value);
            }
        }

        // <summary>

        public bool AlwaysExists()
        {
            return this._Instances == this.DataSet.Instances;
        }
        
        /// <summary>
        /// Compares itself to another DataPoint for identical, overlapping, or non-overlapping values.
        /// </summary>
        /// <param name="other">The other DataPoint to compare.</param>
        /// <returns>A CompareResult indicating the level of similarity to the other DataPoint.</returns>
        public CompareResult Compare(DataPoint other)
        {
            /* 
             * Returns CompareResult.Identical if all values match, CompareResult.Overlap if some values
             * match, and CompareResult.NoOverlap if no values match. For purposes of this comparison,
             * non-existence is considered a value. So if .AlwaysExists() returns false for both, that is
             * considered a matching value. If one returns true and the other returns false, that is an
             * unmatched value. If both return true it is not considered a value.
             */
             
            // other == null considered "never exists."
            if (other == null)
            {
                return this.AlwaysExists() ? CompareResult.NoOverlap : CompareResult.Overlap;
            }
            
            int matchCount = 0;
            foreach (string value in other.ObservedValues)
            {
                if (this.ObservedValues.Contains(value))
                {
                    matchCount++;
                }
                else if (matchCount > 0 || (!this.AlwaysExists() && !other.AlwaysExists()))
                {
                    // Not identical
                    // If a match has been encountered, return Overlap
                    // If AlwaysExists() == false for both, that is considered a match
                    return CompareResult.Overlap;
                }
            }

            if (matchCount == 0)
            {
                // No matches found
                return CompareResult.NoOverlap;
            }
            else if (this.AlwaysExists() != other.AlwaysExists())
            {
                // Unmatched results for AlwaysExists() are considered an unmatched value
                return CompareResult.Overlap;
            }
            else if (matchCount == this.ObservedValues.Count && matchCount == other.ObservedValues.Count)
            {
                // All values matched
                return CompareResult.Identical;
            }
            else
            {
                // Some but not all values matched
                return CompareResult.Overlap;
            }                        

            // all values in other processed
            // if any didn't exist, then AlwaysExists() must be true for at least one datapoint
            /* possibilites:
             * - all other values matched
             *   > could be all this values match too - if so matchCount must equal this.count
             *   > could be that other has 0 values - no it can't, datapoints must have at least one value
             *   > so at least one value matched -- which means if we can find an unmatched value Overlap, otherwise Identical
             *      . One AlwaysExists one doesn't?
             *      . this.count > matchCount?
             * - some/not all other values matched
             *   > Overlap
             * - no other values matched. 
             *   > so no this values match. Overlap if neither AlwaysExists (already covered in the loop), otherwise NoOverlap
             *   */
        }
    }
}
