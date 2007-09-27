using System;
using System.Collections.Generic;
using System.Text;
using Mousourouli.MDE.Recommendation;
using NUnit.Framework;


namespace Mousourouli.MDE.Tests
{

    [TestFixture]
    public class TransactionManagerTests
    {
        static readonly string TestData =
@"1 3 6 73 39 308
3 38 29 2 20
2 -1 33 3
";
        

        static TransactionManagerTests()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [SetUp]
        protected void SetUp()
        {
     
        }


        [Test]
        public void TestReadData()
        {
            TransactionManager tm = new TransactionManager(
                                        new System.IO.MemoryStream(
                                            System.Text.ASCIIEncoding.Default.GetBytes(TestData)));


            Assert.AreEqual(tm[0][0].Item, 1);
            Assert.AreEqual(tm[2][1].Item, 1);
            Assert.AreEqual(tm[2][1].IsPositive, false);
            Assert.AreEqual(tm.Count, 3);


            
        }

      


    }



}
