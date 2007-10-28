using System;
using System.Collections.Generic;
using System.Text;
using Mousourouli.MDE.Recommendation;
using Mousourouli.MDE.Recommendation.Algorithms;
using NUnit.Framework;
using MathNet.Numerics.LinearAlgebra;

namespace Mousourouli.MDE.Tests
{
    [TestFixture]
    public class HitsTest
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




        static HitsTest()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [SetUp]
        public  void SetUp()
        {

        }


        [Test]
        public void PerformHitsTest()
        {
            Matrix l = new Matrix(A);
            HitsResult result = new Mousourouli.MDE.Recommendation.Hits().CalculateHits(l);

            Assert.AreEqual(result.Authorities[0], 0.0, 0.0001);
            Assert.AreEqual(result.Authorities[1], 0.0, 0.0001);
            Assert.AreEqual(result.Authorities[2], 0.3660, 0.0001);

            Assert.AreEqual(result.Hubs[0], 0.3660, 0.0001);
            Assert.AreEqual(result.Hubs[1], 0.0, 0.0001);
            Assert.AreEqual(result.Hubs[2], 0.2113, 0.0001);


        }

        public static string ListVectorToString(IList<double> m)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[[ ");
            for (int i = 0; i < m.Count; i++)
            {

                sb.AppendFormat("{0:0.0000}, ", m[i]);


            }
            sb.Append("]]\n");
            return sb.ToString();
        }



    }
}
