using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITCLib;

namespace SurveyPaths
{
    // TODO implement IPropertyCHange
    public enum TimingReportType { Undefined, TimingUser, TimingWholeSurvey }

    public class SurveyTiming
    {
        public string Title { get; set; }
        public string SurveyCode { get; set; }
        
        public Respondent User { get; set; }
        public List<LinkedQuestion> Questions { get; set; }
        public List<LinkedQuestion> UserQuestions { get; set; }
        public List<LinkedQuestion> NotInterpreted { get; set; }

        public TimingReportType ReportType { get; set; }
        public TimingType TimingPath { get; set; }
        public int WPM { get; set; }
        public double TotalTime { get; set; }
        public int StartQ { get; set; } // Qnum value where we will start timing

        // default constructor
        public SurveyTiming()
        {
            Title = "New Timing Run";
            SurveyCode = "";
            User = new Respondent();
            Questions = new List<LinkedQuestion>();
            NotInterpreted = new List<LinkedQuestion>();
            UserQuestions = new List<LinkedQuestion>();
            ReportType = TimingReportType.Undefined;
            TimingPath = TimingType.Undefined;
            WPM = 150;
            TotalTime = 0;
            StartQ = 0;
        }

        public int QuestionIndex (string refVarName)
        {
            for (int i = 0; i < Questions.Count(); i++)
            {
                if (Questions[i].VarName.RefVarName.Equals(refVarName))
                    return i;
            }
            return -1;
        }

        public LinkedQuestion QuestionAt(string refVarName)
        {
            for (int i = 0; i < Questions.Count(); i++)
            {
                if (Questions[i].VarName.RefVarName.ToLower().Equals(refVarName.ToLower()))
                    return Questions[i];
            }
            return null;
        }

        public LinkedQuestion QuestionAt(int index)
        {
            return Questions[index];
        }

        /// <summary>
        /// Return the total time it would take to read the questions at a specific WPM.
        /// </summary>
        /// <param name="wpm"></param>
        /// <returns>Total time in seconds</returns>
        public double GetTiming(int wpm)
        {
            double total = 0;
            List<LinkedQuestion> list;
            if (ReportType == TimingReportType.TimingUser)
                list = UserQuestions;
            else
                list = Questions;

            foreach (LinkedQuestion q in list)
            {
                if (q.GetQnumValue() >= StartQ) 
                {
                    
                    total += q.GetTiming(wpm);
                }
            }

            total = (double)total / 60;

            return total ;
        }

        /// <summary>
        /// Return the total weighted time it would take to read the questions at a specific WPM.
        /// </summary>
        /// <param name="wpm"></param>
        /// <returns>Total time in seconds (weighted)</returns>
        public double GetWeightedTiming(int wpm)
        {
            double total = 0;
            List<LinkedQuestion> list;
            if (ReportType == TimingReportType.TimingUser)
                list = UserQuestions;
            else
                list = Questions;

            foreach (LinkedQuestion q in list)
            {
                if (q.GetQnumValue() >= StartQ)//!q.IsDerived()
                {
                    if (q.Weight.Value < 0)
                        total += 0;
                    else 
                        total += q.GetTiming(wpm) * q.Weight.Value;
                }
            }

            total = (double)total / 60;

            return total;
        }

        /// <summary>
        /// Calculates the WPM needed to read all the questions in the given amount of time.
        /// </summary>
        /// <param name="targetTime"></param>
        /// <returns></returns>
        public double GetTargetWPM(double targetTime)
        {
            double targetWPM = 0;
            double totalWords = 0;

            List<LinkedQuestion> list;
            if (ReportType == TimingReportType.TimingUser)
                list = UserQuestions;
            else
                list = Questions;

            foreach (LinkedQuestion q in list)
            {
                if (q.GetQnumValue() >= StartQ) //!q.IsDerived()
                {
                    if (q.Weight.Value < 0)
                        totalWords += 0;
                    else
                        totalWords += q.WordCount() * q.Weight.Value;
                }
            }

            targetWPM = (double) (totalWords / (targetTime * 60))  * 60;

            return targetWPM;
        }

        /// <summary>
        /// Returns true if the provided question is ann "other, specify" but checking if it ends with 'o' and there exists another VarName of the same name witout the 'o'
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        private bool IsOtherSpecify(SurveyQuestion q)
        {
            string varname = q.VarName.RefVarName;
            if (varname.EndsWith("o"))
            {
                var nonO = Questions.Where(x => x.VarName.RefVarName.Equals(varname.Substring(0, varname.Length - 1)));

                if (nonO.Count() > 0)
                    return true;

            }

            return false;
        }


