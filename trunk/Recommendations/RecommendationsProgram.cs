using System;
using System.Collections.Generic;
using System.Text;
using Mousourouli.MDE.Recommendation.Algorithms;
using MathNet.Numerics.LinearAlgebra;
using log4net;

namespace Mousourouli.MDE.Recommendation
{
    class RecommendationsProgram
    {
        private static ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        static RecommendationsProgram()
        {
            log4net.Config.XmlConfigurator.Configure();
        }


        static IList<TransactionItem> ReadBasket(string[] args)
        {
            IList<TransactionItem> transaction = new List<TransactionItem>();

            if (args.Length == 0)
                throw new Exception("No transaction provided") ;

            for (int i = 0; i < args.Length; i++)
            {
                string item = args[i].Trim();
                int itemValue = 0;
                try
                {
                    itemValue = Convert.ToInt32(item);
                }
                catch (Exception ex)
                {
                   
                    log.Error(ex);
                    throw ex;
                }

                TransactionItem ti = new TransactionItem(Math.Abs(itemValue), (itemValue >= 0) ? true : false);
               
                transaction.Add(ti);


            }

            return transaction;
        }




        static void Main(string[] args)
        {

            IList<TransactionItem> basket = ReadBasket(args);


            Matrix matrix = Utilities.ReadFromFile("matrix.dat") as Matrix;
            Mapping mapping = Utilities.ReadFromFile("mapping.dat") as Mapping;
            WeightingSchema BWSchema = new DistanceBasedWeightingSchema();

            IRecommendation rec = new MatrixBasedAlgorithm();
            
            IList<KeyValuePair<int, double>> result = rec.GenerateRecommendations(matrix, basket);

            //StringBuilder sb = new StringBuilder();
            //sb.Append("\r\n");
            //for (int i = 0; i < matrix.RowCount; i++)
            //{
            //    for (int j = 0; j < matrix.ColumnCount; j++)
            //    {
            //        sb.AppendFormat("{0}\t", matrix[i, j]);
            //    }
            //    sb.Append("\r\n");

            //}

            //Console.WriteLine(sb.ToString());

            //Console.WriteLine();

            int i = 0;
            foreach(KeyValuePair<int,double> vp in result)
            {
                if ((i++) > 10)
                    break;

                Console.WriteLine(mapping.Index2Real( vp.Key ) + ":" + vp.Value);

            }
            Console.ReadLine();
        }
    }
}
