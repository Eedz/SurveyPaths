using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITCLib;
using System.Xml;

namespace SurveyPaths
{
    public class UserTiming : Timing 
    {
        //public List<Respondent> UserTypes { get; set; } // should we have a list of users or just have 1 at a time

        public Respondent User { get; private set; }
        public Respondent MaxUser { get; private set; }
        public Respondent MinUser { get; private set; }

        public List<LinkedQuestion> QuestionsBeforeDef { get; set; }

        public List<LinkedQuestion> UserQuestions { get; set; }
        public List<LinkedQuestion> UserQuestionsMax { get; set; }
        public List<LinkedQuestion> UserQuestionsMin { get; set; }

        public double TotalUserTimeMax { get; set; }
        public double TotalUserTimeMin { get; set; }
        public double TotalWeightedUserTime { get; set; }

        // default constructor
        public UserTiming()
        {
            Title = "New Timing Run";
            SurveyCode = "";
            User = new Respondent();
            MaxUser = new Respondent();
            MinUser = new Respondent();
            Questions = new List<LinkedQuestion>();
            NotInterpreted = new List<LinkedQuestion>();
            QuestionsBeforeDef = new List<LinkedQuestion>();

            UserQuestions = new List<LinkedQuestion>();
            UserQuestionsMax = new List<LinkedQuestion>();
            UserQuestionsMin = new List<LinkedQuestion>();
            
            WPM = 150;
            TotalTime = 0;
            TotalWeightedTime = 0;
            TotalUserTimeMin = 0;
            TotalUserTimeMax = 0;
            TotalWeightedUserTime = 0;
            SmartWordCount = false;
            StartQ = 0;
        }