        public List<LinkedQuestion> GetTimingQuestions()
        {
            List<LinkedQuestion> set = new List<LinkedQuestion>();

            foreach (LinkedQuestion q in Questions)
            {
                string varname = q.VarName.RefVarName;
                // derived variables
                //if (varname.EndsWith("v") || q.LitQ.Contains("derived") || q.LitQ.Contains("Derived"))
                //    continue;

                // programmer only
                if (q.PreP.StartsWith("Programmer"))
                    continue;

                // routing screen
                if (varname.StartsWith("RS"))
                    continue;

                // termination scripts
                if (varname.StartsWith("BI9"))
                    continue;

                // 
                if (varname.Equals("BI101"))
                    continue;

                if (varname.Equals("BI102") || varname.Equals("BI104"))
                    continue;

                if (varname.Equals("RSOURCE"))
                    continue;


                if (varname.EndsWith("o"))
                {

                    var nonO = Questions.Where(x => x.VarName.RefVarName.Equals(varname.Substring(0, varname.Length - 1)));

                    if (nonO.Count() > 0)
                        continue;
                }

                set.Add(q);
            }

            return set;
        }

        /// <summary>
        /// get all the questions where the specified question is a direct filter.
        /// </summary>
        public List<LinkedQuestion> GetDirectFilterVarList(LinkedQuestion lq)
        {
            List<LinkedQuestion> result = new List<LinkedQuestion>();
            foreach (LinkedQuestion q in Questions)
            {
                if (q.FilteredOn.Any(x => x.VarName.RefVarName.Equals(lq.VarName.RefVarName)))
                {
                    result.Add(q);
                }
            }
            return result;
        }

        /// <summary>
        /// get all the questions where the provided question is an indirect filter
        /// </summary>
        /// <param name="lq"></param>
        /// <returns></returns>
        public List<LinkedQuestion> GetIndirectFilterVarList(LinkedQuestion lq)
        {
            List<LinkedQuestion> result = new List<LinkedQuestion>();
            List<LinkedQuestion> direct = new List<LinkedQuestion>();

            foreach (LinkedQuestion q in Questions)
            {
                if (q.FilteredOn.Any(x => x.VarName.RefVarName.Equals(lq.VarName.RefVarName)))
                {
                    direct.Add(q);
                }
            }

            foreach (LinkedQuestion q in direct)
            {
                result.Add(q);
                result.AddRange(GetIndirectFilterVarList(q));
            }
            return result;
        }

        /// <summary>
        /// Returns the number of questions in this timing run. Depending on TimingReportType, returns either count for User Questions or All Questions.
        /// </summary>
        /// <returns></returns>
        public int GetQuestionCount()
        {
            if (ReportType == TimingReportType.TimingUser)
            {
                return UserQuestions.Count();

            }else
            {
                return Questions.Count();
            }
        }

        public List<LinkedQuestion> GetRespondentQuestions(Respondent r, bool forTiming = false)
        {
            List<LinkedQuestion> set = new List<LinkedQuestion>();

            List<LinkedQuestion> sourceQuestions;

            // filter out questions not for timing
            if (forTiming)
                sourceQuestions = GetTimingQuestions();
            else
                sourceQuestions = Questions;

            foreach (LinkedQuestion q in sourceQuestions)
            {

                if (UserGetsQuestion(r, q))
                {
                    if (!set.Contains(q)) set.Add(q);
                }
                else
                    r.AddSkip(q.VarName.RefVarName);
            }
            return set;
        }

        public List<LinkedQuestion> GetRespondentQuestions(bool forTiming = false)
        {
            List<LinkedQuestion> set = new List<LinkedQuestion>();

            List<LinkedQuestion> sourceQuestions;

            // filter out questions not for timing
            if (forTiming)
                sourceQuestions = GetTimingQuestions();
            else
                sourceQuestions = Questions;

            foreach (LinkedQuestion q in sourceQuestions)
            {

                if (UserGetsQuestion(User, q))
                {
                    if (!set.Contains(q)) set.Add(q);
                }
                else
                    User.AddSkip(q.VarName.RefVarName);
            }
            return set;
        }

