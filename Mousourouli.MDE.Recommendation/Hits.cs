using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using log4net;


namespace Mousourouli.MDE.Recommendation
{
    public class Hits
    {
        protected static readonly int ITERATIONS = 15;

        [NonSerialized]
        private static ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Hits()
        {

        
        }


        public HitsResult CalculateSparseHits(Matrix matrix,IList<int> indices)
        {
            log.Debug(string.Format("CalculateHits:{0}:{1}" , matrix.RowCount, matrix.ColumnCount));

            Matrix initialVector = new Matrix(matrix.RowCount, 1, 1.0);

            Matrix l = matrix.Clone();
            Matrix hubs = initialVector;
            Matrix authorities = hubs.Clone();

            Matrix lt = Matrix.Transpose(l);
            log.Debug("llt");
            Matrix llt = MutliplySparseMatrices( l , lt, indices); //llt hub matrix
            log.Debug("ltl");
            Matrix ltl = MutliplySparseMatrices( lt , l, indices); //ltl authority matrix

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


        Matrix MutliplySparseMatrices(Matrix m1, Matrix m2, IList<int> indices)
        {
            Matrix X = new Matrix(m1.RowCount, m2.ColumnCount);
			for (int j = 0; j < m2.ColumnCount; j++)
			{
				for (int i = 0; i < m1.RowCount; i++)
				{
					double s = 0;
					foreach(int k in indices) 
					{
						s += m1[i, k] * m2[k, j];
					}
					X[i, j] = s;
				}
			}
			return X;

        }



        public HitsResult CalculateHits(Matrix matrix)
        {
            log.Debug(string.Format("CalculateHits:{0}:{1}" , matrix.RowCount, matrix.ColumnCount));

            Matrix initialVector = new Matrix(matrix.RowCount, 1, 1.0);

            Matrix l = matrix.Clone();
            Matrix hubs = initialVector;
            Matrix authorities = hubs.Clone();

            Matrix lt = Matrix.Transpose(l);
            log.Debug("llt");
            Matrix llt = l * lt; //llt hub matrix
            log.Debug("ltl");
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

        private List<KeyValuePair<int,double>> VectorToList(Matrix vector)
        {
            System.Diagnostics.Debug.Assert(vector.ColumnCount == 1);
            List<KeyValuePair<int, double>> result = new List<KeyValuePair<int, double>>();
            for (int i = 0; i < vector.RowCount; i++)
                result.Add(new KeyValuePair<int,double>(i,vector[i, 0]));

            return result;
        }



    }

    public class HitsResult
    {

        public List<KeyValuePair<int, double>> Hubs
        {
            get { return _hubs; }
        }List<KeyValuePair<int, double>> _hubs;

        public List<KeyValuePair<int, double>> Authorities
        {
            get { return _authorities; }
        }List<KeyValuePair<int, double>> _authorities;

        public HitsResult(List<KeyValuePair<int, double>> hub, List<KeyValuePair<int, double>> authorities)
        {
            _hubs = hub;
            _authorities = authorities;

        }



    }





}
