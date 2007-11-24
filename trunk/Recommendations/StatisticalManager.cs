using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace Mousourouli.MDE.Recommendation
{
    public class StatisticalManager
    {
        string _Caption;
        private static ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public StatisticalManager(string Caption)
        {
            _Caption = Caption;
        }


        public void LogStatistics(List<int> hits)
        {
            int TransactionsCount = hits.Count;


            int TransactionHitsCount = 0;
            int OneTransactionItemHitsCount = 0;
            int TwoTransactionItemHitsCount = 0;
            int ThreeTransactionItemHitsCount = 0;
            int OverThreeTransactionItemHitsCount = 0;

            foreach (int i in hits)
            {
                TransactionHitsCount += (i > 0) ? 1 : 0;

                OneTransactionItemHitsCount += (i == 1) ? 1 : 0;
                TwoTransactionItemHitsCount += (i == 2) ? 1 : 0;
                ThreeTransactionItemHitsCount += (i == 3) ? 1 : 0;
                OverThreeTransactionItemHitsCount += (i > 3) ? 1 : 0;


            }

            log.InfoFormat(@"
Statistical Results
{0}
TransactionCount:{1}
TransactionHitsCount:{2}
OneTransactionItemHitsCount:{3}
TwoTransactionItemHitsCount:{4} 
ThreeTransactionItemHitsCount:{5}
OverThreeTransactionItemHitsCount:{6}
EndTime:{7}
", _Caption, TransactionsCount, TransactionHitsCount, OneTransactionItemHitsCount,
 TwoTransactionItemHitsCount, ThreeTransactionItemHitsCount, OverThreeTransactionItemHitsCount, DateTime.Now.ToString());


        }


    }
}