        public UserTiming(string xmlDoc)
        {

            XmlDocument runData = new XmlDocument();

            runData.LoadXml(xmlDoc);
            SurveyCode = runData.SelectSingleNode("/SurveyTiming").Attributes["Survey"].InnerText;
            Title = runData.SelectSingleNode("/SurveyTiming").Attributes["RunTitle"].InnerText;
            TotalTime = Double.Parse(runData.SelectSingleNode("/SurveyTiming/Time").Attributes["Mins"].InnerText) * 60;
            WPM = Int32.Parse(runData.SelectSingleNode("/SurveyTiming/Time").Attributes["WPM"].InnerText);
            StartQ = Int32.Parse(runData.SelectSingleNode("/SurveyTiming/Time").Attributes["StartQ"].InnerText);
            Notes = runData.SelectSingleNode("/SurveyTiming").Attributes["Notes"].InnerText;

            Questions = new List<LinkedQuestion>();

            foreach (XmlNode q in runData.SelectNodes("/SurveyTiming/Questions/Question"))
            {
                LinkedQuestion lq = new LinkedQuestion();

                lq.VarName.FullVarName = q.Attributes["VarName"].InnerText;
                lq.VarName.RefVarName = q.Attributes["refVarName"].InnerText;
                lq.VarName.VarLabel = q.Attributes["VarLabel"].InnerText;
                lq.Qnum = q.Attributes["Qnum"].InnerText;

                lq.PreP = q.Attributes["PreP"].InnerText;
                lq.PreI = q.Attributes["PreI"].InnerText;
                lq.PreA = q.Attributes["PreA"].InnerText;
                lq.LitQ = q.Attributes["LitQ"].InnerText;
                lq.PstI = q.Attributes["PstI"].InnerText;
                lq.PstP = q.Attributes["PstP"].InnerText;
                lq.RespName = q.Attributes["RespName"].InnerText;
                lq.RespOptions = q.Attributes["RespOptions"].InnerText;
                lq.NRCodes = q.Attributes["NRCodes"].InnerText;


                lq.Weight.Value = double.Parse(q.Attributes["Weight"].InnerText);
                lq.Weight.Source = q.Attributes["WeightSource"].InnerText;

                // filter scenarios
                foreach (XmlNode scenarios in q.SelectNodes("/Scenarios"))
                {
                    List<FilterInstruction> list = new List<FilterInstruction>();

                    foreach (XmlNode scenario in scenarios.SelectNodes("/Scenario/FilterInstruction"))
                    {
                        FilterInstruction fi = new FilterInstruction();
                        fi.VarName = scenario.Attributes["VarName"].InnerText;
                        switch (scenario.Attributes["Operation"].InnerText)
                        {
                            case "Equals":
                                fi.Oper = Operation.Equals;
                                break;
                            case "NotEquals":
                                fi.Oper = Operation.NotEquals;
                                break;
                            case "GreaterThan":
                                fi.Oper = Operation.GreaterThan;
                                break;
                            case "LessThan":
                                fi.Oper = Operation.LessThan;
                                break;
                        }

                        foreach (XmlNode resps in scenario.SelectNodes("/Responses/Response"))
                        {
                            fi.ValuesStr.Add(resps.Attributes["ResponseValue"].InnerText);
                        }
                        list.Add(fi);

                    }
                    lq.FilterList.Add(list);
                }

                Questions.Add(lq);

            }

            XmlNode user = runData.SelectSingleNode("/SurveyTiming/UserTypes/User");
            User = new Respondent();
            User.Survey = user.Attributes["Survey"].InnerText;
            User.Description = user.Attributes["Description"].InnerText;
            User.Weight = Double.Parse(user.Attributes["Weight"].InnerText);

            foreach (XmlNode a in runData.SelectNodes("/SurveyTiming/UserTypes/User/Definition/DefinitionAnswer"))
            {
                Answer ans = new Answer(a.Attributes["VarName"].InnerText, a.Attributes["Response"].InnerText);
            }

            XmlNode maxuser = runData.SelectSingleNode("/SurveyTiming/Users/MaxUser");
            MaxUser = new Respondent();
            MaxUser.Survey = User.Survey;
            MaxUser.Description = User.Description;
            MaxUser.Weight = User.Weight;

            foreach (XmlNode a in runData.SelectNodes("/SurveyTiming/UserTypes/MaxUser/Definition/DefinitionAnswer"))
            {
                Answer ans = new Answer(a.Attributes["VarName"].InnerText, a.Attributes["Response"].InnerText);
            }

            UserQuestionsMax = new List<LinkedQuestion>();
            
            foreach (XmlNode maxQ in runData.SelectNodes("/SurveyTiming/MaxQuestions/MaxQuestion"))
            {
                string varname = maxQ.Attributes["VarName"].InnerText;
                var foundq = QuestionAt(varname);
                if (foundq != null)
                    UserQuestionsMax.Add(foundq);
            }

            XmlNode minuser = runData.SelectSingleNode("/SurveyTiming/Users/MinUser");
            MaxUser = new Respondent();
            MaxUser.Survey = User.Survey;
            MaxUser.Description = User.Description;
            MaxUser.Weight = User.Weight;

            foreach (XmlNode a in runData.SelectNodes("/SurveyTiming/UserTypes/MinUser/Definition/DefinitionAnswer"))
            {
                Answer ans = new Answer(a.Attributes["VarName"].InnerText, a.Attributes["Response"].InnerText);
            }

            UserQuestionsMin = new List<LinkedQuestion>();
            foreach (XmlNode minQ in runData.SelectNodes("/SurveyTiming/MinQuestions/MinQuestion"))
            {
                string varname = minQ.Attributes["VarName"].InnerText;
                var foundq = QuestionAt(varname);
                if (foundq != null)
                    UserQuestionsMin.Add(foundq);
            }

        }

