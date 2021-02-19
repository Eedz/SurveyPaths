using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SurveyPaths;
using ITCLib;
using System.Collections.Generic;

namespace SurveyPathsTests
{
    [TestClass]
    public class SurveyTimingTests
    {
        [TestMethod]
        [TestCategory("SurveyTiming")]
        public void UserGetsQuestion_1ScenTrue()
        {
            SurveyTiming timingRun = new SurveyTiming();

            LinkedQuestion q = new LinkedQuestion();
            q.VarName.RefVarName = "AA001";

            FilterInstruction fi = new FilterInstruction();
            fi.VarName = "AA000";
            fi.Oper = Operation.Equals;
            fi.ValuesStr.Add("1");

            List<FilterInstruction> filterList = new List<FilterInstruction>();
            filterList.Add(fi);
            q.FilterList.Add(filterList);

            Respondent r = new Respondent();
            r.Description = "Test Respondent";
            r.AddResponse("AA000", "1");

            Assert.IsTrue(timingRun.UserGetsQuestion(r, q));
        }

        [TestMethod]
        [TestCategory("SurveyTiming")]
        public void UserGetsQuestion_1ScenFalse()
        {
            SurveyTiming timingRun = new SurveyTiming();

            LinkedQuestion q = new LinkedQuestion();
            q.VarName.RefVarName = "AA001";

            FilterInstruction fi = new FilterInstruction();
            fi.VarName = "AA000";
            fi.Oper = Operation.Equals;
            fi.ValuesStr.Add("1");

            List<FilterInstruction> filterList = new List<FilterInstruction>();
            filterList.Add(fi);
            q.FilterList.Add(filterList);

            Respondent r = new Respondent();
            r.Description = "Test Respondent";
            r.AddResponse("AA000", "2");

            Assert.IsFalse(timingRun.UserGetsQuestion(r, q));
        }

        [TestMethod]
        [TestCategory("SurveyTiming")]
        public void UserGetsQuestion_1ScenNoAns_False()
        {
            SurveyTiming timingRun = new SurveyTiming();

            LinkedQuestion q = new LinkedQuestion();
            q.VarName.RefVarName = "AA001";

            FilterInstruction fi = new FilterInstruction();
            fi.VarName = "AA000";
            fi.Oper = Operation.Equals;
            fi.ValuesStr.Add("1");

            List<FilterInstruction> filterList = new List<FilterInstruction>();
            filterList.Add(fi);
            q.FilterList.Add(filterList);

            Respondent r = new Respondent();
            r.Description = "Test Respondent";
            //r.AddResponse("AA000", "2");

            Assert.IsFalse(timingRun.UserGetsQuestion(r, q));
        }

        [TestMethod]
        [TestCategory("SurveyTiming")]
        public void UserGetsQuestion_2ScenNoAns_False()
        {
            SurveyTiming timingRun = new SurveyTiming();

            LinkedQuestion q = new LinkedQuestion();
            q.VarName.RefVarName = "AA001";

            FilterInstruction fi = new FilterInstruction();
            fi.VarName = "AA000";
            fi.Oper = Operation.Equals;
            fi.ValuesStr.Add("1");

            List<FilterInstruction> filterList = new List<FilterInstruction>();
            filterList.Add(fi);
            q.FilterList.Add(filterList);

            FilterInstruction fi2 = new FilterInstruction();
            fi.VarName = "AA000";
            fi.Oper = Operation.Equals;
            fi.ValuesStr.Add("2");


            List<FilterInstruction> filterList2 = new List<FilterInstruction>();
            filterList2.Add(fi);
            q.FilterList.Add(filterList2);

            Respondent r = new Respondent();
            r.Description = "Test Respondent";
            //r.AddResponse("AA000", "2");

            Assert.IsFalse(timingRun.UserGetsQuestion(r, q));
        }

        [TestMethod]
        [TestCategory("SurveyTiming")]
        public void UserGetsQuestion_2Scen_Meets1_True()
        {
            SurveyTiming timingRun = new SurveyTiming();

            LinkedQuestion q = new LinkedQuestion();
            q.VarName.RefVarName = "AA001";

            FilterInstruction fi = new FilterInstruction();
            fi.VarName = "AA000";
            fi.Oper = Operation.Equals;
            fi.ValuesStr.Add("1");

            List<FilterInstruction> filterList = new List<FilterInstruction>();
            filterList.Add(fi);
            q.FilterList.Add(filterList);

            FilterInstruction fi2 = new FilterInstruction();
            fi.VarName = "AA000";
            fi.Oper = Operation.Equals;
            fi.ValuesStr.Add("2");


            List<FilterInstruction> filterList2 = new List<FilterInstruction>();
            filterList2.Add(fi);
            q.FilterList.Add(filterList2);

            Respondent r = new Respondent();
            r.Description = "Test Respondent";
            r.AddResponse("AA000", "2");

            Assert.IsTrue(timingRun.UserGetsQuestion(r, q));
        }
    }
}
