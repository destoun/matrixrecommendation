using System;
using System.Collections.Generic;
using System.Text;
using log4net;


namespace Mousourouli.MDE.Recommendation
{
    [Serializable]
    public class Mapping
    {
        Dictionary<int, int> Real2IndexItems;
        int[] Index2RealItems;

        [NonSerialized]
        private static ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Mapping()
        {

        }

        public Mapping(Set<int> DistinctItems) :base()
        {
            System.Diagnostics.Debug.Assert(DistinctItems.Count > 0);
            Mapping.log.Debug(string.Format("Set with {0} distinct items", DistinctItems.Count));

            Real2IndexItems = new Dictionary<int, int>(DistinctItems.Count);
            Index2RealItems = new int[DistinctItems.Count];
            int index = 0;
            foreach (int item in DistinctItems)
            {
                Real2IndexItems.Add(item,index);
                Index2RealItems[index] = item;
                index++;
            }
           
            
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
            System.Diagnostics.Debug.Assert(i>=0);
            

            return Real2IndexItems[i];
        }

        public int Index2Real(int i)
        {
            System.Diagnostics.Debug.Assert(i>=0);
            System.Diagnostics.Debug.Assert(Index2RealItems.Length>=i);

            return Index2RealItems[i];
        }

        public IList<TransactionItem> Real2IndexMapping(IList<TransactionItem> transaction)
        {
            List<TransactionItem> result = new List<TransactionItem>();

            foreach (TransactionItem tItem in transaction)
            {
                result.Add(new TransactionItem(Real2Index(tItem.Item),tItem.IsPositive));
            }

            return result;
        }


        public IList<int> Index2RealMapping(IEnumerable<int> recommendation)
        {

            List<int> result = new List<int>();
            
            foreach(int item in recommendation)
            {
                result.Add(Index2Real(item));
            }

            return result;
        }

       

    }
}