        public string ExportToXML()
        {

            XmlDocument timingData = new XmlDocument();

            XmlNode surveyNode = timingData.CreateElement("SurveyTiming");
            timingData.AppendChild(surveyNode);

            XmlAttribute surveyCode = timingData.CreateAttribute("Survey");
            surveyCode.Value = SurveyCode;
            surveyNode.Attributes.Append(surveyCode);

            XmlAttribute runTitle = timingData.CreateAttribute("RunTitle");
            runTitle.Value = Title;
            surveyNode.Attributes.Append(runTitle);

            XmlAttribute scheme = timingData.CreateAttribute("TimingScheme");
            scheme.Value = "TimingUser";
            surveyNode.Attributes.Append(scheme);

            XmlAttribute notes = timingData.CreateAttribute("Notes");
            notes.Value = Notes;
            surveyNode.Attributes.Append(notes);

            XmlNode timing = timingData.CreateElement("Time");
            XmlAttribute time = timingData.CreateAttribute("Mins");
            time.Value = TotalTime.ToString();
            timing.Attributes.Append(time);

            XmlAttribute wpm = timingData.CreateAttribute("WPM");
            wpm.Value = WPM.ToString();
            timing.Attributes.Append(wpm);

            XmlAttribute startQ = timingData.CreateAttribute("StartQ");
            startQ.Value = StartQ.ToString();
            timing.Attributes.Append(startQ);


            surveyNode.AppendChild(timing);

            // <Users   .../>
            XmlNode users = timingData.CreateElement("UserTypes");
            

            

            XmlNode user = timingData.CreateElement("User");

            XmlAttribute survey = timingData.CreateAttribute("Survey");
            survey.Value = User.Survey;
            user.Attributes.Append(survey);

            XmlAttribute description = timingData.CreateAttribute("Description");
            description.Value = User.Description;
            user.Attributes.Append(description);

            XmlAttribute userweight = timingData.CreateAttribute("Weight");
            userweight.Value = User.Weight.ToString();
            user.Attributes.Append(userweight);

            XmlNode userDef = timingData.CreateElement("Definition");

            foreach (Answer a in User.Responses)
            {
                XmlNode def = timingData.CreateElement("DefinitionAnswer");
                XmlAttribute var = timingData.CreateAttribute("VarName");
                var.Value = a.VarName;
                XmlAttribute response = timingData.CreateAttribute("Response");
                response.Value = a.ResponseCode;

                def.Attributes.Append(var);
                def.Attributes.Append(response);
                userDef.AppendChild(def);
            }
            
            user.AppendChild(userDef);
            users.AppendChild(user);
            // max user node

            XmlNode maxuser = timingData.CreateElement("MaxUser");
            XmlNode maxDef = timingData.CreateElement("Definition");
            
            foreach (Answer a in MaxUser.Responses)
            {
                XmlNode def = timingData.CreateElement("DefinitionAnswer");
                XmlAttribute var = timingData.CreateAttribute("VarName");
                var.Value = a.VarName;
                XmlAttribute response = timingData.CreateAttribute("Response");
                response.Value = a.ResponseCode;

                def.Attributes.Append(var);
                def.Attributes.Append(response);
                maxDef.AppendChild(def);
            }
            maxuser.AppendChild(maxDef);
            users.AppendChild(maxuser);

            // min node
            XmlNode minuser = timingData.CreateElement("MinUser");
            XmlNode minDef = timingData.CreateElement("Definition");
            foreach (Answer a in MinUser.Responses)
            {
                XmlNode def = timingData.CreateElement("DefinitionAnswer");
                XmlAttribute var = timingData.CreateAttribute("VarName");
                var.Value = a.VarName;
                XmlAttribute response = timingData.CreateAttribute("Response");
                response.Value = a.ResponseCode;

                def.Attributes.Append(var);
                def.Attributes.Append(response);
                minDef.AppendChild(def);
            }
            minuser.AppendChild(minDef);
            users.AppendChild(minuser);


           
            surveyNode.AppendChild(users);

            XmlNode questions = timingData.CreateElement("Questions");
            surveyNode.AppendChild(questions);

            // add all varnames and their filter vars
            foreach (LinkedQuestion q in Questions)
            {
                XmlNode varname = timingData.CreateElement("Question");

                XmlAttribute name = timingData.CreateAttribute("VarName");
                name.Value = q.VarName.FullVarName;
                varname.Attributes.Append(name);

                XmlAttribute refname = timingData.CreateAttribute("refVarName");
                refname.Value = q.VarName.RefVarName;
                varname.Attributes.Append(refname);

                XmlAttribute varlabel = timingData.CreateAttribute("VarLabel");
                varlabel.Value = q.VarName.VarLabel;
                varname.Attributes.Append(varlabel);

                XmlAttribute qnum = timingData.CreateAttribute("Qnum");
                qnum.Value = q.Qnum;
                varname.Attributes.Append(qnum);

                // wordings
                XmlAttribute prep = timingData.CreateAttribute("PreP");
                prep.Value = q.PreP;
                varname.Attributes.Append(prep);

                XmlAttribute prei = timingData.CreateAttribute("PreI");
                prei.Value = q.PreI;
                varname.Attributes.Append(prei);

                XmlAttribute prea = timingData.CreateAttribute("PreA");
                prea.Value = q.PreA;
                varname.Attributes.Append(prea);

                XmlAttribute litq = timingData.CreateAttribute("LitQ");
                litq.Value = q.LitQ;
                varname.Attributes.Append(litq);

                XmlAttribute psti = timingData.CreateAttribute("PstI");
                psti.Value = q.PstI;
                varname.Attributes.Append(psti);

                XmlAttribute pstp = timingData.CreateAttribute("PstP");
                pstp.Value = q.PstP;
                varname.Attributes.Append(pstp);

                XmlAttribute respoptions = timingData.CreateAttribute("RespOptions");
                respoptions.Value = q.RespOptions;
                varname.Attributes.Append(respoptions);

                XmlAttribute respname = timingData.CreateAttribute("RespName");
                respname.Value = q.RespName;
                varname.Attributes.Append(respname);

                XmlAttribute nrcodes = timingData.CreateAttribute("NRCodes");
                nrcodes.Value = q.NRCodes;
                varname.Attributes.Append(nrcodes);
                
                XmlNode scenarios = timingData.CreateElement("Scenarios");
                varname.AppendChild(scenarios);
                // filters
                foreach (List<FilterInstruction> filterList in q.FilterList)
                {
                    XmlNode scenario = timingData.CreateElement("Scenario");

                    foreach (FilterInstruction fi in filterList)
                    {
                        XmlNode filter = timingData.CreateElement("FilterInstruction");
                        XmlAttribute var = timingData.CreateAttribute("VarName");
                        var.Value = fi.VarName;
                        filter.Attributes.Append(var);

                        XmlAttribute oper = timingData.CreateAttribute("Operation");
                        oper.Value = fi.Oper.ToString();
                        filter.Attributes.Append(oper);

                        XmlNode responses = timingData.CreateElement("Responses");
                        filter.AppendChild(responses);
                        foreach (string responseValue in fi.ValuesStr)
                        {
                            XmlNode response = timingData.CreateElement("Response");
                            XmlAttribute resp = timingData.CreateAttribute("ResponseValue");
                            resp.Value = responseValue;
                            response.Attributes.Append(resp);
                            responses.AppendChild(response);
                        }

                        scenario.AppendChild(filter);
                    }

                    scenarios.AppendChild(scenario);
                }

                // weight info
                XmlAttribute weight = timingData.CreateAttribute("Weight");
                weight.Value = q.Weight.Value.ToString();
                varname.Attributes.Append(weight);

                XmlAttribute weightSource = timingData.CreateAttribute("WeightSource");
                weightSource.Value = q.Weight.Source;
                varname.Attributes.Append(weightSource);

                questions.AppendChild(varname);
            }

            XmlNode minquestions = timingData.CreateElement("MinQuestions");

            // add all varnames and their filter vars
            foreach (LinkedQuestion q in UserQuestionsMin)
            {
                XmlNode varname = timingData.CreateElement("MinQuestion");

                XmlAttribute name = timingData.CreateAttribute("VarName");
                name.Value = q.VarName.FullVarName;
                varname.Attributes.Append(name);

                minquestions.AppendChild(varname);
            }
            surveyNode.AppendChild(minquestions);

            XmlNode maxquestions = timingData.CreateElement("MaxQuestions");

            // add all varnames and their filter vars
            foreach (LinkedQuestion q in UserQuestionsMax)
            {
                XmlNode varname = timingData.CreateElement("MaxQuestion");

                XmlAttribute name = timingData.CreateAttribute("VarName");
                name.Value = q.VarName.FullVarName;
                varname.Attributes.Append(name);

                maxquestions.AppendChild(varname);
            }
            surveyNode.AppendChild(maxquestions);

            return timingData.InnerXml;
        }

