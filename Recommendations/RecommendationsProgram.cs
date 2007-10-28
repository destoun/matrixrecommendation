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
            
            String MappingFile = System.Configuration.ConfigurationManager.AppSettings["MappingFile"];
            String MatrixFile = System.Configuration.ConfigurationManager.AppSettings["MatrixFile"];
            String NumOfRecommendations = System.Configuration.ConfigurationManager.AppSettings["NumOfRecommendations"];
            String HitsIterations = System.Configuration.ConfigurationManager.AppSettings["HitsIterations"];
            String HitsAverageBasketSize = System.Configuration.ConfigurationManager.AppSettings["HitsAverageBasketSize"];
            String IsSparseMultiplication = System.Configuration.ConfigurationManager.AppSettings["IsSparseMultiplication"];
            String RecommendationAlgorithm = System.Configuration.ConfigurationManager.AppSettings["RecommendationAlgorithm"];


            log.Info("Reading Transaction");
            IList<TransactionItem> basket = ReadBasket(args);


            log.Info("Reading Matrix file");
            log.Info("Reading Mapping file");
            Matrix matrix = Utilities.ReadFromFile(MatrixFile) as Matrix;
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


            log.Info("Calculate Recommendation based on" + RecommendationAlgorithm);
            IList<KeyValuePair<int, double>> result = rec.GenerateRecommendations(matrix,mapping.Real2IndexMapping( basket ));

            StringBuilder sb = new StringBuilder();
            sb.Append("\r\n");
            foreach(KeyValuePair<int,double> vp in result)
            {
                sb.AppendFormat("{0}:{1:0.000}\r\n", mapping.Index2Real( vp.Key ) , vp.Value);
                

            }
            log.Info(sb.ToString());
            
        }
    }
}
