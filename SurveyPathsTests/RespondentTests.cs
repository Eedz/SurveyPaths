using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ITCLib;
using System.Collections.Generic;

namespace SurveyPathsTests
{
    [TestClass]
    public class RespondentTests
    {

        [TestMethod]
        [TestCategory("Respondent")]
        public void Respondent_SatisfiesSingleTerm()
        {
            List<FilterInstruction> fl = new List<FilterInstruction>();

            FilterInstruction fi = new FilterInstruction()
            {
                VarName = "AA001",
                Oper = Operation.Equals
            };
            fi.ValuesStr.Add("1");

            fl.Add(fi);

            ITCLib.Respondent r = new ITCLib.Respondent();

            r.AddResponse("AA001", "1");


            Assert.IsTrue(r.RespondentSatisfiesFilter(fl));
        }

        [TestMethod]
        [TestCategory("Respondent")]
        public void RespondentSatisfiesTwoTerm()
        {
            
            List<FilterInstruction> fl = new List<FilterInstruction>();

            FilterInstruction fi = new FilterInstruction()
            {
                VarName = "AA001",
                Oper = Operation.Equals
            };
            fi.ValuesStr.Add("1");

            FilterInstruction fi2 = new FilterInstruction()
            {
                VarName = "AA002",
                Oper = Operation.Equals
            };
            fi2.ValuesStr.Add("1");
             
            fl.Add(fi);
            fl.Add(fi2);

          

            Respondent r = new Respondent();

            r.AddResponse("AA001", "1");
            r.AddResponse("AA002", "1");

            Assert.IsTrue(r.RespondentSatisfiesFilter(fl));
        }

        [TestMethod]
        [TestCategory("Respondent")]
        public void RespondentFailsSingleTerm()
        {
            LinkedQuestion q = new LinkedQuestion();

            q.PreP = "Ask if AA001=1.";

            List<FilterInstruction> fl = new List<FilterInstruction>();

            FilterInstruction fi = new FilterInstruction()
            {
                VarName = "AA001",
                Oper = Operation.Equals
            };
            fi.ValuesStr.Add("1");

            fl.Add(fi);

            q.FilterList.Add(fl);

            ITCLib.Respondent r = new ITCLib.Respondent();

            r.AddResponse("AA001", "2");


            Assert.IsFalse(r.RespondentSatisfiesFilter(fl));
        }

        [TestMethod]
        [TestCategory("Respondent")]
        public void RespondentFailsTwoTerm()
        {
            LinkedQuestion q = new LinkedQuestion();

            q.PreP = "Ask if AA001=1 and AA002=1.";
            List<FilterInstruction> fl = new List<FilterInstruction>();

            FilterInstruction fi = new FilterInstruction()
            {
                VarName = "AA001",
                Oper = Operation.Equals
            };
            fi.ValuesStr.Add("1");

            FilterInstruction fi2 = new FilterInstruction()
            {
                VarName = "AA002",
                Oper = Operation.Equals
            };
            fi.ValuesStr.Add("1");

            fl.Add(fi);
            fl.Add(fi2);

            q.FilterList.Add(fl);

            Respondent r = new Respondent();

            r.AddResponse("AA001", "1");
            r.AddResponse("AA002", "2");

            Assert.IsFalse(r.RespondentSatisfiesFilter(fl));
        }

        [TestMethod]
        [TestCategory("Respondent")]
        public void RespondentFailsNoAnswer()
        {
            LinkedQuestion q = new LinkedQuestion();

            q.PreP = "Ask if AA001=1 and AA002=1.";
            List<FilterInstruction> fl = new List<FilterInstruction>();

            FilterInstruction fi = new FilterInstruction()
            {
                VarName = "AA001",
                Oper = Operation.Equals
            };
            fi.ValuesStr.Add("1");

            FilterInstruction fi2 = new FilterInstruction()
            {
                VarName = "AA002",
                Oper = Operation.Equals
            };
            fi.ValuesStr.Add("1");

            fl.Add(fi);
            fl.Add(fi2);

            q.FilterList.Add(fl);

            Respondent r = new Respondent();

            r.AddResponse("AA001", "1");
            

            Assert.IsFalse(r.RespondentSatisfiesFilter(fl));
        }
    }
}
