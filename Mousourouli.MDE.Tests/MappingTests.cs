using System;
using System.Collections.Generic;
using System.Text;
using Mousourouli.MDE.Recommendation;
using NUnit.Framework;


namespace Mousourouli.MDE.Tests
{

    [TestFixture]
    public class MappingTests
    {
        static readonly int[] TestData ={ 1, 6, 73, 39, 308, 2, 33 };

        private IList<int> _DistinctItems;


        static MappingTests()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [SetUp]
        protected void SetUp()
        {
            _DistinctItems = new List<int>(TestData);

        }


        [Test]
        public void TestRealItems2IndexMapping()
        {
            Mapping mapping = new Mapping(_DistinctItems);

            Assert.AreEqual(mapping.Real2Index(1), 0);
            Assert.AreEqual(mapping.Real2Index(6), 1);
            Assert.AreEqual(mapping.Real2Index(308), 4);
            Assert.AreEqual(mapping.Real2Index(33), 6);
            

        }

        [Test]
        public void TestIndex2RealItemsMapping()
        {
            Mapping mapping = new Mapping(_DistinctItems);

            Assert.AreEqual(mapping.Index2Real(0), 1);
            Assert.AreEqual(mapping.Index2Real(1), 6);
            Assert.AreEqual(mapping.Index2Real(4), 308);
            Assert.AreEqual(mapping.Index2Real(6), 33);
            

        }




    }



}
