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
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using OpenXMLHelper;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace SurveyPaths
{    

    public partial class UserTimingForm : Form
    {
        UserTiming CurrentTiming;
        List<UserTiming> UserTimings;

        BindingSource bs;
        BindingSource bsRun;

        List<Respondent> RespondentList;

        bool loading = false; // true if we are loading a past run

        List<string> CustomList;
        public string TimingFolder = "\\\\psychfile\\psych$\\psych-lab-gfong\\SMG\\SDI\\Survey Timing";
        public bool ExcludeCustomList;
        public bool OnlyCustomList;

        public UserTimingForm(string survey, List<Respondent> userTypes)
        {
            InitializeComponent();

            bs = new BindingSource();
            bsRun = new BindingSource();
            bindingNavigator1.BindingSource = bs;

            CustomList = new List<string>();
            DoCustomList();

            RespondentList = userTypes;

            CurrentTiming = new UserTiming();

            CurrentTiming.SurveyCode = survey;

            UserTimings = new List<UserTiming>();
            ChangeSurvey(survey);

            RefreshCurrentRecord();

            // bindings
            bsRun.DataSource = CurrentTiming;

            cboSurvey.DataSource = DBAction.GetAllSurveysInfo().Select(x => x.SurveyCode).ToList();;
            cboSurvey.SelectedItem = null;
            cboSurvey.DataBindings.Add(new Binding("SelectedItem", bsRun, "SurveyCode"));

            txtTimingTitle.DataBindings.Add("Text", bsRun, "Title", true, DataSourceUpdateMode.OnPropertyChanged);
            txtKnownWPM.DataBindings.Add(new Binding("Text", bsRun, "WPM"));
            txtStartAt.DataBindings.Add(new Binding("Text", bsRun, "StartQ"));
            txtNotes.DataBindings.Add(new Binding("Text", bsRun, "Notes"));

            lstResponses.DataSource = new BindingList<Answer>(CurrentTiming.User.Responses);
            lstMinResponses.DataSource = CurrentTiming.MinUser.Responses;
            lstMaxResponses.DataSource = CurrentTiming.MaxUser.Responses;

            rbKnownWPM.Checked = true;

            txtVarName.DataBindings.Clear();
            txtVarName.DataBindings.Add("Text", bs, "VarName.VarName");

            txtQnum.DataBindings.Clear();
            txtQnum.DataBindings.Add("Text", bs, "Qnum");


            txtUserWeight.DataBindings.Add("Text", bsRun, "User.Weight", true, DataSourceUpdateMode.OnPropertyChanged);

            cboSurvey.SelectedIndexChanged += cboSurvey_SelectedIndexChanged;
            bs.CurrentChanged += Bs_CurrentChanged;
        }

        // TODO finish this
        public UserTimingForm(List<UserTiming> userTimings)
        {
            InitializeComponent();

            bs = new BindingSource();
            bsRun = new BindingSource();
            bindingNavigator1.BindingSource = bs;

            CustomList = new List<string>();
            RespondentList = new List<Respondent>();
            foreach (UserTiming ut in userTimings)
            {
                RespondentList.Add(ut.User);
            }

            CurrentTiming = userTimings[0];

            UserTimings = userTimings;
     
            // bindings
            bsRun.DataSource = CurrentTiming;
            bs.DataSource = CurrentTiming.Questions;

            cboSurvey.DataSource = DBAction.GetAllSurveysInfo().Select(x => x.SurveyCode).ToList();;
            cboSurvey.SelectedItem = null;
            cboSurvey.DataBindings.Add(new Binding("SelectedItem", bsRun, "SurveyCode"));

            txtTimingTitle.DataBindings.Add("Text", bsRun, "Title", true, DataSourceUpdateMode.OnPropertyChanged);
            txtKnownWPM.DataBindings.Add(new Binding("Text", bsRun, "WPM"));
            txtStartAt.DataBindings.Add(new Binding("Text", bsRun, "StartQ"));
            txtNotes.DataBindings.Add(new Binding("Text", bsRun, "Notes"));

            lstResponses.DataSource = new BindingList<Answer>(CurrentTiming.User.Responses);
            lstMinResponses.DataSource = CurrentTiming.MinUser.Responses;
            lstMaxResponses.DataSource = CurrentTiming.MaxUser.Responses;

            rbKnownWPM.Checked = true;

            txtVarName.DataBindings.Clear();
            txtVarName.DataBindings.Add("Text", bs, "VarName.VarName");

            txtQnum.DataBindings.Clear();
            txtQnum.DataBindings.Add("Text", bs, "Qnum");


            txtUserWeight.DataBindings.Add("Text", bsRun, "User.Weight", true, DataSourceUpdateMode.OnPropertyChanged);

            cboSurvey.SelectedIndexChanged += cboSurvey_SelectedIndexChanged;
            bs.CurrentChanged += Bs_CurrentChanged;

            RefreshCurrentRecord();
        }

        private void DoCustomList()
        {
            CustomList = new List<string>();
            CustomList.Add("HN512");
            CustomList.Add("HN513");
            CustomList.Add("HN532");
            CustomList.Add("HN523");
            CustomList.Add("HN514");
            CustomList.Add("HN527");
            CustomList.Add("HN519");

            CustomList.Add("HN400");
            CustomList.Add("HN453");
            CustomList.Add("HN708");
            CustomList.Add("FR326");
            CustomList.Add("FR333");
            CustomList.Add("FR351");

            CustomList.Add("FR353");
            CustomList.Add("FR359");
            CustomList.Add("FR355");
            CustomList.Add("FR332");
            CustomList.Add("BR380");
            CustomList.Add("BR384");
            CustomList.Add("IN601");
            CustomList.Add("AD244");
            CustomList.Add("AD282");
            CustomList.Add("IN350");
            CustomList.Add("DI425");
            CustomList.Add("DI426");
            CustomList.Add("DI423");
            CustomList.Add("DI427");
            CustomList.Add("CV085");
            CustomList.Add("CV086");
            CustomList.Add("CV087");
            CustomList.Add("CV088");
            CustomList.Add("CV089");
            CustomList.Add("CV090");
            CustomList.Add("CV091");
            CustomList.Add("CV092");
            CustomList.Add("CV093");
            CustomList.Add("CV094");
            CustomList.Add("CV095");
            CustomList.Add("CV096");

            CustomList.Add("CV099");
            CustomList.Add("CV099o");
            CustomList.Add("CV057");
            CustomList.Add("CV060");
            CustomList.Add("CV062");
            CustomList.Add("BI322");
            CustomList.Add("BI324");

        }

        public void TimeUsers(List<Respondent> respondents)
        {
            List<UserTiming> timings = new List<UserTiming>();
            
            foreach (Respondent r in respondents)
            {
                UserTiming timing = new UserTiming();
                timing.WPM = CurrentTiming.WPM;
                timing.SetUser(r);

                timings.Add(timing);
            }
            UserTimings = timings;
        }

        private void NewTimingRun()
        {
            CurrentTiming = new UserTiming();

            ChangeSurvey("JP4"); 
             
            RefreshCurrentRecord();
        }

        /// <summary>
        /// Change the current survey. Get a new set of questions.
        /// </summary>
        /// <param name="survey"></param>
        private void ChangeSurvey(string surveyCode)
        {
            // set the current timing's survey and reference survey, set up question lists

            SetSurvey(surveyCode);

            cboUserType.DataSource = UserTimings;
            cboUserType.DisplayMember = "User";

            CurrentTiming = UserTimings[0];

            bs.DataSource = CurrentTiming.Questions;
            bs.ResetBindings(true);
            bsRun.ResetBindings(true);

            UpdateResponses();
            RefreshLists();
            UpdateTiming();
            UpdateVarList();

            CountGateways();
            RefreshCurrentRecord();
        }

        private void SetSurvey(string surveyCode)
        {

            Survey survey = DBAction.GetAllSurveysInfo().Where(x => x.SurveyCode.Equals(surveyCode)).FirstOrDefault();
            var qs = DBAction.GetSurveyQuestions(survey).ToList();

            Survey referenceSurvey;
            double wave = survey.Wave;

            // set reference survey
            if (wave > 1)
            {
                string previousWaveCode = "";
                if (survey.SurveyCode.IndexOf("-") > -1)
                {
                    previousWaveCode = survey.SurveyCodePrefix + (wave - 1) + survey.SurveyCode.Substring(survey.SurveyCode.IndexOf("-"));
                }
                else
                {
                    previousWaveCode = survey.SurveyCodePrefix + (wave - 1);
                }
                referenceSurvey = DBAction.GetAllSurveysInfo().Where(x => x.SurveyCode.Equals(previousWaveCode)).FirstOrDefault();
                if (referenceSurvey != null)
                    referenceSurvey.AddQuestions(DBAction.GetSurveyQuestions(referenceSurvey));
            }
            else
            {
                referenceSurvey = null;
            }

            var users = RespondentList.Where(x => x.Survey.Equals(CurrentTiming.SurveyCode) || x.Survey.Equals("")).ToList();
            // create a user timing for each respondent type
            foreach (Respondent r in users)
            {
                if (UserTimings.Any(x => x.User.Description.Equals(r.Description)))
                    continue;

                UserTiming ut = new UserTiming();
                ut.SurveyCode = surveyCode;
                ut.ReferenceSurvey = referenceSurvey;
                PopulateQuestions(ut, qs);
                ut.PopulateFilters();
                ut.SetUser(r);
                UserTimings.Add(ut);
            }

            
        }

        private void PopulateQuestions(UserTiming ut, List<SurveyQuestion> source)
        {
            ut.Questions.Clear();

            foreach (SurveyQuestion q in source)
            {
                //include only custom list
                if (OnlyCustomList)
                {
                    if (CustomList.Count != 0 && !CustomList.Any(x => x.Equals(q.VarName.RefVarName)))
                        continue;
                }
                else if (ExcludeCustomList)
                {// exclude custom list
                    if (CustomList.Count != 0 && CustomList.Any(x => x.Equals(q.VarName.RefVarName)))
                        continue;
                }

                if (IsOtherSpecify(source, q))
                    continue;

                ut.Questions.Add(new LinkedQuestion(q));
            }
        }

        private void PopulateQuestions(List<SurveyQuestion> source )
        {
            CurrentTiming.Questions.Clear();

            foreach (SurveyQuestion q in source)
            {
                //include only custom list
                if (OnlyCustomList)
                {
                    if (CustomList.Count != 0 && !CustomList.Any(x => x.Equals(q.VarName.RefVarName)))
                        continue;
                }
                else if (ExcludeCustomList)
                {// exclude custom list
                    if (CustomList.Count != 0 && CustomList.Any(x => x.Equals(q.VarName.RefVarName)))
                        continue;
                }

                if (IsOtherSpecify(source, q))
                    continue;

                CurrentTiming.Questions.Add(new LinkedQuestion(q));
            }
        }

        

        /// <summary>
        /// Update the list of responses with those stored in the Respondent .
        /// </summary>
        private void UpdateResponses()
        {
            
            lstResponses.DataSource = CurrentTiming.User.Responses;
            lstMaxResponses.DataSource = CurrentTiming.MaxUser.Responses.Except(CurrentTiming.User.Responses).ToList();
            lstMinResponses.DataSource = CurrentTiming.MinUser.Responses.Except(CurrentTiming.User.Responses).ToList();
            
        }

        /// <summary>
        /// Update the VarName list.
        /// </summary>
        /// <param name="list"></param>
        private void UpdateVarList()
        {
            cboGoToVar.DataSource = null;
            
           
            if (rbAllUserQ.Checked)
                cboGoToVar.DataSource = CurrentTiming.UserQuestions;
            else if (rbMaxUserQs.Checked)
                cboGoToVar.DataSource = CurrentTiming.UserQuestionsMax;
            else if (rbMinUserQs.Checked)
                cboGoToVar.DataSource = CurrentTiming.UserQuestionsMin;

            lstNotInterpreted.DataSource = CurrentTiming.NotInterpreted;
            lstNotInterpreted.Visible = CurrentTiming.NotInterpreted.Count > 0;
            lblUninterpretable.Visible = CurrentTiming.NotInterpreted.Count > 0;
        }

        private void RefreshLists()
        {
            if (CurrentTiming.Questions == null) return;

            lstWeightedQuestionList.Items.Clear();
            lstWeightedQuestionList.View = System.Windows.Forms.View.Details;

            List<LinkedQuestion> list;

            if (rbAllUserQ.Checked)
                list = CurrentTiming.Questions;
            else if (rbMaxUserQs.Checked)
                list = CurrentTiming.UserQuestionsMax;
            else if (rbMinUserQs.Checked)
                list = CurrentTiming.UserQuestionsMin;
            else
                list = CurrentTiming.UserQuestions;

            chUser.Width = 60;
            chWeight.Width = 0;
            chWeightSource.Width = 0;

            double totaltime = 0;
            foreach (LinkedQuestion lq in list)
            {
                string minmax = "";

                if (CurrentTiming.UserQuestionsMax.Contains(lq) && CurrentTiming.UserQuestionsMin.Contains(lq))
                    minmax = "+/-";
                else if (CurrentTiming.UserQuestionsMax.Contains(lq) && !CurrentTiming.UserQuestionsMin.Contains(lq))
                    minmax = "+";
                else if (CurrentTiming.UserQuestionsMin.Contains(lq) && !CurrentTiming.UserQuestionsMax.Contains(lq))
                    minmax = "-";
                else
                    minmax = "N/A";

                double time = 0;
                if (CurrentTiming.IsForTiming(lq))
                {
                    time = lq.GetTiming(CurrentTiming.WPM, false);
                    totaltime += time;
                }
                    
                ListViewItem li = new ListViewItem(new string[] { lq.Qnum, lq.VarName.RefVarName, lq.VarName.VarLabel, lq.Weight.Value.ToString(), lq.Weight.Source, lq.WordCount().ToString(), time.ToString("N2"), minmax });
                li.Tag = lq;

                lstWeightedQuestionList.Items.Add(li);

                FormatListItem(li, GetQuestionType(li));
            }
            totaltime = totaltime / 60;
            lblTotal.Text = "Total Questions: " + list.Count() + "    Total Time: " + totaltime.ToString("N2");
            
            grpMinMaxFilter.Visible = true;

        }

        private void UpdateTiming()
        {

            //List<Respondent> userList = new List<Respondent>();
            //foreach (object o in cboUserType.Items)
            //{
            //    if (((Respondent)o).Description.Equals("Blank Respondent"))
            //        continue;
            //    userList.Add(o as Respondent);
            //}

            //TimeUsers(userList);

            dataGridView1.Rows.Clear();
            double totalWMean = 0;

            foreach (UserTiming ut in UserTimings)
            {

                

                // create a row for this user
                Respondent r = ut.User;

                r.TotalMaxTime = Math.Round(ut.GetUserBasedTiming(CurrentTiming.WPM, TimingType.Max), 2);
                r.TotalMinTime = Math.Round(ut.GetUserBasedTiming(CurrentTiming.WPM, TimingType.Min),2);

                int minWC = ut.TotalWordCount(TimingType.Min);
                int maxWC = ut.TotalWordCount(TimingType.Max);
                double meanWC = (minWC + maxWC) / 2;

                double mean = Math.Round((r.TotalMaxTime + r.TotalMinTime) / 2, 2);

                totalWMean += Math.Round(mean * r.Weight,2);

                string minCol = r.TotalMinTime + " (mins)" + Environment.NewLine + minWC + " (words)";
                string maxCol = r.TotalMaxTime + " (mins)" + Environment.NewLine + maxWC + " (words)";
                string meanCol = mean + " (mins)" + Environment.NewLine + meanWC + " (words)";

                string[] row = new string[] { r.Description, minCol, maxCol, meanCol, r.Weight.ToString(), (mean * r.Weight).ToString() };
                dataGridView1.Rows.Add(row);

            }

            dataGridView1.AutoResizeColumns();

            txtTargetTime.Text = totalWMean.ToString();

            if (!string.IsNullOrEmpty(txtKnownWPM.Text))
            {
                // 
            }
        }


        #region PstP Analysis
        /// <summary>
        /// (QuestionRouting) For each question, get any VarNames appearing in the PstP and then get a reference to that question in the master list and
        /// add this reference to the list of possible next questions.
        /// </summary>
        private void PopulateNextQuestions()
        {

            foreach (LinkedQuestion q in CurrentTiming.Questions)
            {
                var qr = new QuestionRouting(q.PstP, q.RespOptions);

                foreach (RoutingVar rv in qr.RoutingVars)
                {
                    string s = rv.Varname.Substring(0, rv.Varname.IndexOf("."));
                    LinkedQuestion next = CurrentTiming.Questions.Find(x => x.VarName.VarName.Equals(s));

                    foreach (int v in rv.ResponseCodes)
                        q.PossibleNext.Add(v, next);
                }
            }
        }

        /// <summary>
        /// (string) For each question, get any varnames appearing in the PstP and then get a reference to that question in the master list and
        /// add this reference to the list of possible next questions.
        /// </summary>
        private void PopulateNextQuestionsString()
        {
            foreach (LinkedQuestion q in CurrentTiming.Questions)
            {
                int i = 0;
                var routing = q.GetRoutingVars();

                foreach (string s in routing)
                {
                    LinkedQuestion next = CurrentTiming.Questions.Find(x => x.VarName.VarName.Equals(s));

                    q.PossibleNext.Add(i, next);
                    i++;
                }
            }
        }
        #endregion

        

        #region Menu Strip
        // Save/Load

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewTimingRun();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveTimingRun();
            
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadTimingRun();
            
        }

        // View
        private void filterTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LinkedQuestion current = (LinkedQuestion)bs.Current;
            FilterTree frm = new FilterTree(current, ViewBy.Filters);
            frm.Visible = true;
        }

        private void routingTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LinkedQuestion current = (LinkedQuestion)bs.Current;
            FilterTree frm = new FilterTree(current, ViewBy.Routing);
            frm.Visible = true;
        }

        // Reports
        private void dSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OutputReport(DSVReport(CurrentTiming.Questions, false), CurrentTiming.SurveyCode + " - DSV Report");
        }
         
        

        

        private void currentTimingToolStripMenuItem_Click(object sender, EventArgs e)
        {
           

            DataTable report = new DataTable();
            string fileName = "";

            report = UserBasedTimingReport();
            fileName = CurrentTiming.SurveyCode + " - Method 2 - " + CurrentTiming.Title;

            OutputReport(report, fileName);
        }

        

        #endregion

        #region Main Form Area

        /// <summary>
        /// For a given question, add a button for each possible next question.
        /// </summary>
        /// <param name="lq"></param>
        private void AddNextButtons(LinkedQuestion lq)
        {
            flowLayoutPanel1.Controls.Clear();

            foreach (KeyValuePair<int, LinkedQuestion> p in lq.PossibleNext)
            {
                if (p.Value != null)
                {
                    Button btn = new Button()
                    {
                        Text = p.Value.VarName.RefVarName,
                        Tag = p.Value.VarName.RefVarName
                    };

                    btn.Click += Btn_Click;
                    btn.Visible = true;

                    flowLayoutPanel1.Controls.Add(btn);
                }
            }
        }

        /// <summary>
        /// For each question, add a button for each VarName appearing as a filter.
        /// </summary>
        /// <param name="lq"></param>
        private void AddPrevButtons(LinkedQuestion lq)
        {
            flowLayoutPanel2.Controls.Clear();

            foreach (LinkedQuestion p in lq.FilteredOn)
            {

                Button btn = new Button()
                {
                    Text = p.VarName.RefVarName,
                    Tag = p.VarName.RefVarName
                };

                btn.Click += Btn_Click;
                btn.Visible = true;

                flowLayoutPanel2.Controls.Add(btn);
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            List<LinkedQuestion> list = (List<LinkedQuestion>)bs.DataSource;
            LinkedQuestion lq = list.Find(x => x.VarName.VarName.Equals(btn.Tag.ToString()));
            if (lq == null)
                return;
            bs.Position = list.IndexOf(lq);
        }

        /// <summary>
        /// Refresh the Current Question, question text, previous and next buttons, and the universe description.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bs_CurrentChanged(object sender, EventArgs e)
        {
            RefreshCurrentRecord();
        }

        private void RefreshCurrentRecord()
        {
            LinkedQuestion CurrentQuestion = (LinkedQuestion)bs.Current;

            rtbQuestionText.Rtf = "";
            rtbQuestionText.Rtf = CurrentQuestion.GetQuestionTextRich();

            AddNextButtons(CurrentQuestion);
            AddPrevButtons(CurrentQuestion);

            //PopulateUniverse(CurrentQuestion);

            if (CurrentTiming.NotInterpreted.Contains(CurrentQuestion))
                txtScenarios.Text = "This filter could not be interpreted.";
            else
                txtScenarios.Text = CurrentQuestion.PrintDirectFilterInstructions();



            //List<LinkedQuestion> directFilterList = CurrentTiming.GetDirectFilterVarList(CurrentQuestion);
            //List<LinkedQuestion> indirectFilterList = new List<LinkedQuestion>();
            //if (CurrentQuestion.GetQnumValue() > CurrentTiming.StartQ)
            //{
                

            //    foreach (LinkedQuestion q in directFilterList)
            //    {
            //        indirectFilterList.AddRange(CurrentTiming.GetIndirectFilterVarList(q));
            //    }
            //    indirectFilterList = indirectFilterList.Except(directFilterList).ToList();
            //}
            

            //txtDirectFilters.Text = string.Join("\r\n", directFilterList);
            //txtIndirectFilters.Text = string.Join("\r\n", indirectFilterList);
        }

        #endregion

        #region Events

        private void cboUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboUserType.SelectedItem == null)
                return;


            //Respondent r = (Respondent)((ComboBox)sender).SelectedItem;

            UserTiming selected = (UserTiming)((ComboBox)sender).SelectedItem;
            CurrentTiming = (UserTiming)((ComboBox)sender).SelectedItem; ;
            //foreach (UserTiming ut in UserTimings)
            //{
            //    if (ut.User.Description.Equals(selected.User.Description))
            //    {
            //        CurrentTiming = ut;
            //        break;
            //    }
            //}
            bsRun.DataSource = CurrentTiming;

            RefreshLists();
            UpdateVarList();
            UpdateResponses();
        }

        private void cmdTime_Click(object sender, EventArgs e)
        {

            Respondent r = CurrentTiming.User; // save currently selected user

            UpdateResponses();
            UpdateTiming();

           
            CurrentTiming.SetUser(r);

            RefreshLists();
        }

        private void cboSurvey_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSurvey.SelectedItem != null && !loading)
                ChangeSurvey((string)cboSurvey.SelectedItem);
        }

        private void cboGoToVar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboGoToVar.SelectedItem == null)
                return;

            int selected = cboGoToVar.SelectedIndex;
            LinkedQuestion selectedQ = (LinkedQuestion) cboGoToVar.SelectedItem;

            bs.Position = selected;

            ListViewItem item = lstWeightedQuestionList.FindItemWithText(selectedQ.VarName.RefVarName, true, 0, false);
            if (item != null)
            {
                lstWeightedQuestionList.SelectedItems.Clear();
                item.EnsureVisible();
                item.Selected = true;
                
            }
            
            


        }

        private void lstResponses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstResponses.SelectedItem == null)
                return;

            Answer a = (Answer)lstResponses.SelectedItem;
            int i = 0;

            bs.Position = CurrentTiming.QuestionIndex(a.VarName);

        }


        /// <summary>
        /// After selecting an item in the VarName list, go to that VarName. The index in the list should match the position of the bindingsource.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstWeightedQuestionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstWeightedQuestionList.SelectedItems.Count == 0)
                return;

            string sel = lstWeightedQuestionList.SelectedItems[0].SubItems[1].Text; // the selected Var
            int varPos = CurrentTiming.QuestionIndex(sel);                          // the index of that var in the list
            bs.Position = varPos;                                                   // move to that index
        }

        #endregion


        private string GetFilePath()
        {
            return TimingFolder + "\\" + CurrentTiming.SurveyCode + "\\Method 2\\"  ;
        }

        private string GenerateSASCodeResponseFreq(Survey survey, Survey previousWave) //string project, string projectWave)
        {
            StringBuilder s = new StringBuilder();

            

            if (survey.Wave == 1)
            {
                MessageBox.Show("This is the first wave. Response frequenceis from previous wave not available.");
                return "";
            }

            string cc = survey.CountryCode;
            string wave = Math.Floor(survey.Wave).ToString();
            int waveChar = 96 + int.Parse(wave) -1;
            char waveLetter = (char) waveChar;

            // get previous wave
            List<SurveyQuestion> previousWaveQs = DBAction.GetSurveyQuestions(previousWave).ToList();
        
            
            foreach (LinkedQuestion q in CurrentTiming.Questions)
            {
                // skip those with weights
                if (q.Weight.Value > -1)
                    continue;
                // skip those without filters
                if (q.FilterList.Count == 0)
                    continue;

                if (!AllVarsPresent(q.FilteredOn, previousWaveQs))
                    continue;

                s.Append("%filterStat(newVar = " + q.VarName.VarName + ", filter = (");
                // GET EACH FILTER LIST
                foreach (List<FilterInstruction> fl in q.FilterList)
                {
                    s.Append("(");
                    foreach (FilterInstruction fi in fl)
                    {
                        
                        string fullVarName = waveLetter + Utilities.ChangeCC(fi.VarName, cc);
                        if (fi.Range)
                            s.Append(fullVarName + " in (" + fi.ValuesStr[0] + ":" + fi.Values.Last() + ") AND ");
                        else if (fi.ValuesStr.Count > 1)
                        {
                            s.Append(fullVarName + " in (" + string.Join(", ", fi.ValuesStr) + ") AND ");
                        } else
                        {
                            string op = "=";
                            if (fi.Oper == Operation.GreaterThan)
                                op = ">";
                            else if (fi.Oper == Operation.LessThan)
                                op = "<";
                            else if (fi.Oper == Operation.NotEquals)
                                op = "<>";

                            s.Append(fullVarName + op + fi.ValuesStr[0] + ") AND ");
                        }
                        
                    }
                    s.Length--;
                    s.Length--;
                    s.Length--;
                    s.Length--;
                    
                    //s.Append("")
                    s.Append(") OR ");
                }
                s.Length--;
                s.Length--; s.Length--;
                s.Length--;
                s.AppendLine(");");
            }
           

            return s.ToString();
        }

        private bool AllVarsPresent(List<LinkedQuestion> filterVars, List<SurveyQuestion> refList)
        {
            bool allFound = false;

            foreach (LinkedQuestion lq in filterVars) {
                if (!refList.Exists(x=>x.VarName.RefVarName.Equals(lq.VarName.RefVarName)))
                    return allFound;
            }
            allFound = true;
            return allFound;
        }

        private Respondent GetCurrentRespondent()
        {
            if (cboUserType.SelectedItem == null)
                return new Respondent();

            Respondent r = new Respondent((Respondent)cboUserType.SelectedItem);

            return (Respondent)cboUserType.SelectedItem;
        }

         
        private bool IsOtherSpecify(List<SurveyQuestion> sourceList, SurveyQuestion q)
        {
            string varname = q.VarName.RefVarName;
            if (varname.EndsWith("o"))
            {
                var nonO = sourceList.Where(x => x.VarName.RefVarName.Equals(varname.Substring(0, varname.Length - 1)));

                if (nonO.Count() > 0)
                    return true;
                
            }

            return false;
        }

        private DataTable UserBasedTimingReport()
        {
            // create the data table
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("Qnum"));
            dt.Columns.Add(new DataColumn("VarName"));
            dt.Columns.Add(new DataColumn("Question"));
            dt.Columns.Add(new DataColumn("VarLabel"));
            dt.Columns.Add(new DataColumn("Filter"));
            dt.Columns.Add(new DataColumn("Words"));
            dt.Columns.Add(new DataColumn("Timing Estimate (secs)"));
            
            foreach (UserTiming ut in UserTimings)
            {
                dt.Columns.Add(new DataColumn(ut.User.Description));
            }

            double totalTime = 0;

            // for each question, 
            // if there is an answer provided, 
            // get the list of questions that are directly and indirectly impacted by this answer, and create a list with varlabel etc.
            // otherwise, the list is empty
            foreach (LinkedQuestion lq in UserTimings[0].Questions)
            {

                // construct the row
                DataRow r = dt.NewRow();

                // basic info
                r["Qnum"] = lq.Qnum;
                r["VarName"] = lq.VarName.RefVarName;
                r["Question"] = lq.GetQuestionText();
                r["VarLabel"] = "<strong><em>" + lq.VarName.VarLabel + "</em></strong>\r\n" + lq.RespOptions + "\r\n" + lq.NRCodes;
                
                

                //if heading, move on
                if (lq.VarName.RefVarName.StartsWith("Z"))
                {
                    dt.Rows.Add(r);
                    continue;
                }

                // add filter if there is a VarName in the filter
                if (lq.FilteredOn.Count > 0)
                    r["Filter"] = lq.PreP;
                else
                    r["Filter"] = "";


                r["Words"] = lq.WordCount();

                // timing info

                double time;
                if (CurrentTiming.IsForTiming(lq))
                    time = lq.GetTiming(CurrentTiming.WPM, false);
                else
                    time = 0;

                double weight = lq.Weight.Value;

                r["Timing Estimate (secs)"] = time.ToString("N2");

                totalTime += time;

                // user types

                foreach (UserTiming ut in UserTimings)
                {
                    
                    if (ut.UserQuestionsMax.Contains(lq, new LinkedQuestionComparer()) && ut.UserQuestionsMin.Contains(lq, new LinkedQuestionComparer()))
                        r[ut.User.Description] = "+/-";
                    else if (ut.UserQuestionsMax.Contains(lq, new LinkedQuestionComparer()) && !ut.UserQuestionsMin.Contains(lq, new LinkedQuestionComparer()))
                        r[ut.User.Description] = "+";
                    else if (ut.UserQuestionsMin.Contains(lq, new LinkedQuestionComparer()) && !ut.UserQuestionsMax.Contains(lq, new LinkedQuestionComparer()))
                        r[ut.User.Description] = "-";
                    else
                        r[ut.User.Description] = "N/A";
                }


                dt.Rows.Add(r);
            }

            DataRow total = dt.NewRow();
            total["VarName"] = "Total";
            //total["Timing Estimate (secs)"] = ((double)totalTime / 60).ToString("N2") + " mins";
            dt.Rows.Add(total);

            DataRow total2 = dt.NewRow();
            total2["VarName"] = "Total";
            //total2["Timing Estimate (secs)"] = ((double)totalTime / 60).ToString("N2") + " mins";

            DataRow meanRow = dt.NewRow();
            meanRow["VarName"] = "Mean";

            DataRow weightRow = dt.NewRow();
            weightRow["VarName"] = "Weight";

            DataRow wmeanRow = dt.NewRow();
            wmeanRow["VarName"] = "Weighted Mean";

            double wmean = 0;

            DataRow defs = dt.NewRow();
            defs["VarName"] = "User Definition";

            DataRow totalWords = dt.NewRow();
            totalWords["VarName"] = "Average Word Count";

            foreach (UserTiming ut in UserTimings)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Answer a in ut.User.Responses)
                {
                    sb.AppendLine(a.ToString());
                }

                defs[ut.User.Description] = sb.ToString();
                total2[ut.User.Description] = ut.User.TotalMaxTime + " mins (max)\r\n" + ut.User.TotalMinTime + " mins (min)";
                meanRow[ut.User.Description] = ((ut.User.TotalMaxTime + ut.User.TotalMinTime) /2).ToString("N2");
                weightRow[ut.User.Description] = ut.User.Weight;
                wmeanRow[ut.User.Description] = (((ut.User.TotalMaxTime + ut.User.TotalMinTime) / 2) * ut.User.Weight).ToString("N2");
                wmean += (((ut.User.TotalMaxTime + ut.User.TotalMinTime) / 2) * ut.User.Weight);
                totalWords[ut.User.Description] = ut.MeanWordCount();
            }

            wmeanRow["Timing Estimate (secs)"] = wmean.ToString("N2");

            dt.Rows.InsertAt(wmeanRow, 0);
            dt.Rows.InsertAt(weightRow, 0);
            
            dt.Rows.InsertAt(meanRow, 0);
            dt.Rows.InsertAt(total2, 0);
            dt.Rows.InsertAt(defs, 0);

            dt.Rows.Add(totalWords);

            return dt;
        }

        

        /// <summary>
        /// Generates a table with all questions in the survey. Includes columns for DSVs and filter lists
        /// </summary>
        /// <param name="questionList"></param>
        /// <param name="removeNoFilter"></param>
        /// <returns></returns>
        private DataTable DSVReport(List<LinkedQuestion> questionList, bool removeNoFilter = true)
        { 
            
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("Qnum"));
            dt.Columns.Add(new DataColumn("VarName"));
            
            dt.Columns.Add(new DataColumn("Question"));
            dt.Columns.Add(new DataColumn("VarLabel"));
            dt.Columns.Add(new DataColumn("DSV"));
            dt.Columns.Add(new DataColumn("Used As Filter For"));

            dt.Columns.Add(new DataColumn("Filters"));

            foreach (LinkedQuestion lq in questionList)
            {
 
                int directFilterCount=0;
                int indirectFilterCount=0;
                string filterList, filterExpList = "";
                
                filterList = "";

                List<LinkedQuestion> directFilterList = CurrentTiming.GetDirectFilterVarList(lq);
                directFilterCount = directFilterList.Count();

                filterExpList = GetDirectFilterListColumn(lq, directFilterList);

              
                List<LinkedQuestion> indirectFilterList = new List<LinkedQuestion>();

                if (lq.GetQnumValue() > CurrentTiming.StartQ )
                {
                    foreach (LinkedQuestion q in directFilterList)
                    {
                        indirectFilterList.AddRange(CurrentTiming.GetIndirectFilterVarList(q));
                    }

                    indirectFilterList = indirectFilterList.Except(directFilterList).ToList();
                    indirectFilterCount = indirectFilterList.Count();
                }
                
                filterList = UsedAsFilterColumn(lq, directFilterList, indirectFilterList);
                
                
                if (directFilterCount == 0 && removeNoFilter)
                    continue;

                //if (Utilities.GetQuestionType(lq) != QuestionType.Standalone)
                //   continue; 

                DataRow r = dt.NewRow();               

                // basic info
                r["Qnum"] = lq.Qnum;
                r["VarName"] = lq.VarName.RefVarName;
                r["Filters"] = filterExpList;
                r["Question"] = lq.GetQuestionText();
                r["VarLabel"] = "<strong><em>" + lq.VarName.VarLabel + "</em></strong>\r\n" + lq.RespOptions + "\r\n" + lq.NRCodes;


                //filter info
                if (directFilterCount > 0 || indirectFilterCount > 0)
                {
                    r["DSV"] = directFilterCount + indirectFilterCount;
                }


                r["Used As Filter For"] = filterList;


                dt.Rows.Add(r);
            }
            
            
            return dt;

        }

        private void OutputReport(DataTable dt, string customFileName, string customLocation = "\\\\psychfile\\psych$\\psych-lab-gfong\\SMG\\SDI\\Reports\\")
        {

            SurveyReport SR = new SurveyReport();
            SR.Surveys.Add(new ReportSurvey(CurrentTiming.SurveyCode));
            SR.FileName = customLocation;
            
            SR.ReportTable = dt;
            SR.OutputReportTableXML(customFileName);
        }

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

            List<LinkedQuestion> directList = CurrentTiming.GetDirectFilterConditionList(fi);
            int directCount = directList.Count();
            int indirectCount = 0;

            if (includeIndirect)
            {
                List<LinkedQuestion> indirectList = new List<LinkedQuestion>();
                foreach (LinkedQuestion q in indirectFilterList)
                {
                    if (CurrentTiming.QuestionHasIndirectFilter(q, fi))
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
                List<LinkedQuestion> directList = CurrentTiming.GetDirectFilterConditionList(fi);
                int directCount = directList.Count();

                string direct = "";
                string indirect = "";
                int indirectCount =0;
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
                        if (CurrentTiming.QuestionHasIndirectFilter(q, fi))
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
                            indirect ;
            }

            filterList = Utilities.TrimString(filterList, "\r\n");
            return filterList;
        }

        

        /// <summary>
        /// Adds color and formatting to the specified row, based on its QuestionType.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="questionType"></param>
        private void FormatListItem(ListViewItem row, QuestionType questionType)
        {
            // color row based on type
            row.UseItemStyleForSubItems = true;
            row.Tag = questionType;


            switch (questionType)
            {
                case QuestionType.Series:
                    row.ForeColor = System.Drawing.Color.Black;
                    break;
                case QuestionType.Standalone:
                    row.ForeColor = System.Drawing.Color.Blue;
                    row.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                    break;

                case QuestionType.Heading:
                    row.ForeColor = System.Drawing.Color.Magenta;
                    row.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                    break;
                case QuestionType.InterviewerNote:
                    row.ForeColor = System.Drawing.Color.Lime;
                    row.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                    break;
                case QuestionType.Subheading:
                    row.ForeColor = System.Drawing.Color.LightBlue;
                    row.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                    break;

            }

        }

        /// <summary>
        /// Determines the type of questions for the given row.
        /// </summary>
        /// <param name="row"></param>
        /// <returns>QuestionType enum based on the Qnum and VarName.</returns>
        private QuestionType GetQuestionType(ListViewItem row)
        {
            string qnum = row.Text;
            string varname = row.SubItems[1].Text;

            int head = Int32.Parse(Utilities.GetSeriesQnum(qnum));
            string tail = Utilities.GetQnumSuffix(qnum);

            QuestionType qType;

            // get Question Type
            if (varname.StartsWith("Z"))
            {
                if (varname.EndsWith("s"))
                    qType = QuestionType.Subheading; // subheading
                else
                    qType = QuestionType.Heading; // heading
            }
            else if (varname.StartsWith("HG"))
            {
                qType = QuestionType.Standalone; // QuestionType.InterviewerNote; // interviewer note
            }
            else
            {
                if ((tail == "" || tail == "a") && (head != 0))
                    qType = QuestionType.Standalone; // standalone or first in series
                else
                    qType = QuestionType.Series; // series
            }
            return qType;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void cmdRefreshLists_Click(object sender, EventArgs e)
        {
            RefreshLists();
        }

        private void LoadTimingRun()
        {
            // get path
            var filePath = string.Empty;

            // user common open file dialog here
            using (CommonOpenFileDialog folderBrowser = new CommonOpenFileDialog())
            {
                folderBrowser.IsFolderPicker = true;
                folderBrowser.InitialDirectory = GetFilePath();

                if (folderBrowser.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    filePath = folderBrowser.FileName;
                }
                else
                {
                    return;
                }
            }

            if (string.IsNullOrEmpty(filePath))
                return;

            loading = true;

            var files = Directory.GetFiles(filePath, "*.xml");
            UserTimings.Clear();
            foreach (string f in files)
            {
                UserTimings.Add(new UserTiming(File.ReadAllText(f)));
            }

            CurrentTiming = UserTimings[0];

            bsRun.DataSource = CurrentTiming;
            bs.DataSource = CurrentTiming.Questions;

            // bsRun.ResetBindings(true);
            RefreshLists();
            UpdateTiming();

            loading = false;
        }

        private void SaveTimingRun()
        {
            if (MessageBox.Show("Confirm saving run with title:\r\n\r\n" + CurrentTiming.Title, "Confirm Save", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            // save the timing run
            string runFolder = GetFilePath() + "\\" + CurrentTiming.Title + " " + DateTime.Now.ToShortDateString();
            Directory.CreateDirectory(runFolder);

            foreach (UserTiming ut in UserTimings)
            {
                string userTypeFile = runFolder + "\\" + CurrentTiming.Title + " - " + ut.User.Description + ".xml";

                File.WriteAllText(userTypeFile, ut.ExportToXML());
            }

            DataTable report = new DataTable();
            string fileName = "";

            report = UserBasedTimingReport();
            fileName = CurrentTiming.SurveyCode + " Method 2 - " + CurrentTiming.Title;

            OutputReport(report, fileName, runFolder + "\\");

            MessageBox.Show("Saved.");
           
        }



       

        private void includeNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentTiming.IncludeNotes = includeNotesToolStripMenuItem.Checked;
        }

        private void CustomListOptions_Click(object sender, EventArgs e)
        {
            if (excludeToolStripMenuItem.Checked)
                limitToToolStripMenuItem.Checked = false;
            else if (limitToToolStripMenuItem.Checked)
                excludeToolStripMenuItem.Checked = false;

            ExcludeCustomList = excludeToolStripMenuItem.Checked;
            OnlyCustomList = limitToToolStripMenuItem.Checked;

        }

        private void TimingRadioButtons_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Tag.Equals("WPM"))
            {
                txtKnownWPM.Enabled = true;
                txtTargetTime.Enabled = true;
                txtKnownTime.Enabled = false;
                txtTargetWPM.Enabled = false;
            }
            else if (rb.Tag.Equals("Time"))
            {
                txtKnownWPM.Enabled = false;
                txtTargetTime.Enabled = false;
                txtKnownTime.Enabled = true;
                txtTargetWPM.Enabled = true;
            }
        }

        private void ShowUserQs_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton chk = (RadioButton)sender;

            if (chk.Checked)
            {
                RefreshLists();
                UpdateVarList();
            }
        }

        

        // count gateways
        private int CountGateways()
        {
            int gatewayCount = 0;
            foreach (LinkedQuestion q in CurrentTiming.Questions)
            {
                if (CurrentTiming.GetDirectFilterVarList(q).Count() > 0)
                    gatewayCount++;
            }
            return gatewayCount;
        }

        private void txtKnownWPM_TextChanged(object sender, EventArgs e)
        {
            foreach(UserTiming ut in UserTimings)
            {
                if (!string.IsNullOrEmpty(txtKnownWPM.Text))
                    ut.WPM = Int32.Parse(txtKnownWPM.Text);
                else
                    ut.WPM = 0;
            }
        }

        #region Not Implemented or Not working or Temporary

        private void FrequencyTable(DataTable dt)
        {
            // frequenct table
            SurveyReport freq = new SurveyReport();
            freq.Surveys.Add(new ReportSurvey("NZL3"));
            freq.FileName = "\\\\psychfile\\psych$\\psych-lab-gfong\\SMG\\SDI\\Reports\\";
            DataTable dt2 = new DataTable();

            dt2.Columns.Add(new DataColumn("Word Count"));
            dt2.Columns.Add(new DataColumn("Question"));
            dt2.Columns.Add(new DataColumn("Responses"));

            List<int> counts = new List<int>();
            Dictionary<int, int> qCounts = new Dictionary<int, int>();
            Dictionary<int, int> rCounts = new Dictionary<int, int>();

            foreach (DataRow r in dt.Rows)
            {
                int count = Int32.Parse((string)r["Question Word Count"]);

                if (!counts.Contains(count))
                    counts.Add(count);

                if (!qCounts.ContainsKey(count))
                    qCounts.Add(count, 1);
                else
                    qCounts[count]++;

                count = Int32.Parse((string)r["Response Word Count"]);

                if (!counts.Contains(count))
                    counts.Add(count);

                if (!rCounts.ContainsKey(count))
                    rCounts.Add(count, 1);
                else
                    rCounts[count]++;
            }

            foreach (int i in counts)
            {
                DataRow r = dt2.NewRow();
                r["Word Count"] = i;

                if (qCounts.ContainsKey(i))
                    r["Question"] = qCounts[i];
                else
                    r["Question"] = 0;

                if (rCounts.ContainsKey(i))
                    r["Responses"] = rCounts[i];
                else
                    r["Responses"] = 0;

                dt2.Rows.Add(r);
            }

            freq.ReportTable = dt2;
            freq.OutputReportTableXML();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lq"></param>
        private void PopulateUniverse(LinkedQuestion lq)
        {
            // populate universe
            txtScenarios.Text = "";


            string universe = PrintFilterInstructions(lq);


            universe = universe.Replace("Ask if ", "");

            universe = Utilities.TrimString(universe, "\r\n");

            string[] lines = universe.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string[] distinct = lines.Distinct().ToArray();

            txtScenarios.Text = universe; // string.Join(" AND \r\n", distinct);

            lblUniverse.Text = "Ask " + lq.VarName.VarName + " if:";
        }





        /// <summary>
        /// Print the filter instructions for the given questions and the filter instructions for any questions referenced in those instructions.
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        private string PrintFilterInstructions(LinkedQuestion question)
        {
            string result = question.VarName + ":\r\n";
            if (question.FilterList.Count == 0)
                return result + "(no filter)\r\n";
            else
            {
                foreach (List<FilterInstruction> fl in question.FilterList)
                {
                    foreach (FilterInstruction fi in fl)
                        result += fi + "\r\n";

                    result += "OR\r\n";
                }

                result = Utilities.TrimString(result, "OR\r\n");

                result += " AND \r\n";

                foreach (LinkedQuestion q in question.FilteredOn)
                {
                    result += PrintFilterInstructions(q);
                }
            }

            return result;
        }

        /// <summary>
        /// Print the PreP for the given question and the PreP for any questions referenced in the PreP.
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        private string PrintFilters(LinkedQuestion question)
        {
            string result = "";
            if (!question.PreP.StartsWith("Ask all."))
                result += question.PreP.Replace("<br>", "\r\n") + "\r\n";

            foreach (LinkedQuestion q in question.FilteredOn)
            {
                result += PrintFilters(q);
            }

            return result;
        }


        #endregion
    }
}

