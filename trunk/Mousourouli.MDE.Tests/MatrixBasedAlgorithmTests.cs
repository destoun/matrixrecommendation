using System;
using System.Collections.Generic;
using System.Text;

using Mousourouli.MDE.Recommendation;
using Mousourouli.MDE.Recommendation.Algorithms;
using NUnit.Framework;

namespace Mousourouli.MDE.Tests
{
    [TestFixture]
    class MatrixBasedAlgorithmTests
    {
 




        static MatrixBasedAlgorithmTests()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [SetUp]
        protected void SetUp()
        {

        }


        [Test]
        public void TestMatrixBasedAlgorithm()
        {
           
            

        }




    }
}