        public void TimeUsers (List<Respondent> respondents)
        {
            //UserTypes = respondents;
            foreach (Respondent r in respondents)
            {
                Respondent rMax = GetMaxUser(r);
                Respondent rMin = GetMinUser(r);

                
                UserQuestionsMax = GetRespondentQuestions(rMax);
                UserQuestionsMin = GetRespondentQuestions(rMin);

                r.TotalMaxTime = Math.Round( GetUserBasedTiming(WPM, TimingType.Max), 2);
                r.TotalMinTime = Math.Round( GetUserBasedTiming(WPM, TimingType.Min), 2);
            }
        }

        public void SetUser(Respondent r)
        {
            User = r;
            int qnum = 0;
            foreach (Answer a in r.Responses)
            {
                var q = Questions.First(x => x.VarName.RefVarName.Equals(a.VarName));

                if (q.GetQnumValue() > StartQ)
                    StartQ = q.GetQnumValue();
            }
            StartQ = 0;
            //foreach(LinkedQuestion q in Questions)
            //{
            //    if (q.GetQnumValue() < qnum)
            //        QuestionsBeforeDef.Add(q);
            //}


            MaxUser = GetMaxUser(r);
            MinUser = GetMinUser(r);

            UserQuestions = GetRespondentQuestions();
            UserQuestionsMax = GetRespondentQuestions(MaxUser);
            UserQuestionsMin = GetRespondentQuestions(MinUser);

            User.TotalMaxTime = Math.Round(GetUserBasedTiming(WPM, TimingType.Max), 2);
            User.TotalMinTime = Math.Round(GetUserBasedTiming(WPM, TimingType.Min), 2);
        }

