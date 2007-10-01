using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace Mousourouli.MDE.Recommendation.Algorithms
{
    public interface IRecommendation
    {
        IList<KeyValuePair<int,double>> GenerateRecommendations(Matrix matrix, IList<TransactionItem> currentBasket);

    }

}
