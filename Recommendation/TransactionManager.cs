using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using log4net;

namespace Mousourouli.MDE.Recommendation
{
    class TransactionManager
    {
        IList<IList<TransactionItem>> Transactions;
        Set<int> DistinctItems;
        private ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TransactionManager(string filename)
        {
            Transactions = new List<IList<TransactionItem>>();
            DistinctItems = new Set<int>();

            using (StreamReader sr = new StreamReader(filename))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    IList<TransactionItem> transaction = CreateTransaction(line);
                    Transactions.Add(transaction);
                    
                }
            }

        }

        IList<TransactionItem> CreateTransaction(string line)
        {
            List<TransactionItem> transaction = new List<TransactionItem>();
            if (!String.IsNullOrEmpty(line))
            {
                string[] items = line.Split(' ', '\t');
                int itemValue;
                foreach (string item in items)
                {

                    try
                    {
                        itemValue = Convert.ToInt32(item);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        throw ex;
                    }

                    TransactionItem ti = new TransactionItem(Math.Abs(itemValue), (itemValue >= 0) ? true : false);
                    //TODO: find if item exists
                    DistinctItems.Add(ti.Item);
                    transaction.Add(ti);
                    

                }
            }

            return transaction;
        }

        public void LogTranscaction()
        {
            foreach (IList<TransactionItem> transaction in Transactions)
            {
                log.Debug("Start Trans:{");
                foreach (TransactionItem item in transaction)
                {
                    log.Debug(item);
                }
                log.Debug("}End Trans;");
            }

            StringBuilder sb = new StringBuilder();
            foreach (int itemValue in DistinctItems)
            {
                sb.AppendFormat("{0}:",itemValue);
            }
            log.Debug(sb.ToString());


        }
        
        

    }


}
