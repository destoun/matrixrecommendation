using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using MathNet.Numerics.LinearAlgebra;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Mousourouli.MDE.Recommendation
{
    class CreateMappingProgram
    {
        private static ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        static CreateMappingProgram()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        static void Main(string[] args)
        {
            log.Info("Start");
            String DataFile = System.Configuration.ConfigurationManager.AppSettings["DataFile"];
            String MappingFile = System.Configuration.ConfigurationManager.AppSettings["MappingFile"];


            log.Info("Read Transactions");
            TransactionManager tm = new TransactionManager(DataFile);
            Mapping mapping = new Mapping(tm.DistinctItems);

            log.Info("Saving Files");
            Utilities.SaveToFile(MappingFile, mapping);

            log.Info("Mapping Done!");


        }
    }
}
