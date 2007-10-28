using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using log4net;


namespace Mousourouli.MDE.Recommendation
{
    public class Hits
    {
        protected int _iterations = 15;

        [NonSerialized]
        private static ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Hits()
        {

        
        }

        public Hits(int iterations)
        {
            _iterations = iterations;
        }


        public HitsResult CalculateSparseHits(Matrix matrix,IList<int> indices)
        {
            log.Debug(string.Format("CalculateHits:{0}:{1}" , matrix.RowCount, matrix.ColumnCount));

            Matrix initialVector = new Matrix(matrix.RowCount, 1, 1.0);

            

            Matrix l = matrix.Clone();
            
            Matrix hubs = initialVector;
            Matrix authorities = hubs.Clone();
            Matrix lt = Matrix.Transpose(l);
            Matrix llt = MutliplySparseMatrices( l , lt, indices); //llt hub matrix
            Matrix ltl = MutliplySparseMatrices( lt , l, indices); //ltl authority matrix
            
            for (int iteration = 0; iteration < _iterations; iteration++)
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

        string LogIndices(IList<int> indices)
        {
            
            StringBuilder sb = new StringBuilder();
            foreach (int i in indices)
                sb.AppendFormat("{0}:", i);
            sb.Append("\r\n");

            return sb.ToString();
        }


        Matrix MutliplySparseMatrices(Matrix m1, Matrix m2, IList<int> indices)
        {

            //return m1 * m2;
            Matrix X = new Matrix(m1.RowCount, m2.ColumnCount);
            for (int j = 0; j < m2.ColumnCount; j++)
            {
                for (int i = 0; i < m1.RowCount; i++)
                {
                    double s = 0;
                    if (indices.Contains(i) || indices.Contains(j))
                    {
                        for(int k = 0; k< m2.ColumnCount; k++)
                        {
                            s += m1[i, k] * m2[k, j];
                        }
                    }
                    else
                    {
                        foreach (int k in indices)
                        {
                            s += m1[i, k] * m2[k, j];
                        }
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
            Matrix llt = l * lt; //llt hub matrix
            Matrix ltl = lt * l; //ltl authority matrix
           
            for (int iteration = 0; iteration < _iterations; iteration++)
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
