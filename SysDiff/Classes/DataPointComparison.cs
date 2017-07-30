using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysDiff
{
    public enum CompareResult { Identical, Overlap, NoOverlap };

    /// <summary>
    /// Representation of a comparison between two datapoints.
    /// </summary>
    public class DataPointComparison
    {
        public CompareResult Similarity { get; }
        public DataPoint Left { get; }
        public DataPoint Right { get; }

        public string Type
        {
            get
            {
                // Constructor ensures that both Types are equal if present
                if (this.Left == null)
                    return this.Right.Type;
                else
                    return this.Left.Type;
            }
        }

        public string Key
        {
            get
            {
                // Constructor ensures that both Keys are equal if present
                if (this.Left == null)
                    return this.Right.Key;
                else
                    return this.Left.Key;
            }
        }

        public DataPointComparison(DataPoint left, DataPoint right)
        {
            this.Left = left;
            this.Right = right;
            if (left == null && right == null)
            {
                throw new ArgumentNullException();
            }
            else if (left == null)
            {
                this.Similarity = right.Compare(left);
            }
            else if (right == null)
            {
                this.Similarity = left.Compare(right);
            }
            else
            {
                if (left.Type != right.Type || left.Key != right.Key)
                {
                    throw new UnmatchedDataPointsException();
                }
                this.Similarity = left.Compare(right);
            }
        }

        [Serializable]
        public class UnmatchedDataPointsException : Exception
        {
            public UnmatchedDataPointsException()
            { }
        }
    }
}
