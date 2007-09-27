using System;
using System.Collections.Generic;
using System.Text;
using log4net;

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

            //RecommendationMatrix   ReadDataset(filename, matrix creation algorithm);
            //User current status (input) ... session window
            //Recommendations RecommendationAlgorithm(Matrix, user input, recommendation algorithm);

            TransactionManager tm = new TransactionManager(@"D:\work\iwanna\diplomatki\Code\testdata\kosarak.dat");
            //tm.LogTranscaction();
            Console.WriteLine("End");
            Console.ReadLine();



        }
    }
}
