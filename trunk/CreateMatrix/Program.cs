using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using MathNet.Numerics.LinearAlgebra;


using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;

namespace Mousourouli.MDE.Recommendation
{
    class Program
    {
        private static ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        static Program()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        static void Main(string[] args)
        {
   
            TransactionManager tm = new TransactionManager(@"D:\work\iwanna\diplomatki\Code\testdata\partofkosarak.dat");
            Mapping mapping = new Mapping(tm.DistinctItems);
            
            //            WeightingSchema BWSchema = new BooleanWeightingSchema();
            WeightingSchema BWSchema = new DistanceBasedWeightingSchema();


            MatrixCreator mc = new MatrixCreator();

            Matrix matrix = mc.Generate(tm, mapping, BWSchema);

            Utilities.SaveToFile("mapping.dat",mapping);
            Utilities.SaveToFile("matrix.dat", matrix);
            
            LogMatrix(matrix);

            Console.WriteLine("End");
            //Console.ReadLine();

        }


        public static void LogMatrix(Matrix matrix)
        {


            StringBuilder sb = new StringBuilder();
            sb.Append("\r\n");
            for (int i = 0; i < matrix.RowCount; i++)
            {
                for (int j = 0; j < matrix.ColumnCount; j++)
                {
                    sb.AppendFormat("{0}\t", matrix[i, j]);
                }
                sb.Append("\r\n");

            }

            log.Debug(sb.ToString());

        }

        


    }
}
