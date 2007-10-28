using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using log4net;


namespace Mousourouli.MDE.Recommendation
{
    public class Hits
    {
        protected static readonly int ITERATIONS = 25;

        [NonSerialized]
        private static ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Hits()
        {

        }

        public HitsResult CalculateHits(Matrix matrix)
        {
            log.Debug(string.Format("CalculateHits:{0}:{1}" , matrix.RowCount, matrix.ColumnCount));

            Matrix initialVector = new Matrix(matrix.RowCount, 1, 1.0);

            Matrix l = matrix;
            Matrix hubs = initialVector;
            Matrix authorities = hubs.Clone();

            Matrix lt = Matrix.Transpose(l);
            Matrix llt = l * lt; //llt hub matrix
            Matrix ltl = lt * l; //ltl authority matrix

            for (int iteration = 0; iteration < ITERATIONS; iteration++)
            {
                log.Debug("Iteration:" + iteration);
                hubs = llt * hubs;
                hubs.Multiply(1 / hubs.Norm1());

                authorities = ltl * authorities;
                authorities.Multiply(1 / authorities.Norm1());
            }

            log.Debug("CalculateHits:return");
            return new HitsResult(VectorToList(hubs), VectorToList(authorities));

        }

        private IList<double> VectorToList(Matrix vector)
        {
            System.Diagnostics.Debug.Assert(vector.ColumnCount == 1);
            List<double> result = new List<double>();
            for (int i = 0; i < vector.RowCount; i++)
                result.Add(vector[i, 0]);

            return result;
        }



    }

    public class HitsResult
    {
        
        public IList<double> Hubs
        {
            get { return _hubs; }
        }IList<double> _hubs;
        
        public IList<double> Authorities
        {
            get { return _authorities; }
        }IList<double> _authorities;

        public HitsResult(IList<double> hub, IList<double> authorities)
        {
            _hubs = hub;
            _authorities = authorities;

        }



    }

}
