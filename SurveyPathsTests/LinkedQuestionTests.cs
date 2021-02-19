using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ITCLib;

namespace SurveyPathsTests
{
    [TestClass]
    public class LinkedQuestionTests
    {

        [TestMethod, TestCategory("LinkedQuestion")]
        public void LQ_GetFiltersByResponse_0RO_0NR()
        {
            LinkedQuestion q = new LinkedQuestion();
            q.VarName.FullVarName = "AA000";
            q.VarName.RefVarName = "AA000";
            q.RespName = "0";
            q.NRName = "0";
            q.RespOptions = "";

            var fl = q.GetFiltersByResponse();
            Assert.IsTrue(fl.Count == 0);

        }

        [TestMethod, TestCategory("LinkedQuestion")]
        public void LQ_GetFiltersByResponse_0RO_1NR()
        {
            LinkedQuestion q = new LinkedQuestion();
            q.VarName.FullVarName = "AA000";
            q.VarName.RefVarName = "AA000";
            q.RespName = "0";
            q.NRName = "rdk";
            q.RespOptions = "8   refused";

            var fl = q.GetFiltersByResponse();
            Assert.IsTrue(fl.Count == 1);

        }

        [TestMethod, TestCategory("LinkedQuestion")]
        public void LQ_GetFiltersByResponse_2RO_0NR()
        {
            LinkedQuestion q = new LinkedQuestion();
            q.VarName.FullVarName = "AA000";
            q.VarName.RefVarName = "AA000";
            q.RespName ="yesno";
            q.RespOptions = "1  Yes\r\n2   No";

            var fl = q.GetFiltersByResponse();
            Assert.IsTrue(fl[0].VarName.Equals("AA000"));
            Assert.IsTrue(fl[0].ValuesStr[0].Equals("1"));
            Assert.IsTrue(fl[1].VarName.Equals("AA000"));
            Assert.IsTrue(fl[1].ValuesStr[0].Equals("2"));
            Assert.IsTrue(fl.Count == 2);
        }

        
    }
}
