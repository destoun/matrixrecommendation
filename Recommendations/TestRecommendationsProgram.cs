using System;
using System.Collections.Generic;
using System.Text;
using Mousourouli.MDE.Recommendation.Algorithms;
using MathNet.Numerics.LinearAlgebra;
using log4net;

namespace Mousourouli.MDE.Recommendation
{
    class TestRecommendationsProgram
    {
        private static ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        static TestRecommendationsProgram()
        {
            log4net.Config.XmlConfigurator.Configure();
        }


        static readonly string TestData =
@"1 2 3 4
1 3 4
3 2 1
4 2 1
4 2 1
4 2 1";

        static readonly string TestData2 = @"1";


        static void TestMain(string[] args)
        {
             TransactionManager tm = new TransactionManager(
                                        new System.IO.MemoryStream(
                                            System.Text.ASCIIEncoding.Default.GetBytes(TestData)));

            TransactionManager tm2 = new TransactionManager(
                                        new System.IO.MemoryStream(
                                            System.Text.ASCIIEncoding.Default.GetBytes(TestData2)));

            Mapping mapping = new Mapping(tm.DistinctItems);

            //WeightingSchema BWSchema = new BooleanWeightingSchema();
            WeightingSchema BWSchema = new DistanceBasedWeightingSchema();


            MatrixCreator mc = new MatrixCreator();

            Matrix matrix = mc.Generate(tm, mapping, BWSchema);

            IRecommendation rec = new MatrixBasedAlgorithm();
            IList<TransactionItem> basket = mapping.Real2IndexMapping( tm2[0] );
            IList<KeyValuePair<int, double>> result = rec.GenerateRecommendations(matrix, basket);

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

            Console.WriteLine(sb.ToString());

            Console.WriteLine();

            foreach(KeyValuePair<int,double> vp in result)
            {
                Console.WriteLine(mapping.Index2Real( vp.Key ) + ":" + vp.Value);

            }
            Console.ReadLine();
        }
    }
}