        public double GetUserBasedTiming(int wpm, TimingType path)
        {
            double total = 0;
            List<LinkedQuestion> list;

            if (path == TimingType.Max)
                list = UserQuestionsMax;
            else if (path == TimingType.Min)
                list = UserQuestionsMin;
            else
                list = UserQuestions;

            foreach (LinkedQuestion q in list)
            {
                if (q.GetQnumValue() >= StartQ && IsForTiming(q))
                {
                    total += q.GetTiming(wpm, false, IncludeNotes);
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
        public double GetTargetWPMUserBased(double targetTime, TimingType path)
        {
            double targetWPM = 0;
            double totalWords = 0;

            List<LinkedQuestion> list;

            if (path == TimingType.Max)
                list = UserQuestionsMax;
            else if (path == TimingType.Min)
                list = UserQuestionsMin;
            else
                list = UserQuestions;

            foreach (LinkedQuestion q in list)
            {
                if (q.GetQnumValue() >= StartQ && IsForTiming(q))
                {

                    totalWords += q.WordCount();
                }
            }

            targetWPM = (double)(totalWords / (targetTime * 60)) * 60;

            return targetWPM;
        }

        public int TotalWordCount(TimingType path)
        {
            int totalWords = 0;

            List<LinkedQuestion> list;

            if (path == TimingType.Max)
                list = UserQuestionsMax;
            else if (path == TimingType.Min)
                list = UserQuestionsMin;
            else
                list = UserQuestions;

            foreach (LinkedQuestion q in list)
            {
                if (q.GetQnumValue() >= StartQ && IsForTiming(q))
                {

                    totalWords += q.WordCount(false);
                }
            }

            return totalWords;
        }

        public double MeanWordCount()
        {
            double total = 0;

            total = (TotalWordCount(TimingType.Max) + TotalWordCount(TimingType.Min)) / 2;

            return total;
        }

        public List<LinkedQuestion> GetRespondentQuestions(Respondent r)
        {
            List<LinkedQuestion> set = new List<LinkedQuestion>();

            foreach (LinkedQuestion q in Questions)
            {
                if (UserGetsQuestion(r, q) && (q.GetQnumValue() > StartQ || q.Weight.Value ==1 || q.Weight.Value ==0))
                {
                    if (!set.Contains(q)) set.Add(q);
                }
            }
            return set;
        }

        public List<LinkedQuestion> GetRespondentQuestions()
        {
            return GetRespondentQuestions(User);
        }

        /// <summary>
        /// Returns true if the provided user should be asked the specified question. The user gets the question if their answers satisfy at least one of the question's 
        /// filter scenarios, or if there are no filter scenarios (an Ask all).
        /// </summary>
        /// <param name="r"></param>
        /// <param name="lq"></param>
        /// <returns></returns>
        public bool UserGetsQuestion(Respondent r, LinkedQuestion lq)
        {
            // if there are no filter conditions, it is ask all, so any user gets this question
            if (lq.FilterList.Count == 0)
                return true;

            if (lq.FilteredOnRtypeOnly())
                return true;

            // if the VarName was skipped, the user does not get this question
                if (r.Responses.Any(x => x.VarName.Equals(lq.VarName.RefVarName) && x.Skipped))
                return false;

            // if the respondent has an answer for this question, include it
            if (r.Responses.Any(x => x.VarName.Equals(lq.VarName.RefVarName) && !x.Skipped))
                return true;

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

                if (lq.GetQnumValue() <= StartQ)
                    continue;

                // skip ones that already have an answer
                if (result.Responses.Any(x => x.VarName.Equals(lq.VarName.RefVarName)))
                    continue;

                // skip ones that this user doesn't answer
                if (!UserGetsQuestion(result, lq))
                {
                    result.AddSkip(lq.VarName.RefVarName);
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

                if (directFilterCount + indirectFilterCount == 0)
                    continue;

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

                // if maxResp = blank then no answer is used as a filter elsewhere so we can answer with anything 
                if (string.IsNullOrEmpty(maxResp))
                    maxResp = "0";
                
                result.AddResponse(lq.VarName.RefVarName, maxResp);
                
                
            }

            return result;
        }

        public Respondent GetMinUser(Respondent r)
        {
            Respondent result = new Respondent(r, false);

            foreach (LinkedQuestion lq in Questions)
            {

                if (lq.GetQnumValue() <= StartQ)
                    continue;

                // skip ones that already have an answer
                if (result.Responses.Any(x => x.VarName.Equals(lq.VarName.RefVarName)))
                    continue;


                // skip ones that this user doesn't answer
                if (!UserGetsQuestion(result, lq))
                {
                    result.AddSkip(lq.VarName.RefVarName);
                    continue;
                }

                int directFilterCount;
                int indirectFilterCount;

                List<LinkedQuestion> directFilterList = GetDirectFilterVarList(lq);
                directFilterCount = directFilterList.Count();

                List<LinkedQuestion> indirectFilterList = new List<LinkedQuestion>();
                try
                {
                    if (lq.GetQnumValue() >= StartQ)
                    {

                        foreach (LinkedQuestion q in directFilterList)
                        {
                            indirectFilterList.AddRange(GetIndirectFilterVarList(q));
                        }
                    }
                }catch(Exception e)
                {

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

                // if minResp = blank then no answer is used as a filter elsewhere so we can answer with anything 
                if (string.IsNullOrEmpty(minResp))
                    minResp = "0";

                result.AddResponse(lq.VarName.RefVarName, minResp);
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

                }
            }

            return hasFilter;
        }


        public List<LinkedQuestion> GetMaxQuestions()
        {
            return GetRespondentQuestions(MaxUser);
        }

        public List<LinkedQuestion> GetMinQuestions()
        {
            return GetRespondentQuestions(MinUser);
        }

        public bool IsForTiming(SurveyQuestion q)
        {
            return !q.IsDerived() && !q.IsProgramming() && !q.IsTermination() && !q.VarName.RefVarName.Equals("BI104");
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

        #region Report Methods
        private string GetDirectFilterListColumn(LinkedQuestion lq, List<LinkedQuestion> directFilterList)
        {
            string filterExpList = "";
            // for each response in this lq, create a filter instruction and get the list of varnames that fall under it
            var resps = lq.GetRespNumbers();
            foreach (string response in resps)
            {
                Dictionary<string, string> filterExpressions = new Dictionary<string, string>();

                FilterInstruction fi = new FilterInstruction();
                fi.VarName = lq.VarName.RefVarName;
                fi.Oper = Operation.Equals;
                fi.ValuesStr.Add(response);
                fi.FilterExpression = fi.VarName + "=" + fi.ValuesStr[0];

                // for each direct varname, get its filter in regard to this varname
                foreach (LinkedQuestion dlq in directFilterList)
                {
                    string filterExp = dlq.GetFilterExpression(fi);

                    filterExpressions.Add(dlq.VarName.RefVarName + "(" + dlq.Weight.Value + " - " + dlq.Weight.Source + ")", filterExp);

                }

                // 
                foreach (string f in filterExpressions.Values)
                {
                    if (filterExpList.Contains(f))
                        continue;

                    filterExpList += "[lblue]<strong>" + f + "</strong>[/lblue]\r\n";
                    foreach (string k in filterExpressions.Keys)
                    {
                        if (filterExpressions[k].Equals(f))
                        {
                            filterExpList += k + "\r\n";
                        }
                    }

                    filterExpList += "\r\n";
                }
            }
            return filterExpList;
        }



        private string UsedAsFilterColumn(LinkedQuestion lq, List<LinkedQuestion> directFilterList, List<LinkedQuestion> indirectFilterList, FilterInstruction fi, bool includeIndirect = false)
        {

            string filterList = "";

            string direct = "";
            string indirect = "";

            List<LinkedQuestion> directList = GetDirectFilterConditionList(fi);
            int directCount = directList.Count();
            int indirectCount = 0;

            if (includeIndirect)
            {
                List<LinkedQuestion> indirectList = new List<LinkedQuestion>();
                foreach (LinkedQuestion q in indirectFilterList)
                {
                    if (QuestionHasIndirectFilter(q, fi))
                        indirectList.Add(q);

                }

                if (indirectList.Count() > 0)
                {
                    indirect = "<u><strong>Indirect:</strong></u>\r\n";

                    foreach (LinkedQuestion s in indirectList)
                    {
                        indirect += "<strong>" + s.VarName.RefVarName + "</strong> - " + s.VarName.VarLabel + "\r\n";
                    }
                }

                indirectCount = indirectList.Count();
            }

            if (directList.Count() > 0)
            {
                direct = "<u><strong>Direct:</strong></u>\r\n";

                foreach (LinkedQuestion s in directList)
                {
                    direct += "<strong>" + s.VarName.RefVarName + "</strong> - " + s.VarName.VarLabel + "\r\n";
                }
            }

            direct = Utilities.TrimString(direct, "\r\n");
            indirect = Utilities.TrimString(indirect, "\r\n");
            if (!string.IsNullOrEmpty(indirect))
                indirect += "\r\n\r\n";

            filterList += "<strong>" + fi.ToString() + ":</strong> " + directCount + " (direct), " + indirectCount + " (indirect)" +
                        "\r\n" + direct + "\r\n\r\n" +
                        indirect;

            filterList = Utilities.TrimString(filterList, "\r\n");
            return filterList;
        }

        private string UsedAsFilterColumn(LinkedQuestion lq, List<LinkedQuestion> directFilterList, List<LinkedQuestion> indirectFilterList, bool includeIndirect = false)
        {

            string filterList = "";

            List<FilterInstruction> responses = lq.GetFiltersByResponse();
            // get list of responses
            // for each response get direct count, get indirect count

            foreach (FilterInstruction fi in responses)
            {
                List<LinkedQuestion> directList = GetDirectFilterConditionList(fi);
                int directCount = directList.Count();

                string direct = "";
                string indirect = "";
                int indirectCount = 0;
                if (directList.Count() > 0)
                {
                    direct = "<u><strong>Direct:</strong></u>\r\n";

                    foreach (LinkedQuestion s in directList)
                    {
                        direct += "<strong>" + s.VarName.RefVarName + "</strong> - " + s.VarName.VarLabel + "\r\n";
                    }
                }

                if (includeIndirect)
                {
                    List<LinkedQuestion> indirectList = new List<LinkedQuestion>();
                    foreach (LinkedQuestion q in indirectFilterList)
                    {
                        if (QuestionHasIndirectFilter(q, fi))
                            indirectList.Add(q);

                    }

                    if (indirectList.Count() > 0)
                    {
                        indirect = "<u><strong>Indirect:</strong></u>\r\n";

                        foreach (LinkedQuestion s in indirectList)
                        {
                            indirect += "<strong>" + s.VarName.RefVarName + "</strong> - " + s.VarName.VarLabel + "\r\n";
                        }

                        indirect = Utilities.TrimString(indirect, "\r\n");
                    }
                    indirectCount = indirectList.Count();
                }

                direct = Utilities.TrimString(direct, "\r\n");

                if (!string.IsNullOrEmpty(indirect))
                    indirect += "\r\n\r\n";

                if (directCount == 0 && indirectCount == 0)
                    continue;

                filterList += "<strong>" + fi.ToString() + ":</strong> " + directCount + " (direct), " + indirectCount + " (indirect)" +
                            "\r\n" + direct + "\r\n\r\n" +
                            indirect;
            }

            filterList = Utilities.TrimString(filterList, "\r\n");
            return filterList;
        }

        #endregion  
    }
}
