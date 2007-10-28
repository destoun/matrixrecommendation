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
    class CreateMatrixProgram
    {
        private static ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        static CreateMatrixProgram()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        static void Main(string[] args)
        {
            log.Info("Start");
            String DataFile = System.Configuration.ConfigurationManager.AppSettings["DataFile"];
            String WeightingSchema = System.Configuration.ConfigurationManager.AppSettings["WeightingSchema"];

            String MappingFile = System.Configuration.ConfigurationManager.AppSettings["MappingFile"];
            String MatrixFile = System.Configuration.ConfigurationManager.AppSettings["MatrixFile"];

            log.Info("Read Transactions");
            TransactionManager tm = new TransactionManager(DataFile);
            Mapping mapping = new Mapping(tm.DistinctItems);
            
            WeightingSchema BWSchema;
            if (WeightingSchema.StartsWith("Bool"))
                BWSchema  = new BooleanWeightingSchema();
            else
                BWSchema = new DistanceBasedWeightingSchema();

            log.Info("Creating Matrix");
            MatrixCreator mc = new MatrixCreator();
            Matrix matrix = mc.Generate(tm, mapping, BWSchema);

            log.Info("Saving Files");
            Utilities.SaveToFile(MappingFile,mapping);
            Utilities.SaveToFile(MatrixFile, matrix);

            if (log.IsDebugEnabled)
                LogMatrix(matrix);
            
            log.Info("Done!");

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