        public bool UserGetsQuestion(Respondent r, LinkedQuestion lq)
        {


            // if there are no filter conditions, it is ask all, so any user gets this question
            if (lq.FilterList.Count == 0)
            {
                return true;
            }
            // if the VarName was skipped, the user does not get this question
            if (r.Responses.Any(x => x.VarName.Equals(lq.VarName.RefVarName) && x.Skipped))
                return false;

            // for each scenario, if it contains any of the respondent's answers, check them and if they are satisfactory, add to set
            // if there are none that match, add to set
            // if every scenario is not satisfied, skip the question
            int failed = 0;
            foreach (List<FilterInstruction> list in lq.FilterList)
            {
                List<FilterInstruction> f = new List<FilterInstruction>();
                foreach (Answer a in r.Responses)
                {
                    f.AddRange(list.Where(x => x.VarName.Equals(a.VarName)).ToList());
                }

                if (f.Count() == 0)
                {
                    failed++;
                }
                else if (r.RespondentSatisfiesFilter(f))
                {
                    return true;
                }
                else
                {
                    failed++;
                }
            }

            // if all filter scenarios failed, user does not get the question, return false
            if (failed == lq.FilterList.Count())
            {
                return false;
            }
            else
            {
                return true;
            }


        }

        public Respondent GetMaxUser(Respondent r)
        {

            Respondent result = new Respondent(r, false);

            foreach (LinkedQuestion lq in Questions)
            {
                // skip ones that already have an answer
                var answers = result.Responses.Where(x => x.VarName.Equals(lq.VarName.RefVarName));
                if (answers.Count() > 0)
                {
                    continue;
                }

                int directFilterCount;
                int indirectFilterCount;

                List<LinkedQuestion> directFilterList = GetDirectFilterVarList(lq);
                directFilterCount = directFilterList.Count();
                List<LinkedQuestion> indirectFilterList = new List<LinkedQuestion>();
                if (lq.GetQnumValue() >= StartQ)
                {
                    
                    foreach (LinkedQuestion q in directFilterList)
                    {
                        indirectFilterList.AddRange(GetIndirectFilterVarList(q));
                    }
                }

                indirectFilterList = indirectFilterList.Except(directFilterList).ToList();
                indirectFilterCount = indirectFilterList.Count();

                List<FilterInstruction> responses = lq.GetFiltersByResponse();
                // get list of responses
                // for each response get direct count, get indirect count

                int max = 0;
                string maxResp = "";
                foreach (FilterInstruction fi in responses)
                {

                    List<LinkedQuestion> directList = GetDirectFilterConditionList(fi);
                    int directCount = directList.Count();

                    List<LinkedQuestion> indirectList = new List<LinkedQuestion>();
                    foreach (LinkedQuestion q in indirectFilterList)
                    {
                        if (QuestionHasIndirectFilter(q, fi))
                            indirectList.Add(q);

                    }
                    int indirectCount = indirectList.Count();

                    // for all responses, get the highest number of direct+indirect
                    // add that response to the respondent
                    if (directCount + indirectCount > max)
                    {
                        max = directCount + indirectCount;
                        maxResp = fi.ValuesStr[0];
                    }
                }

                if (!string.IsNullOrEmpty(maxResp) && UserGetsQuestion(result, lq))
                {
                    result.AddResponse(lq.VarName.RefVarName, maxResp);
                }
            }

            return result;
        }

        public Respondent GetMinUser(Respondent r)
        {
            Respondent result = new Respondent(r, false);

            foreach (LinkedQuestion lq in Questions)
            {

                // skip ones that already have an answer
                if (result.Responses.Any(x => x.VarName.Equals(lq.VarName.RefVarName)))
                    continue;

                int directFilterCount;
                int indirectFilterCount;

                List<LinkedQuestion> directFilterList = GetDirectFilterVarList(lq);
                directFilterCount = directFilterList.Count();

                List<LinkedQuestion> indirectFilterList = new List<LinkedQuestion>();
                if (lq.GetQnumValue() >= StartQ)
                {

                    foreach (LinkedQuestion q in directFilterList)
                    {
                        indirectFilterList.AddRange(GetIndirectFilterVarList(q));
                    }
                }

                indirectFilterList = indirectFilterList.Except(directFilterList).ToList();
                indirectFilterCount = indirectFilterList.Count();

                if (directFilterCount + indirectFilterCount == 0)
                    continue;

                List<FilterInstruction> responses = lq.GetFiltersByResponse();
                // get list of responses
                // for each response get direct count, get indirect count

                int min = 9999999;
                string minResp = "";
                foreach (FilterInstruction fi in responses)
                {

                    List<LinkedQuestion> directList = GetDirectFilterConditionList(fi);
                    int directCount = directList.Count();

                    List<LinkedQuestion> indirectList = new List<LinkedQuestion>();
                    foreach (LinkedQuestion q in indirectFilterList)
                    {
                        if (QuestionHasIndirectFilter(q, fi))
                            indirectList.Add(q);

                    }
                    int indirectCount = indirectList.Count();



                    // for all responses, get the lowest number of direct+indirect
                    // add that response to the respondent
                    if (directCount + indirectCount < min)
                    {
                        min = directCount + indirectCount;
                        minResp = fi.ValuesStr[0];
                    }
                }
                if (!string.IsNullOrEmpty(minResp) && UserGetsQuestion(r, lq))
                {
                    result.AddResponse(lq.VarName.RefVarName, minResp);
                }

            }

            return result;
        }

