using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace Mousourouli.MDE.Recommendation.Algorithms
{
    public class MatrixBasedAlgorithm: IRecommendation
    {
        private int _topItems;

        public MatrixBasedAlgorithm(int TopItems)
        {
            _topItems = TopItems;
        }
        
        public IList<KeyValuePair<int,double>> GenerateRecommendations(MathNet.Numerics.LinearAlgebra.Matrix matrix, IList<TransactionItem> currentBasket)
        {


            Matrix RecommendationsVector = new Matrix( 1, matrix.ColumnCount);
            foreach (TransactionItem trItem in currentBasket)
            {
                RecommendationsVector += matrix.GetMatrix(trItem.Item, trItem.Item, 0, matrix.RowCount - 1);
            }

            List<KeyValuePair<int,double>> list = new List<KeyValuePair<int,double>>();
            for (int i = 0; i < RecommendationsVector.ColumnCount; i++)
                list.Add(new KeyValuePair<int, double>(i, RecommendationsVector[0, i]));

            list.Sort(delegate(KeyValuePair<int,double> x, KeyValuePair<int,double> y)
            {
                return y.Value.CompareTo(x.Value);
                
            });


            return TopX( list );
            
        }

        public IList<KeyValuePair<int, double>> TopX(IList<KeyValuePair<int, double>> list)
        {
            IList<KeyValuePair<int, double>> result = new List<KeyValuePair<int, double>>();
            for (int i = 0; i < list.Count && i < _topItems; i++)
                result.Add(list[i]);

            return result;
        }

    }
}
