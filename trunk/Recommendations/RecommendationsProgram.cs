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


        static void Main(string[] args)
        {
            
            String MappingFile = System.Configuration.ConfigurationManager.AppSettings["MappingFile"];
            String MatrixFile = System.Configuration.ConfigurationManager.AppSettings["MatrixFile"];
            String TransactionsFile = System.Configuration.ConfigurationManager.AppSettings["TransactionsFile"];

            String NumOfRecommendations = System.Configuration.ConfigurationManager.AppSettings["NumOfRecommendations"];
            String HitsIterations = System.Configuration.ConfigurationManager.AppSettings["HitsIterations"];
            String HitsAverageBasketSize = System.Configuration.ConfigurationManager.AppSettings["HitsAverageBasketSize"];
            String IsSparseMultiplication = System.Configuration.ConfigurationManager.AppSettings["IsSparseMultiplication"];
            String RecommendationAlgorithm = System.Configuration.ConfigurationManager.AppSettings["RecommendationAlgorithm"];
            double SplitThreshold = Convert.ToDouble( System.Configuration.ConfigurationManager.AppSettings["SplitThreshold"]);
            

            log.Info("Reading Transaction File");
            TransactionManager tm = new TransactionManager(TransactionsFile);
            log.Info("Reading Matrix file");
            Matrix matrix = Utilities.ReadFromFile(MatrixFile) as Matrix;
            log.Info("Reading Mapping file");
            Mapping mapping = Utilities.ReadFromFile(MappingFile) as Mapping;

            IRecommendation rec;
            if (RecommendationAlgorithm.StartsWith("Matrix"))
            {
                rec = new MatrixBasedAlgorithm(Convert.ToInt32(NumOfRecommendations));
            }
            else
            {
                rec = new HitsBasedAlgorithm(Convert.ToInt32(HitsAverageBasketSize),
                    Convert.ToInt32(NumOfRecommendations),
                    Convert.ToInt32(HitsIterations),
                    Convert.ToBoolean(IsSparseMultiplication));
            }

            string StartCaption = string.Format("Algorithm:{0},SplitThreshold:{1},HitsIterations:{2},HitsAB:{3},{4}", RecommendationAlgorithm, SplitThreshold, 
                HitsIterations, HitsAverageBasketSize, DateTime.Now);
            //log.Info("Calculate Recommendation based on " + RecommendationAlgorithm + " sparse:" + IsSparseMultiplication);
            //Recommendation(tm, matrix, mapping, rec);
            
            StatisticalManager sm = new StatisticalManager(StartCaption);
            sm.LogStatistics(Evaluation(tm, matrix, mapping, rec, SplitThreshold));

            Console.ReadLine();

            
        }


        private static List<int> Evaluation(TransactionManager tm,
            Matrix matrix, Mapping mapping, IRecommendation rec, double splitThreshold)
        {
            int counter = 0;
            Evaluator evaluator = new Evaluator(matrix, mapping, rec, splitThreshold);
            foreach (IList<TransactionItem> basket in tm)
            {
                log.Debug(counter++);
                evaluator.Evaluate(basket);        
            }

            return evaluator.Hits;
        }


        //private static void Recommendation(TransactionManager tm, 
        //    Matrix matrix, Mapping mapping, IRecommendation rec)
        //{
        //    foreach (IList<TransactionItem> basket in tm)
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("\r\nBasket:");
        //        foreach (TransactionItem ti in basket)
        //            sb.AppendFormat("{0}:", ti.PositiveValue * ti.Item);
        //        sb.Append("\r\n");
        //        log.Info(sb.ToString());

        //        IList<KeyValuePair<int, double>> result = rec.GenerateRecommendations(matrix, mapping.Real2IndexMapping(basket));

        //        sb = new StringBuilder();
        //        sb.Append("\r\nRecommendations\r\n");
        //        foreach (KeyValuePair<int, double> vp in result)
        //            sb.AppendFormat("{0}:{1:0.000}\r\n", mapping.Index2Real(vp.Key), vp.Value);

        //        log.Info(sb.ToString());
        //    }
        //}





    }
}
