using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ITCLib;
using ITCReportLib;
using System.IO;
using System.Xml;

namespace SurveyPaths
{
    public partial class SavedRunList : Form
    {
        public string TimingFolder = "\\\\psychfile\\psych$\\psych-lab-gfong\\SMG\\SDI\\Survey Timing";

        List<Timing> SavedRuns;

        public SavedRunList()
        {
            InitializeComponent();

            cboSurvey.DataSource = DBAction.GetAllSurveysInfo();
            cboSurvey.SelectedItem = null;
            cboSurvey.SelectedIndexChanged += cboSurvey_SelectedIndexChanged;

            SavedRuns = new List<Timing>();

        }

        private void cboSurvey_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)cboSurvey.SelectedItem))
                return;

            string surveyCode = (string)cboSurvey.SelectedItem;
            string savedRunFolder;
            listView1.Items.Clear();
            savedRunFolder = GetFilePath(surveyCode) + "\\Method 2";
            GetSavedMethod2Runs(savedRunFolder);
            savedRunFolder = GetFilePath(surveyCode) + "\\Method 3\\Saved Timing Runs";
            GetSavedRuns(savedRunFolder);

        }

        private void GetSavedRuns (string savedRunFolder)
        {
            
            string[] files;

            if (Directory.Exists(savedRunFolder))
            {
                files = Directory.GetFiles(savedRunFolder, "*.xml");
            }
            else
            {
                MessageBox.Show("Directory " + savedRunFolder + " not found.");
                return;
            }

            foreach(string f in files)
            {
                Timing run = new Timing();
                XmlDocument runData = new XmlDocument();

                ListViewItem lvi = new ListViewItem();
                listView1.View = View.Details;
                runData.LoadXml(File.ReadAllText(f));
                string scheme = runData.SelectSingleNode("/SurveyTiming").Attributes["TimingScheme"].InnerText;

                if (scheme.Equals("WholeSurvey"))
                {
                    SurveyTiming timing = new SurveyTiming(File.ReadAllText(f));
                    SavedRuns.Add(timing);
                    lvi = new ListViewItem(new string[] { timing.Title, "Method 3" });
                }
                else
                {
                    continue;
                }

                listView1.Items.Add(lvi);

            }
        }

        private void GetSavedMethod2Runs(string savedRunFolder)
        {
            string[] folders;
            if (Directory.Exists(savedRunFolder))
            {
                folders = Directory.GetDirectories(savedRunFolder);
            }
            else
            {
                MessageBox.Show("Directory " + savedRunFolder + " not found.");
                return;
            }

            foreach(string folder in folders)
            {
                ListViewItem lvi = new ListViewItem(new string[] { folder.Substring(folder.LastIndexOf("\\") + 1), "Method 2" });
                listView1.Items.Add(lvi);
            }

        }


        

        private void CompareTimingRuns(SurveyTiming time1, SurveyTiming time2)
        {

            DataTable dt = new DataTable();

            string description1 = time1.Title;
            string description2 = time2.Title;

            dt.Columns.Add(new DataColumn(description1 + " VarName"));
            dt.Columns.Add(new DataColumn(description1 + " Word Count"));
            dt.Columns.Add(new DataColumn(description1 + " Weight"));
            dt.Columns.Add(new DataColumn(description1 + " Weight Source"));

            dt.Columns.Add(new DataColumn(description2 + " VarName"));
            dt.Columns.Add(new DataColumn(description2 + " Word Count"));
            dt.Columns.Add(new DataColumn(description2 + " Weight"));
            dt.Columns.Add(new DataColumn(description2 + " Weight Source"));

            

            foreach (LinkedQuestion q in time1.Questions)
            {
                
                DataRow r = dt.NewRow();
                int words1 = q.WordCount(time1.IncludeNotes);
                r[description1 + " VarName"] = q.VarName.RefVarName;
                r[description1 + " Word Count"] = words1;
                r[description1 + " Weight"] = q.Weight.Value;
                r[description1 + " Weight Source"] = q.Weight.Source;

                var match = time2.Questions.FirstOrDefault(x => x.VarName.RefVarName.Equals(q.VarName.RefVarName));
                if (match != null)
                {
                    r[description2 + " VarName"] = match.VarName.RefVarName;
                    int words2 = match.WordCount(time2.IncludeNotes);
                    if (words2 != words1)
                        r[description2 + " Word Count"] = "[brightgreen]" + words2 + "[/brightgreen]";
                    else
                        r[description2 + " Word Count"] = words2;

                    if (match.Weight.Value != q.Weight.Value)
                        r[description2 + " Weight"] = "[brightgreen]" + match.Weight.Value + "[/brightgreen]";
                    else 
                        r[description2 + " Weight"] = match.Weight.Value;

                    if (match.Weight.Source != q.Weight.Source)
                        r[description2 + " Weight Source"] = "[brightgreen]" + match.Weight.Source + "[/brightgreen]";
                    else
                        r[description2 + " Weight Source"] = match.Weight.Source;
                }

                dt.Rows.Add(r);
                
            }

            List<LinkedQuestion> time2Only = time2.Questions.Except(time1.Questions, new LinkedQuestionComparer()).ToList();

            foreach (LinkedQuestion q in time2Only)
            {
                DataRow r = dt.NewRow();
                r[description2 + " VarName"] = q.VarName.RefVarName;
                r[description2 + " Word Count"] = q.WordCount(time2.IncludeNotes);
                r[description2 + " Weight"] = q.Weight.Value;
                r[description2 + " Weight Source"] = q.Weight.Source;
                dt.Rows.Add(r);
            }


            OutputReport(dt, time1.Title + " vs. " + time2.Title);

            //// GET ADDED QS
            //List<LinkedQuestion> time1Only = time1.Questions.Except(time2.Questions).ToList();

            //double time1Time = time1Only.Sum(x => x.Weight.Value * x.GetTiming(time1.WPM, time1.IncludeNotes));

            //// GET REMOVED QS
            //List<LinkedQuestion> time2Only = time2.Questions.Except(time1.Questions).ToList();

            //double time2Time = time2Only.Sum(x => x.Weight.Value * x.GetTiming(time1.WPM, time1.IncludeNotes));

            //// for commons compare wording counts
            //List<LinkedQuestion> commonTime1 = time1.Questions.Intersect(time2.Questions).ToList();
            //List<LinkedQuestion> commonTime2 = time2.Questions.Intersect(time1.Questions).ToList();

            //foreach (LinkedQuestion q in commonTime1)
            //{

            //}

            //lblResults.Text = time1Only.Count() + " questions appear only in " + time1.Title + " totalling " + time1Time + " mins";
            //lblResults.Text += "\r\n" + time2Only.Count() + " questions appear only in " + time2.Title + " totalling " + time2Time + " mins";
        }

        private void OutputReport(DataTable dt, string customFileName)
        {
            SurveyReport SR = new SurveyReport();
            SR.Surveys.Add(new ReportSurvey("Survey Timing Method 3 Comparison"));
            SR.FileName = "\\\\psychfile\\psych$\\psych-lab-gfong\\SMG\\SDI\\Reports\\ISR\\";

            SR.ReportTable = dt;
            SR.OutputReportTableXML(customFileName);
        }

        private string GetFilePath(string surveyCode)
        {
            return TimingFolder + "\\" + surveyCode;
        }

        private void cmdAddTiming_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            if (lstToCompare.Items.Count >= 2)
                return;

            int index = listView1.SelectedItems[0].Index;

            lstToCompare.Items.Add(SavedRuns[index]);
        }

        private void cmdRevoveTiming_Click(object sender, EventArgs e)
        {
            if (lstToCompare.SelectedItem == null)
                return;

            lstToCompare.Items.Remove(lstToCompare.SelectedItem);
        }

        private void cmdCompare_Click(object sender, EventArgs e)
        {
            if (lstToCompare.Items.Count != 2)
                return;
            object time1 = lstToCompare.Items[0];
            object time2 = lstToCompare.Items[1];

            if (time1 is SurveyTiming && time2 is SurveyTiming)
                CompareTimingRuns(time1 as SurveyTiming, time2 as SurveyTiming);
            else if (time1 is string && time2 is string)
            {
                // get list of user timings from selected folders
                List<UserTiming> timing1 = new List<UserTiming>();
                List<UserTiming> timing2 = new List<UserTiming>();

                var files = Directory.GetFiles(time1 as string, "*.xml");

                foreach (string f in files)
                {
                    timing1.Add(new UserTiming(File.ReadAllText(f)));
                }

                var files2 = Directory.GetFiles(time2 as string, "*.xml");

                foreach (string f in files2)
                {
                    timing2.Add(new UserTiming(File.ReadAllText(f)));
                }

                foreach (UserTiming ut in timing1)
                {
                    foreach (UserTiming ut2 in timing2) {
                        if (ut.User.Description.Equals(ut2.User.Description))
                        {
                            CompareTimingRuns(ut, ut2);
                            break;
                        }
                    }
                }
                // compare
                
            }

        }


        private void CompareTimingRuns(UserTiming time1, UserTiming time2)
        {

            
            // compare the vars and word counts

            


            DataTable dt = new DataTable();

            string description1 = time1.User.Description;
            string description2 = time2.User.Description;

            dt.Columns.Add(new DataColumn(description1 + " VarName"));
            dt.Columns.Add(new DataColumn(description1 + " Word Count"));
            dt.Columns.Add(new DataColumn(description1 + " Min/Max"));

            dt.Columns.Add(new DataColumn(description2 + " VarName"));
            dt.Columns.Add(new DataColumn(description2 + " Word Count"));
            dt.Columns.Add(new DataColumn(description1 + " Min/Max"));



            foreach (LinkedQuestion q in time1.UserQuestions)
            {

                DataRow r = dt.NewRow();
                int words1 = q.WordCount(time1.SmartWordCount, time1.IncludeNotes);
                r[description1 + " VarName"] = q.VarName.RefVarName;
                r[description1 + " Word Count"] = words1;
                string path1 = time1.WhichPath(q);
                r[description1 + " Min/Max"] = path1;

                var match = time2.Questions.FirstOrDefault(x => x.VarName.RefVarName.Equals(q.VarName.RefVarName));
                if (match != null)
                {
                    r[description2 + " VarName"] = match.VarName.RefVarName;
                    int words2 = match.WordCount(time2.SmartWordCount, time2.IncludeNotes);
                    if (words2 != words1)
                        r[description2 + " Word Count"] = "[brightgreen]" + words2 + "[/brightgreen]";
                    else
                        r[description2 + " Word Count"] = words2;

                    string path2 = time2.WhichPath(q);
                    if (path2 != path1)
                        r[description2 + " Min/Max"] = "[brightgreen]" + path2 + "[/brightgreen]";
                    else
                        r[description2 + " Min/Max"] = path2;
                }

                dt.Rows.Add(r);

            }

            List<LinkedQuestion> time2Only = time2.Questions.Except(time1.Questions, new LinkedQuestionComparer()).ToList();

            foreach (LinkedQuestion q in time2Only)
            {
                DataRow r = dt.NewRow();
                r[description2 + " VarName"] = q.VarName.RefVarName;
                r[description2 + " Word Count"] = q.WordCount(time2.SmartWordCount, time2.IncludeNotes);
                r[description2 + " Min/Max"] = time2.WhichPath(q);
                dt.Rows.Add(r);
            }

            OutputReport(dt, time1.Title + "(" + time1.User.Description + ")" + " vs. " + time2.Title + "(" + time2.User.Description + ")");

        }

        private void comparisonToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //List<LinkedQuestion> tripleList = CurrentTiming.GetRespondentQuestions(usertype1);

        

            //List<LinkedQuestion> cigHTPList = CurrentTiming.GetRespondentQuestions(usertype2);




            // get questions for both



            //ReportSurvey survey1 = new ReportSurvey(usertype1.Description);

            //foreach (LinkedQuestion q in tripleList)
            //{
            //    q.VarName.VarName = Utilities.RemoveHighlightTags(q.VarName.VarName);

            //    SurveyQuestion newQ = new SurveyQuestion();

            //    newQ.SurveyCode = q.SurveyCode;
            //    newQ.VarName.VarName = q.VarName.VarName;
            //    newQ.VarName.RefVarName = q.VarName.RefVarName;
            //    newQ.Qnum = q.Qnum;
            //    newQ.PreP = q.PreP;
            //    newQ.PreI = q.PreI;
            //    newQ.PreA = q.PreA;
            //    newQ.LitQ = q.LitQ;
            //    newQ.PstI = q.PstI;
            //    newQ.PstP = q.PstP;
            //    newQ.RespName = q.RespName;
            //    newQ.RespOptions = q.RespOptions;
            //    newQ.NRName = q.NRName;
            //    newQ.NRCodes = q.NRCodes;

            //    survey1.Questions.Add(newQ);
            //}

            //survey1.VarLabelCol = true;


            //ReportSurvey survey2 = new ReportSurvey(usertype2.Description);
            //foreach (LinkedQuestion q in cigHTPList)
            //{

            //    q.VarName.VarName = Utilities.RemoveHighlightTags(q.VarName.VarName);

            //    SurveyQuestion newQ = new SurveyQuestion();

            //    newQ.SurveyCode = q.SurveyCode;
            //    newQ.VarName.VarName = q.VarName.VarName;
            //    newQ.VarName.RefVarName = q.VarName.RefVarName;
            //    newQ.Qnum = q.Qnum;
            //    newQ.PreP = q.PreP;
            //    newQ.PreI = q.PreI;
            //    newQ.PreA = q.PreA;
            //    newQ.LitQ = q.LitQ;
            //    newQ.PstI = q.PstI;
            //    newQ.PstP = q.PstP;
            //    newQ.RespName = q.RespName;
            //    newQ.RespOptions = q.RespOptions;
            //    newQ.NRName = q.NRName;
            //    newQ.NRCodes = q.NRCodes;

            //    survey2.Questions.Add(newQ);

            //}



            //survey2.VarLabelCol = true;

            //DoCompareReport(survey1, survey2, usertype1.Description.Replace("/", "-") + " vs. " + usertype2.Description.Replace("/", "-") + " Comparison", true);

            //ReportSurvey survey1skipped = new ReportSurvey(usertype1.Description + " skipped");
            //ReportSurvey survey2skipped = new ReportSurvey(usertype2.Description + " skipped");

            //foreach (Answer a in usertype1.Responses)
            //{
            //    if (!a.Skipped)
            //        continue;

            //    var skipped = CurrentTiming.Questions.FirstOrDefault(x => x.VarName.RefVarName.Equals(a.VarName));

            //    if (skipped != null)
            //    {
            //        SurveyQuestion newQ = new SurveyQuestion();

            //        newQ.SurveyCode = skipped.SurveyCode;
            //        newQ.VarName.VarName = skipped.VarName.VarName;
            //        newQ.VarName.RefVarName = skipped.VarName.RefVarName;
            //        newQ.Qnum = skipped.Qnum;
            //        newQ.PreP = skipped.PreP;
            //        newQ.PreI = skipped.PreI;
            //        newQ.PreA = skipped.PreA;
            //        newQ.LitQ = skipped.LitQ;
            //        newQ.PstI = skipped.PstI;
            //        newQ.PstP = skipped.PstP;
            //        newQ.RespName = skipped.RespName;
            //        newQ.RespOptions = skipped.RespOptions;
            //        newQ.NRName = skipped.NRName;
            //        newQ.NRCodes = skipped.NRCodes;

            //        survey1skipped.Questions.Add(newQ);
            //    }

            //}

            //foreach (Answer a in usertype2.Responses)
            //{
            //    if (!a.Skipped)
            //        continue;

            //    var skipped = CurrentTiming.Questions.FirstOrDefault(x => x.VarName.RefVarName.Equals(a.VarName));

            //    if (skipped != null)
            //    {
            //        SurveyQuestion newQ = new SurveyQuestion();

            //        newQ.SurveyCode = skipped.SurveyCode;
            //        newQ.VarName.VarName = skipped.VarName.VarName;
            //        newQ.VarName.RefVarName = skipped.VarName.RefVarName;
            //        newQ.Qnum = skipped.Qnum;
            //        newQ.PreP = skipped.PreP;
            //        newQ.PreI = skipped.PreI;
            //        newQ.PreA = skipped.PreA;
            //        newQ.LitQ = skipped.LitQ;
            //        newQ.PstI = skipped.PstI;
            //        newQ.PstP = skipped.PstP;
            //        newQ.RespName = skipped.RespName;
            //        newQ.RespOptions = skipped.RespOptions;
            //        newQ.NRName = skipped.NRName;
            //        newQ.NRCodes = skipped.NRCodes;

            //        survey2skipped.Questions.Add(newQ);
            //    }

            //}

            //DoCompareReport(survey1skipped, survey2skipped, usertype1.Description.Replace("/", "-") + " vs. " + usertype2.Description.Replace("/", "-") + " Comparison (skipped)", false);
        }

        private void DoCompareReport(ReportSurvey s1, ReportSurvey s2, string filename, bool hideIdentical)
        {

            SurveyReport SR = new SurveyReport();

            SR.FileName = "\\\\psychfile\\psych$\\psych-lab-gfong\\SMG\\SDI\\Reports\\ISR\\";
            string customFileName = filename;

            SR.AddSurvey(s1);
            SR.AddSurvey(s2);
            SR.ShowQuestion = false;
            SR.SurveyCompare.HideIdenticalQuestions = hideIdentical;
            SR.SurveyCompare.ReInsertDeletions = true;
            SR.GenerateReport();
            SR.OutputReportTableXML(customFileName);
        }

        
    }
}

