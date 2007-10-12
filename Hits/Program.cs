using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace Hits
{
    class Program
    {
        static void Main(string[] args)
        {
            
            double[,] A = new double[,] 
                        {
                            { 0,0,1,0,1,0},
                            { 1,0,0,0,0,0},
                            { 0,0,0,0,1,0},
                            { 0,0,0,0,0,0},
                            { 0,0,1,1,0,0},
                            { 0,0,0,0,1,0}
                        };

            double[,] V = new double[,] { { 1 }, {1 }, { 1 }, { 1 }, { 1} , { 1 }  };


            Matrix l = new Matrix(A);
            Matrix y0 = new Matrix(V);
            Matrix x0 = y0.Clone();

            Matrix lt = Matrix.Transpose(l);
            Matrix llt = l * lt;
            Matrix ltl = lt *l;

            
            for(int iteration=0;iteration<25;iteration++)
            {
                y0 = llt * y0;
                y0.Multiply(1/y0.Norm1());

                x0 = ltl * x0;
                x0.Multiply(1 / x0.Norm1());
            }
        

            Console.WriteLine(MatrixToString(Matrix.Transpose( y0 )));
            Console.WriteLine(MatrixToString(Matrix.Transpose( x0)));
            Console.ReadLine();

        }



        public static string MatrixToString(Matrix m)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < m.RowCount; i++)
            {
                if (i == 0)
                    sb.Append("[[");
                else
                    sb.Append(" [");
                for (int j = 0; j < m.ColumnCount; j++)
                {
                    if (j != 0)
                        sb.Append(',');
                    sb.AppendFormat("{0:0.0000}",m[i, j]);
                }
                if (i == m.RowCount - 1)
                    sb.Append("]]");
                else
                    sb.Append("]\n");
            }
            return sb.ToString();
        }


    }


        


}

