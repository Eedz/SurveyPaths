using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITCLib;
using System.Xml;

namespace SurveyPaths
{

    public enum TimingReportType { Undefined, TimingUser, TimingWholeSurvey }

    public class SurveyTiming : Timing
    {
        
        // default constructor
        public SurveyTiming()
        {
            Title = "New Timing Run";
            SurveyCode = "";
            
            Questions = new List<LinkedQuestion>();
            NotInterpreted = new List<LinkedQuestion>();

            WPM = 150;
            TotalTime = 0;
            TotalWeightedTime = 0;
            SmartWordCount = false;
            
            StartQ = 0;
        }

        public SurveyTiming(string xmlDoc)
        {

            XmlDocument runData = new XmlDocument();

            runData.LoadXml(xmlDoc);
            SurveyCode = runData.SelectSingleNode("/SurveyTiming").Attributes["Survey"].InnerText;
            Title = runData.SelectSingleNode("/SurveyTiming").Attributes["RunTitle"].InnerText;
            TotalWeightedTime = Double.Parse(runData.SelectSingleNode("/SurveyTiming/Time").Attributes["Mins"].InnerText);
            WPM = Int32.Parse(runData.SelectSingleNode("/SurveyTiming/Time").Attributes["WPM"].InnerText);
            StartQ = Int32.Parse(runData.SelectSingleNode("/SurveyTiming/Time").Attributes["StartQ"].InnerText);
            Notes = runData.SelectSingleNode("/SurveyTiming").Attributes["Notes"].InnerText;

            foreach (XmlNode q in runData.SelectNodes("/SurveyTiming/Questions/Question"))
            {
                LinkedQuestion lq = new LinkedQuestion();

                lq.VarName.VarName = q.Attributes["VarName"].InnerText;
                lq.VarName.RefVarName = q.Attributes["refVarName"].InnerText;
                lq.VarName.VarLabel = q.Attributes["VarLabel"].InnerText;
                lq.Qnum = q.Attributes["Qnum"].InnerText;

                lq.PreP = q.Attributes["PreP"].InnerText;
                lq.PreI = q.Attributes["PreI"].InnerText;
                lq.PreA = q.Attributes["PreA"].InnerText;
                lq.LitQ = q.Attributes["LitQ"].InnerText;
                lq.PstI = q.Attributes["PstI"].InnerText;
                lq.PstP = q.Attributes["PstP"].InnerText;
                try
                {
                    lq.RespName = q.Attributes["RespName"].InnerText;
                }
                catch (Exception)
                {
                    lq.RespName = "0";
                }
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
            scheme.Value = "WholeSurvey";
            surveyNode.Attributes.Append(scheme);

            XmlAttribute notes = timingData.CreateAttribute("Notes");
            notes.Value = Notes;
            surveyNode.Attributes.Append(notes);

            XmlNode timing = timingData.CreateElement("Time");
            XmlAttribute time = timingData.CreateAttribute("Mins");
            time.Value = TotalWeightedTime.ToString();
            timing.Attributes.Append(time);
            XmlAttribute wpm = timingData.CreateAttribute("WPM");
            wpm.Value = WPM.ToString();
            timing.Attributes.Append(wpm);
            XmlAttribute startQ = timingData.CreateAttribute("StartQ");
            startQ.Value = StartQ.ToString();
            timing.Attributes.Append(startQ);


            surveyNode.AppendChild(timing);

            XmlNode questions = timingData.CreateElement("Questions");
            surveyNode.AppendChild(questions);

            // add all varnames and their filter vars
            foreach (LinkedQuestion q in Questions)
            {
                XmlNode varname = timingData.CreateElement("Question");

                XmlAttribute name = timingData.CreateAttribute("VarName");
                name.Value = q.VarName.VarName;
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

            return timingData.InnerXml;
        }

        /// <summary>
        /// Return the total time it would take to read the questions at a specific WPM.
        /// </summary>
        /// <param name="wpm"></param>
        /// <returns>Total time in seconds</returns>
        public double GetWholeSurveyTiming(int wpm)
        {
            double total = 0;
           
            foreach (LinkedQuestion q in Questions)
            {
                if (q.GetQnumValue() >= StartQ) 
                {
                    total += q.GetTiming(wpm, SmartWordCount, IncludeNotes);
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
        public double GetWeightedWholeSurveyTiming(int wpm)
        {
            double total = 0;
            
            foreach (LinkedQuestion q in Questions)
            {
                if (q.GetQnumValue() >= StartQ)
                {
                    if (q.Weight.Value < 0)
                        total += 0;
                    else 
                        total += q.GetTiming(wpm, SmartWordCount, IncludeNotes) * q.Weight.Value;
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

            foreach (LinkedQuestion q in Questions)
            {
                if (q.GetQnumValue() >= StartQ)
                {
                    if (q.Weight.Value < 0)
                        totalWords += 0;
                    else
                        totalWords += q.WordCount(SmartWordCount, IncludeNotes) * q.Weight.Value;
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

        public bool IsForTiming(SurveyQuestion q)
        {
            return !q.IsDerived() && !q.IsProgramming() && !q.IsTermination() && !q.VarName.RefVarName.Equals("BI104");
        }

        

        public List<LinkedQuestion> old_GetTimingQuestions()
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

        

        
    }
}
