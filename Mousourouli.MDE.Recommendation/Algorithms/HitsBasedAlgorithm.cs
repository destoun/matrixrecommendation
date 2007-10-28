using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using Mousourouli.MDE.Recommendation;

namespace Mousourouli.MDE.Recommendation.Algorithms
{
    public class HitsBasedAlgorithm: IRecommendation
    {
        public static readonly int TOPITEMS = 10;

        private int _averageBasketSize;
        

        public HitsBasedAlgorithm(int AverageBasketSize)
        {
            System.Diagnostics.Debug.Assert(AverageBasketSize>0);
            _averageBasketSize = AverageBasketSize;
        }


        
        public IList<KeyValuePair<int,double>> GenerateRecommendations(
            MathNet.Numerics.LinearAlgebra.Matrix matrix, 
            IList<TransactionItem> currentBasket)
        {
            System.Diagnostics.Debug.Assert(matrix.RowCount == matrix.ColumnCount);
            
            Matrix subMatrix = new Matrix(matrix.RowCount, matrix.ColumnCount);
            List<int> indices = new List<int>();
            foreach (TransactionItem ti in currentBasket)
            {
                if (!ti.IsPositive)
                    continue;

                indices.Add(ti.Item);
                for (int i = 0; i < matrix.RowCount; i++)
                {
                    subMatrix[ti.Item, i] = matrix[ti.Item, i];
                    subMatrix[i,ti.Item] = matrix[i,ti.Item];
                }

            }

            HitsResult hitsresult = new Hits().CalculateSparseHits(subMatrix,indices);


            hitsresult.Authorities.Sort(delegate(KeyValuePair<int, double> x, KeyValuePair<int, double> y)
            {
                return y.Value.CompareTo(x.Value);

            });

            hitsresult.Hubs.Sort(delegate(KeyValuePair<int, double> x, KeyValuePair<int, double> y)
           {
               return y.Value.CompareTo(x.Value);

           });


            double currentBaskPersentage = ((double)currentBasket.Count / (double)_averageBasketSize) * 100.0;
            IList<KeyValuePair<int,double>> topHubs = TopX(hitsresult.Hubs,TOPITEMS);
            IList<KeyValuePair<int,double>> topAuthorities = TopX(hitsresult.Authorities,TOPITEMS);

            if (currentBaskPersentage <= 20)
            {
                return topHubs;
            }
            else if (currentBaskPersentage > 20 && currentBaskPersentage <= 80)
            {
                return MergeHubsAndAuthorities(topHubs, topAuthorities, TOPITEMS);
            }
            else
            {
                return topAuthorities;

            }
            
         
            return null;
            
        }

        public IList<KeyValuePair<int,double>> TopX(IList<KeyValuePair<int,double>> list,int x)
        {
            IList<KeyValuePair<int,double>> result = new List<KeyValuePair<int,double>>();
            for (int i = 0; i < list.Count  && i< x; i++)
                    result.Add(list[i]);

                return result;
        }


        public IList<KeyValuePair<int, double>> MergeHubsAndAuthorities(
            IList<KeyValuePair<int, double>> hubs,
            IList<KeyValuePair<int, double>> authorities,int x)
        {
            IList<KeyValuePair<int, double>> result = new List<KeyValuePair<int, double>>();
            for (int i = 0; i < hubs.Count && i < x/2; i++)
                result.Add(hubs[i]);

            for (int i = 0; i < authorities.Count && result.Count <= 10; i++)
            {
                if (!result.Contains(authorities[i]))
                    result.Add(authorities[i]);
            }

            return result;

        }




    }
}
