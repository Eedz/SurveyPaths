using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SurveyPathsTests
{
    [TestClass]
    public class WPMTests
    {
        [TestMethod]
        public void TestWPM()
        {
            double timeToRead = 0;

            double words = 32;
            double wpm = 200;

            timeToRead = (double)(words / wpm) ;
            timeToRead *= 60;

        }
    }
}
