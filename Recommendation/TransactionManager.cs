using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using log4net;

namespace Mousourouli.MDE.Recommendation
{
    public class TransactionManager: IEnumerable<IList<TransactionItem>>   
    {
        IList<IList<TransactionItem>> _Transactions;
        Set<int> _DistinctItems;
        private ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region Constructors
        public TransactionManager(string filename)
            : this(File.Open(filename, FileMode.Open))
        {

        }

        public TransactionManager(Stream stream)
        {
            _Transactions = new List<IList<TransactionItem>>();
            _DistinctItems = new Set<int>();

            using (StreamReader sr = new StreamReader(stream))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    IList<TransactionItem> transaction = CreateTransaction(line);
                    _Transactions.Add(transaction);

                }
            }

        }

        #endregion 

        public IList<TransactionItem> this[int i]
        {
            get
            {
                return _Transactions[i];
            }

        }

        public int Count
        {
            get
            {
                return _Transactions.Count;
            }
        }


        

        IList<TransactionItem> CreateTransaction(string line)
        {
            List<TransactionItem> transaction = new List<TransactionItem>();
            if (!String.IsNullOrEmpty(line))
            {
                string[] items = line.Trim().Split(' ', '\t');
                int itemValue;
                foreach (string item in items)
                {
                    string timmedItem = item.Trim();
                    if (timmedItem.Length == 0)
                        continue;

                    try
                    {
                        itemValue = Convert.ToInt32(timmedItem);
                    }
                    catch (Exception ex)
                    {
                        //log.Debug("'" + item + "'");
                        log.Error(ex);
                        throw ex;
                    }

                    TransactionItem ti = new TransactionItem(Math.Abs(itemValue), (itemValue >= 0) ? true : false);
                    //TODO: find if item exists
                    _DistinctItems.Add(ti.Item);
                    transaction.Add(ti);
                    

                }
            }

            return transaction;
        }

        public void LogTranscaction()
        {
            foreach (IList<TransactionItem> transaction in _Transactions)
            {
                log.Debug("Start Trans:{");
                foreach (TransactionItem item in transaction)
                {
                    log.Debug(item);
                }
                log.Debug("}End Trans;");
            }

            StringBuilder sb = new StringBuilder();
            foreach (int itemValue in _DistinctItems)
            {
                sb.AppendFormat("{0}:",itemValue);
            }
            log.Debug(sb.ToString());


        }

        
        #region IEnumerable<IList<TransactionItem>> Members

        public IEnumerator<IList<TransactionItem>> GetEnumerator()
        {
            return _Transactions.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }


}