        // 
        // TODO this doesn't take into account <> > <
        /// <summary>
        /// For a given filter instruction, get the list of VarNames that use it as a filter
        /// </summary>
        /// <param name="fi"></param>
        /// <returns></returns>
        public List<LinkedQuestion> GetDirectFilterConditionList(FilterInstruction fi)
        {
            List<LinkedQuestion> vars = new List<LinkedQuestion>();

            foreach (LinkedQuestion lq in Questions)
            {
                if (QuestionHasFilter(lq, fi))
                {
                    vars.Add(lq);
                }
            }
            return vars;
        }



        public bool QuestionHasIndirectFilter(LinkedQuestion lq, FilterInstruction fi)
        {
            bool hasFilter = false;

            // if any of lq's filters has the filter, add it to the result
            foreach (LinkedQuestion q in ListFilters(lq))
            {
                if (QuestionHasFilter(q, fi))
                {
                    hasFilter = true;
                    break;
                }
            }
            return hasFilter;
        }

        /// <summary>
        /// Returns the list of questions that filter the given question
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public List<LinkedQuestion> ListFilters(LinkedQuestion question)
        {
            List<LinkedQuestion> result = new List<LinkedQuestion>();


            foreach (LinkedQuestion lq in question.FilteredOn)
                if (!result.Contains(lq))
                    result.Add(lq);

            foreach (LinkedQuestion q in question.FilteredOn)
            {
                List<LinkedQuestion> indirect = ListFilters(q);

                foreach (LinkedQuestion i in indirect)
                    if (!result.Contains(i))
                        result.Add(i);
            }

            return result;
        }

        public bool QuestionHasFilter(LinkedQuestion lq, FilterInstruction fi)
        {
            bool hasFilter = false;

            if (fi.ValuesStr.Count() == 0)
                return false;


            string fiValue = fi.ValuesStr[0];

            foreach (List<FilterInstruction> list in lq.FilterList)
            {

                if (fiValue.Length == 1 && char.IsLetter(Char.Parse(fiValue)))
                {
                    // check for letter-type response (C or P usually)
                    if (list.Any(x => x.VarName.Equals(fi.VarName) && x.ValuesStr.Contains(fiValue)))
                    {
                        hasFilter = true;
                        break;
                    }
                }

                else
                {
                    if (list.Any(x => x.Oper == Operation.Equals && x.VarName.Equals(fi.VarName) && x.ValuesStr.Contains(Int32.Parse(fiValue).ToString())))
                    {
                        hasFilter = true;
                        break;
                    }
                    else if (list.Any(x => x.Oper == Operation.NotEquals && x.VarName.Equals(fi.VarName) && !x.ValuesStr.Contains(Int32.Parse(fiValue).ToString())))
                    {
                        hasFilter = true;
                        break;
                    }
                    else if (list.Any(x => x.Oper == Operation.GreaterThan && x.VarName.Equals(fi.VarName) && x.ValuesStr.Any(y => Int32.Parse(y) < Int32.Parse(fiValue))))
                    {
                        // check if fiValue is less than all list values
                        hasFilter = true;
                        break;
                    }
                    else if (list.Any(x => x.Oper == Operation.LessThan && x.VarName.Equals(fi.VarName) && x.ValuesStr.Any(y => Int32.Parse(y) > Int32.Parse(fiValue))))
                    {
                        // check if fiValue is greater than all list values
                        hasFilter = true;
                        break;
                    }


                    // 
                    // TODO <>
                    //if (list.Any(x => x.VarName.Equals(fi.VarName)))

                    //{
                    //    if (list.Any(x=>x.ValuesStr.Contains(Int32.Parse(fi.ValuesStr[0]).ToString())))
                    //    hasFilter = true;
                    //    break;
                    //}
                }
            }

            return hasFilter;
        }

    }
}
