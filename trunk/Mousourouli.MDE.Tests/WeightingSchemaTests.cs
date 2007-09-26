using System;
using System.Collections.Generic;
using System.Text;
using Mousourouli.MDE.Recommendation;
using NUnit.Framework;


namespace Mousourouli.MDE.Tests
{
  
    [TestFixture]
    public class WeightingSchemaTests
    {
        WeightingSchema distanceS;
        WeightingSchema booleanS;

        static WeightingSchemaTests()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [SetUp]
        protected void SetUp()
        {
            log.Debug("SetUp");
            distanceS = new DistanceBasedWeightingSchema();
            booleanS = new BooleanWeightingSchema();
        
        }


        [Test]
        public void TestBooleanSchema()
        {
            Assert.AreEqual(booleanS.CalculatePositiveWeight(1, 2), 1);
            Assert.AreEqual(booleanS.CalculatePositiveWeight(4, 5), 1);
            Assert.AreEqual(booleanS.CalculatePositiveWeight(3, 7), 1);
            Assert.AreEqual(booleanS.CalculateNegativeWeight(3, 7), -1);

        }

        [Test]
        public void TestDistancedBasedSchema()
        {
            Assert.AreEqual(distanceS.CalculatePositiveWeight(1, 2), 1);
            Assert.AreEqual(distanceS.CalculatePositiveWeight(4, 5), 1.0/4.0);
            Assert.AreEqual(distanceS.CalculatePositiveWeight(3, 7), 1.0/3.0);
            Assert.AreEqual(distanceS.CalculateNegativeWeight(3, 7), -1.0/6.0);


        }


    }


   
}
