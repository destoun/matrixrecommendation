using System;
using System.Collections.Generic;
using System.Text;
using Mousourouli.MDE.Recommendation.Algorithms;
using MathNet.Numerics.LinearAlgebra;
using log4net;

namespace Mousourouli.MDE.Recommendation
{
    public class Evaluator
    {
        private static ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Matrix _matrix;
        Mapping _mapping;
        IRecommendation _rec;
        double _splitThresholdPercentage;
        List<int> _hits;

        public List<int> Hits
        {
            get { return _hits; }
        }

        public Evaluator(Matrix matrix, Mapping mapping, IRecommendation rec, double splitThresholdPercentage)
        {
            _matrix = matrix;
            _mapping = mapping;
            _rec = rec;
            _splitThresholdPercentage = splitThresholdPercentage;

            if (_splitThresholdPercentage <= 0 || _splitThresholdPercentage >= 1)
                throw new Exception("SplitThershold should be between 0 and 1");

            _hits = new List<int>();

        }

        private  IList<TransactionItem>[] SplitTransaction(IList<TransactionItem> transaction)
        {

            int splitIndex = (int) Math.Ceiling( transaction.Count * _splitThresholdPercentage);
            if (splitIndex == transaction.Count)
                splitIndex -= 1;

            IList<TransactionItem>[] result = new IList<TransactionItem>[2];

            IList<TransactionItem> firstPart = new List<TransactionItem>();
            IList<TransactionItem> secondPart = new List<TransactionItem>();

            for (int i=0; i< transaction.Count ; i++)
            {
                if (i < splitIndex)
                    firstPart.Add(transaction[i]);
                else
                    secondPart.Add(transaction[i]);

            }

            result[0] = firstPart;
            result[1] = secondPart;

            return result;

        }


        public void Evaluate(IList<TransactionItem> transaction)
        {
            //Split transaction 

            if (transaction.Count <= 1)
                return;

            IList<TransactionItem>[] parts = SplitTransaction(transaction);

            IList<KeyValuePair<int, double>> virtualResutls = _rec.GenerateRecommendations(_matrix, 
                                    _mapping.Real2IndexMapping(parts[0]));

            List<int> realResults = new List<int>();

            foreach (KeyValuePair<int, double> item in virtualResutls)
                realResults.Add(_mapping.Index2Real(item.Key));

            _hits.Add( Intersection(realResults, parts[1]));
           
        }

        private int Intersection(List<int> results, IList<TransactionItem> secondPart)
        {
            int count = 0;
            foreach (TransactionItem item in secondPart)
            {
                if (results.Exists(delegate(int match){
                    return item.Item == match;
                }))
                    count++;

            }
            return count;
        }



    }
}
