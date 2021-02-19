using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ITCLib;

namespace SurveyPathsTests
{
    [TestClass]
    public class AnswerTests
    {
        [TestMethod]
        [TestCategory("Answer")]
        public void Answer_SatisfiesFilterInstructionEquals()
        {
            Answer a = new Answer("AA000", "1");

            FilterInstruction fi = new FilterInstruction();
            fi.VarName = "AA000";
            fi.Oper = Operation.Equals;
            fi.ValuesStr.Add("1");
            

            Assert.IsTrue(a.SatisfiesFilter(fi));
        }

        [TestMethod]
        [TestCategory("Answer")]
        public void Answer_SatisfiesFilterInstructionGreater()
        {
            Answer a = new Answer("AA000", "2");

            FilterInstruction fi = new FilterInstruction();
            fi.VarName = "AA000";
            fi.Oper = Operation.GreaterThan;
            fi.ValuesStr.Add("1");
            

            Assert.IsTrue(a.SatisfiesFilter(fi));
        }

        [TestMethod]
        [TestCategory("Answer")]
        public void Answer_SatisfiesFilterInstructionLess()
        {
            Answer a = new Answer("AA000", "1");

            FilterInstruction fi = new FilterInstruction();
            fi.VarName = "AA000";
            fi.Oper = Operation.LessThan;
            fi.ValuesStr.Add("2");


            Assert.IsTrue(a.SatisfiesFilter(fi));
        }

        [TestMethod]
        [TestCategory("Answer")]
        public void Answer_SatisfiesFilterInstructionNotEqual()
        {
            Answer a = new Answer("AA000", "1");

            FilterInstruction fi = new FilterInstruction();
            fi.VarName = "AA000";
            fi.Oper = Operation.NotEquals;
            fi.ValuesStr.Add("2");


            Assert.IsTrue(a.SatisfiesFilter(fi));
        }

        [TestMethod]
        [TestCategory("Answer")]
        public void Answer_NotSatisfiesFilterInstructionEquals()
        {
            Answer a = new Answer("AA000", "2");

            FilterInstruction fi = new FilterInstruction();
            fi.VarName = "AA000";
            fi.ValuesStr.Add("1");
            fi.Oper = Operation.Equals;

            Assert.IsFalse(a.SatisfiesFilter(fi));
        }

        [TestMethod]
        [TestCategory("Answer")]
        public void Answer_NotSatisfiesFilterInstructionGreater()
        {
            Answer a = new Answer("AA000", "1");

            FilterInstruction fi = new FilterInstruction();
            fi.VarName = "AA000";
            fi.ValuesStr.Add("2");
            fi.Oper = Operation.GreaterThan;

            Assert.IsFalse(a.SatisfiesFilter(fi));
        }

        [TestMethod]
        [TestCategory("Answer")]
        public void Answer_NotSatisfiesFilterInstructionLess()
        {
            Answer a = new Answer("AA000", "2");

            FilterInstruction fi = new FilterInstruction();
            fi.VarName = "AA000";
            fi.ValuesStr.Add("1");
            fi.Oper = Operation.LessThan;

            Assert.IsFalse(a.SatisfiesFilter(fi));
        }

        [TestMethod]
        [TestCategory("Answer")]
        public void Answer_NotSatisfiesFilterInstructionNotEqual()
        {
            Answer a = new Answer("AA000", "1");

            FilterInstruction fi = new FilterInstruction();
            fi.VarName = "AA000";
            fi.Oper = Operation.NotEquals;
            fi.ValuesStr.Add("1");


            Assert.IsFalse(a.SatisfiesFilter(fi));
        }
    }
}
