using System;
using System.Collections.Generic;
using System.Text;

namespace Mousourouli.MDE.Recommendation
{
    [Serializable]
    public class Mapping
    {
        Dictionary<int, int> RealItems2Index;
        int[] Index2RealItems;

        public Mapping(IList<int> DistinctItems)
        {
            RealItems2Index = new Dictionary<int, int>();
           // Index2RealItems = DistinctItems.ToArray();
            
        }

        public int Count
        {
            get
            {
                return Index2RealItems.Length;
            }

        }

        public int Real2Index(int i)
        {
            return 1;
        }

        public int Index2Real(int i)
        {
            return 2;
        }

        public IList<TransactionItem> RealItems2IndexMapping(IList<TransactionItem> transaction)
        {

            return null;
        }


        public IList<int> Index2RealItemsMapping(IList<int> recommendation)
        {
            return new List<int>(new int[] { 3, 2, 3 });
        }

        public void SaveToFile(string filename)
        {
            throw new Exception("UNIMPLEMENTED");
        }


    }
}
