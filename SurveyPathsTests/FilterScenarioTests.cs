using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ITCLib;
using SurveyPaths;
using System.Collections.Generic;

namespace SurveyPathsTests
{
    [TestClass]
    public class FilterScenarioTests
    {

                [TestMethod]
        [TestCategory("FilterScenarios")]
        public void TestLexer()
        {

            FilterTokenizer tokenizer = new FilterTokenizer();

            var tokens = tokenizer.Tokenize("AA000=1, 2 or 3. ");
        }

        [TestMethod]
        [TestCategory("FilterScenarios")]
        public void TestLexer2()
        {

            FilterTokenizer tokenizer = new FilterTokenizer();

            var tokens = tokenizer.Tokenize("AA000=1, 2 or 3");
        }

        [TestMethod]
        [TestCategory("FilterScenarios")]
        public void TestLexer2Scenarios()
        {

            FilterTokenizer tokenizer = new FilterTokenizer();

            var tokens = tokenizer.Tokenize("AA000=1, 2 or 3 or AA001=2");

            foreach (DslToken t in tokens)
            {

            }
        }

        [TestMethod]
        [TestCategory("FilterScenarios")]
        public void TestLexerAnyOfScenarios()
        {

            FilterTokenizer tokenizer = new FilterTokenizer();

            var tokens = tokenizer.Tokenize("Ask if any of (AA000, AA001, AA002)=1");

            foreach (DslToken t in tokens)
            {

            }
        }

    }
}
