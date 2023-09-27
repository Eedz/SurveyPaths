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
using System.Text.RegularExpressions;
using BooleanLogicParser;
using System.IO;
using System.Xml;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using OpenXMLHelper;

namespace SurveyPaths
{    
    public enum TimingType { Undefined, Max, Min }
    

    public partial class SurveyTimingForm : Form
    {
        public string TimingFolder = "\\\\psychfile\\psych$\\psych-lab-gfong\\SMG\\SDI\\Survey Timing";
        SurveyTiming CurrentTiming;

        BindingSource bs;
        BindingSource bsRun;

        bool loading = false; // true if we are loading a past run

        List<string> CustomList;

        public bool ExcludeCustomList;
        public bool OnlyCustomList;

        public SurveyTimingForm(string survey)
        {
            InitializeComponent();

            //ToolStripBindableMenuItem smartWordCountMenuItem = new ToolStripBindableMenuItem();
            //smartWordCountMenuItem.Text = "Smart Word Count";
            //smartWordCountMenuItem.CheckOnClick = true;
            //smartWordCountMenuItem.Checked = true;
            //optionsToolStripMenuItem.DropDownItems.Add(smartWordCountMenuItem);

            bs = new BindingSource();
            bsRun = new BindingSource();
            bindingNavigator1.BindingSource = bs;

            CustomList = new List<string>();

            CurrentTiming = new SurveyTiming();
            CurrentTiming.SurveyCode = survey;

            ChangeSurvey(survey);

            RefreshCurrentRecord();

            // bindings
            bsRun.DataSource = CurrentTiming;

            cboSurvey.DataSource = DBAction.GetAllSurveysInfo().Select(x=>x.SurveyCode).ToList();
            cboSurvey.SelectedItem = null;
            cboSurvey.DataBindings.Add(new Binding("SelectedItem", bsRun, "SurveyCode"));
          
            txtTimingTitle.DataBindings.Add("Text", bsRun, "Title", true, DataSourceUpdateMode.OnPropertyChanged);
            txtKnownWPM.DataBindings.Add(new Binding("Text", bsRun, "WPM"));
            txtStartAt.DataBindings.Add(new Binding("Text", bsRun, "StartQ"));
            txtNotes.DataBindings.Add(new Binding("Text", bsRun, "Notes"));
            
            rbKnownWPM.Checked = true;
            
            txtVarName.DataBindings.Clear();
            txtVarName.DataBindings.Add("Text", bs, "VarName.VarName");

            txtQnum.DataBindings.Clear();
            txtQnum.DataBindings.Add("Text", bs, "Qnum");

            txtWeight.DataBindings.Clear();
            txtWeight.DataBindings.Add("Text", bs, "Weight.Value");

            txtWeightSource.DataBindings.Clear();
            txtWeightSource.DataBindings.Add("Text", bs, "Weight.Source");

            //smartWordCountMenuItem.DataBindings.Add("Checked", CurrentTiming, "SmartWordCount");
            //smartWordCountMenuItem.Checked = true;
            CurrentTiming.SmartWordCount = true;
            cboSurvey.SelectedIndexChanged += cboSurvey_SelectedIndexChanged;
            bs.CurrentChanged += Bs_CurrentChanged;
        }

        /// <summary>
        /// Load the form with an empty run, then load the provided run.
        /// </summary>
        /// <param name="timing"></param>
        public SurveyTimingForm(SurveyTiming timing)
        {
            InitializeComponent();

            //ToolStripBindableMenuItem smartWordCountMenuItem = new ToolStripBindableMenuItem();
           // smartWordCountMenuItem.Text = "Smart Word Count";
            optionsToolStripMenuItem.DropDownItems.Add(smartWordCountToolStripMenuItem);

            loading = true;
            bs = new BindingSource();
            bsRun = new BindingSource();
            bindingNavigator1.BindingSource = bs;

            CustomList = new List<string>();

            CurrentTiming = new SurveyTiming();

            bs.DataSource = CurrentTiming.Questions;
            bs.ResetBindings(true);
            bsRun.ResetBindings(true);

            // bindings
            bsRun.DataSource = CurrentTiming;

            cboSurvey.DataSource = DBAction.GetAllSurveysInfo().Select(x => x.SurveyCode).ToList();;
            cboSurvey.SelectedItem = null;
            cboSurvey.DataBindings.Add(new Binding("SelectedItem", bsRun, "SurveyCode"));

            txtTimingTitle.DataBindings.Add("Text", bsRun, "Title", true, DataSourceUpdateMode.OnPropertyChanged);
            txtKnownWPM.DataBindings.Add(new Binding("Text", bsRun, "WPM"));
            txtStartAt.DataBindings.Add(new Binding("Text", bsRun, "StartQ"));
            txtNotes.DataBindings.Add(new Binding("Text", bsRun, "Notes"));
            txtTargetTime.DataBindings.Add(new Binding("Text", bsRun, "TotalWeightedTime"));
            rbKnownWPM.Checked = true;

            txtVarName.DataBindings.Clear();
            txtVarName.DataBindings.Add("Text", bs, "VarName.VarName");

            txtQnum.DataBindings.Clear();
            txtQnum.DataBindings.Add("Text", bs, "Qnum");

            txtWeight.DataBindings.Clear();
            txtWeight.DataBindings.Add("Text", bs, "Weight.Value");

            txtWeightSource.DataBindings.Clear();
            txtWeightSource.DataBindings.Add("Text", bs, "Weight.Source");

           // smartWordCountMenuItem.DataBindings.Add("Checked", bs, "SmartWordCount");

            cboSurvey.SelectedIndexChanged += cboSurvey_SelectedIndexChanged;
            bs.CurrentChanged += Bs_CurrentChanged;
            LoadRun(timing);

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

        private void NewTimingRun()
        {
            CurrentTiming = new SurveyTiming();

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

            bs.DataSource = CurrentTiming.Questions;
            bs.ResetBindings(true);
            bsRun.ResetBindings(true);

            RefreshLists();
            UpdateTiming();
            UpdateVarList();

            RefreshCurrentRecord();
        }

        private void SetSurvey(string surveyCode)
        {
            CurrentTiming.SurveyCode = surveyCode;
            Survey survey = DBAction.GetAllSurveysInfo().Where(x=>x.SurveyCode.Equals(surveyCode)).FirstOrDefault();

            SetReferenceSurvey(survey);

            var qs = DBAction.GetSurveyQuestions(survey).ToList();

            CurrentTiming.Questions.Clear();

            foreach (SurveyQuestion q in qs)
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
                
                if (IsOtherSpecify(qs, q))
                    continue;

                CurrentTiming.Questions.Add(new LinkedQuestion(q));
            }

            PopulateWeights();
            
            CurrentTiming.PopulateFilters();
        }

        private void SetReferenceSurvey(Survey survey)
        {
            // set reference survey
            double wave = survey.Wave;
            string previousWaveCode;

            if (wave > 1 && wave % 1 == 0)
                previousWaveCode = survey.SurveyCodePrefix + (wave - 1);
            else if (wave % 1 == 0.5)
                previousWaveCode = survey.SurveyCodePrefix + (wave - 0.5);
            else
                previousWaveCode = survey.SurveyCode;

            CurrentTiming.ReferenceSurvey = DBAction.GetAllSurveysInfo().Where(x => x.SurveyCode.Equals(previousWaveCode)).FirstOrDefault();
            CurrentTiming.ReferenceSurvey.AddQuestions(DBAction.GetSurveyQuestions(CurrentTiming.ReferenceSurvey));
  
        }

        /// <summary>
        /// Update the VarName list.
        /// </summary>
        /// <param name="list"></param>
        private void UpdateVarList()
        {
            cboGoToVar.DataSource = null;
            cboGoToVar.DataSource = CurrentTiming.Questions;
        }

        /// <summary>
        /// Assign a weight of 0 to non-timing questions and 1 to ask all questions.
        /// </summary>
        private void UpdateStage1Weights()
        {
            int count1 = 0;
            int count0 = 0;
            foreach (LinkedQuestion lq in CurrentTiming.Questions)
            {

                if (lq.VarName.RefVarName.StartsWith("Z"))
                {
                    lq.Weight.Value = 0;
                    lq.Weight.Source = "A";
                    count0++;
                }
                else if (lq.PreP.StartsWith("Ask all.") || lq.PreP.Contains("Ask all."))
                {
                    lq.Weight.Source = "A";
                    lq.Weight.Value = 1;
                    count1++;
                }
                else if (lq.IsProgramming() || lq.IsDerived() || lq.IsTermination())
                {
                    lq.Weight.Value = 0;
                    lq.Weight.Source = "A";
                    count0++;
                }

                

            }

            txtMessages.Text += (int)(count1 + count0) + " weights automatically assigned. 0: " + count0 + ", 1: " + count1 + "\r\n";
        }

        private void RefreshLists()
        {
            if (CurrentTiming.Questions == null) return;

            lstWeightedQuestionList.Items.Clear();
            lstWeightedQuestionList.View = System.Windows.Forms.View.Details;

            lstUnweightedQuestionList.Items.Clear();
            lstUnweightedQuestionList.View = System.Windows.Forms.View.Details;

            chUser.Width = 0;
            chWeight.Width = 60;
            chWeightSource.Width = 60;

            foreach (LinkedQuestion lq in CurrentTiming.Questions)
            {

                ListViewItem li = new ListViewItem(new string[] { lq.Qnum, lq.VarName.RefVarName, lq.VarName.VarLabel, lq.Weight.Value.ToString(), lq.Weight.Source,
                    lq.WordCount().ToString(), (lq.GetTiming(CurrentTiming.WPM, CurrentTiming.SmartWordCount, CurrentTiming.IncludeNotes) * lq.Weight.Value).ToString() });
                li.Tag = lq;

                if (lq.Weight.Value == -1)
                    lstUnweightedQuestionList.Items.Add(li);
                else
                    lstWeightedQuestionList.Items.Add(li);


                FormatListItem(li, GetQuestionType(li));
            }

            lblMissingWeights.Text = "Missing weights: " + lstUnweightedQuestionList.Items.Count;
            lstUnweightedQuestionList.Visible = true;
            lblMissingWeights.Visible = true;
                        
        }

        private void UpdateTiming()
        {
            if (CurrentTiming.WPM < 30)
                return;

            lblTotalTime.Text = "";

            CurrentTiming.TotalTime = Math.Round(CurrentTiming.GetWholeSurveyTiming(CurrentTiming.WPM), 2);
            CurrentTiming.TotalWeightedTime = Math.Round(CurrentTiming.GetWeightedWholeSurveyTiming(CurrentTiming.WPM), 2);

            for (int i = CurrentTiming.WPM - 30; i < CurrentTiming.WPM; i += 10)
            {
                lblTotalTime.Text += "\r\nTime (" + i + " WPM): " + CurrentTiming.GetWeightedWholeSurveyTiming(i).ToString("N2") + " mins weighted";
            }
            lblTotalTime.Text = lblTotalTime.Text.Substring(2);

            lblTotalTime3.Text = "";
            for (int i = CurrentTiming.WPM + 10; i <= CurrentTiming.WPM + 30; i += 10)
            {
                lblTotalTime3.Text += "Time (" + i + " WPM): " + CurrentTiming.GetWeightedWholeSurveyTiming(i).ToString("N2") + " mins weighted\r\n";
            }

            if (!string.IsNullOrEmpty(txtKnownTime.Text))
                txtTargetWPM.Text = CurrentTiming.GetTargetWPM(Double.Parse(txtKnownTime.Text)).ToString("N2");

            txtTargetTime.DataBindings.Clear();
            txtTargetTime.DataBindings.Add(new Binding("Text", CurrentTiming, "TotalWeightedTime"));


        }
       

        #region Menu Strip
        // Save/Load

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewTimingRun();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string DataFile;

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = GetFilePath();
                saveFileDialog.Filter = "XML documents (*.xml)|*.xml";
                saveFileDialog.FilterIndex = 2;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = CurrentTiming.SurveyCode + " Method 3 - " + CurrentTiming.Title + " " + DateTime.Now.ToString("dd-MMM-yyyy hh.mm");

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DataFile = saveFileDialog.FileName;
                }
                else
                {
                    return;
                }
            }

            File.WriteAllText(DataFile, CurrentTiming.ExportToXML());

            DataTable report = new DataTable();
            string fileName = "";

            report = WholeSurveyTimingTable(CurrentTiming.Questions);
            fileName = CurrentTiming.SurveyCode + " - Method 3 - " + CurrentTiming.Title;


            OutputReport(report, fileName, GetFilePath() + "Saved Timing Runs\\");
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // get path
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = GetFilePath();
                openFileDialog.Filter = "XML Documents (*.xml)|*.xml";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                }
            }

            if (string.IsNullOrEmpty(filePath))
                return;

            SurveyTiming run = new SurveyTiming(File.ReadAllText(filePath));
            LoadRun(run);
        }

        private void LoadRun(SurveyTiming run)
        {

            loading = true;

            CurrentTiming = run;

            bsRun.DataSource = CurrentTiming;
            bs.DataSource = CurrentTiming.Questions;
            txtTargetTime.DataBindings.Clear();
            txtTargetTime.DataBindings.Add(new Binding("Text", bsRun, "TotalWeightedTime"));
        
            RefreshLists();
            RefreshCurrentRecord();

            txtMessages.Text = "Loaded run: " + CurrentTiming.Title;
            lblTotalTime.Text = "";
            lblTotalTime3.Text = "";

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


        // Weights
        private void importToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = GetFilePath();
                openFileDialog.Filter = "Text/Word files (*.txt, *.doc, *.docx)|*.txt;*.doc;*.docx";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                }
            }

            try
            {
                if (filePath.EndsWith("txt"))
                    ImportWeights(filePath);
                else
                    ImportWeightsFromWord(filePath);

                RefreshLists();
               
                UpdateTiming();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error importing weight. Ensure that the file is formatted correctly.");
            }

        }

        // Reports
        private void currentTimingToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DataTable report = new DataTable();
            string fileName = "";
            
            report = WholeSurveyTimingTable(CurrentTiming.Questions);
            fileName = CurrentTiming.SurveyCode + " - Method 3 Timing";
  

            OutputReport(report, fileName);
        }

        private void missingWeightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OutputReport(MissingWeightsReport(CurrentTiming.Questions), CurrentTiming.SurveyCode + " - Missing Weights Report");
        }

        private void generateResponseFreqCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateSAS frm = new GenerateSAS();

            frm.ShowDialog();

            if (frm.DialogResult != DialogResult.OK)
                return;

            if (frm.Surv.SurveyCode.Equals(CurrentTiming.SurveyCode)) {
                MessageBox.Show("Please choose a different survey than the current timing run.");
                return;
            }

            Survey survey = DBAction.GetAllSurveysInfo().Where(x => x.SurveyCode.Equals(CurrentTiming.SurveyCode)).FirstOrDefault();

            string sas = GenerateSASCodeResponseFreq(survey, frm.Surv);
            if (string.IsNullOrEmpty(sas))
                return;
            Directory.CreateDirectory(GetFilePath() + "\\Code");

            string filename = GetFilePath() + "\\Code\\" + CurrentTiming.SurveyCode + "-ResponseFrequencyCode.txt";
            File.WriteAllText(filename, sas);

            System.Diagnostics.Process.Start(filename);
            
        }

        

        #endregion

        #region Main Form Area

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

            AddPrevButtons(CurrentQuestion);

            
        }

        #endregion

        #region Events

        private void cmdTime_Click(object sender, EventArgs e)
        {
            UpdateTiming();

            RefreshLists();
        }

        private void cboSurvey_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSurvey.SelectedItem != null && !loading)
                ChangeSurvey((string)cboSurvey.SelectedItem);
            else if (loading)
                loading = false;
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
                return;
            }
            
            item = lstUnweightedQuestionList.FindItemWithText(selectedQ.VarName.RefVarName, true, 0, false);

            if (item != null)
            {
                lstUnweightedQuestionList.SelectedItems.Clear();
                item.EnsureVisible();
                item.Selected = true;
            }


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

        private void lstUnweightedQuestionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstUnweightedQuestionList.SelectedItems.Count == 0)
                return;

            string sel = lstUnweightedQuestionList.SelectedItems[0].SubItems[1].Text; // the selected Var
            int varPos = CurrentTiming.QuestionIndex(sel);                          // the index of that var in the list
            bs.Position = varPos;
        }

        

        

        #endregion


        private void PopulateWeights()
        {
            // get file path
            string folder = GetFilePath();
            // stage 1
            UpdateStage1Weights();
            // stage 2
            ImportWeights(folder + CurrentTiming.SurveyCode + "-stage2.txt");
            // stage 3
            ImportWeights(folder + CurrentTiming.SurveyCode + "-stage3.txt");
            // stage 4
            ImportWeights(folder + CurrentTiming.SurveyCode + "-stage4.txt");
        }

        private string GetFilePath()
        {
            return TimingFolder + "\\" + CurrentTiming.SurveyCode + "\\Method 3\\" ;
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

        /// <summary>
        /// Assigns weights to any VarNames found in the file. Each line is assumed to be of the form [VarName];[Weight];[Source]
        /// </summary>
        /// <param name="filePath"></param>
        private void ImportWeights(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return;

            try
            {
                string[] lines = System.IO.File.ReadAllLines(filePath);
                string message = "";
                int count = 0;
            

                foreach (string line in lines)
                {
                    if (string.IsNullOrEmpty(line))
                        continue;

                    int equals = line.IndexOf('=');
                    int semicolon = line.IndexOf(';');
                    int semicolon2 = line.IndexOf(';', semicolon + 1);

                    string varname = line.Substring(0, semicolon);

                    double weight = Double.Parse(line.Substring(semicolon + 1, semicolon2 - semicolon - 1));
                    string source = line.Substring(semicolon2 + 1);

                    LinkedQuestion q2 = CurrentTiming.Questions.SingleOrDefault(x => x.VarName.RefVarName.Equals(varname)|| x.VarName.VarName.Equals(varname));

                    if (q2 != null && q2.Weight.Source != "A")// do not override automatic weights
                    {
                        q2.Weight = new Weight(weight, source);
                        count++;
                    }

                }

                message = count + " weights imported from " + Path.GetFileName(filePath) + "\r\n";
                txtMessages.Text += message;
            }
            catch (FileNotFoundException nfe)
            {
                
                txtMessages.Text += "Could not find file: " + Path.GetFileName(nfe.FileName) + "\r\n";
            }
            catch (Exception e)
            {
                int i = 0;

            }
        }

        /// <summary>
        /// Assigns weights to any VarNames found in the file. The file is assumed to have one table with the 2nd column containing VarNames and the last column containing weights.
        /// </summary>
        /// <param name="filePath"></param>
        private void ImportWeightsFromWord(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return;

            string message = "";
            int count = 0;

            // Open word document for read  
            using (var doc = WordprocessingDocument.Open(filePath, false))
            {
                // To create a temporary table   
                

                // Find the first table in the document.   
                Table table = doc.MainDocumentPart.Document.Body.Elements<Table>().First();

                // To get all rows from table  
                IEnumerable<TableRow> rows = table.Elements<TableRow>();
                string source = "G";

                // To read data from rows and to add records to the temporary table  
                foreach (TableRow row in rows)
                {
                    string varname = row.Elements<TableCell>().ElementAt(1).GetCellText();

                    if (varname.Equals("VarName"))
                        continue;

                    double weight = 0;
                    if (!double.TryParse(row.Elements<TableCell>().Last().GetCellText(), out weight))
                        continue;
                    
                    LinkedQuestion q2 = CurrentTiming.Questions.SingleOrDefault(x => x.VarName.RefVarName.Equals(varname));

                    if (q2 != null && q2.Weight.Source != "A")
                    {
                        q2.Weight = new Weight(weight, source);
                        count++;
                    }

                   
                }

                message = count + " weights imported from " + filePath.Substring(filePath.LastIndexOf("\\") + 1) + "\r\n";
                txtMessages.Text += message;

            }
        
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

        private DataTable WholeSurveyTimingTable(List<LinkedQuestion> questionList)
        {
            // create the data table
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("Qnum"));
            dt.Columns.Add(new DataColumn("VarName"));
            dt.Columns.Add(new DataColumn("VarLabel"));
            dt.Columns.Add(new DataColumn("Question"));
            dt.Columns.Add(new DataColumn("Filters"));
            dt.Columns.Add(new DataColumn("Word Count"));
            dt.Columns.Add(new DataColumn("Weight"));
            dt.Columns.Add(new DataColumn("Weight Source"));
            dt.Columns.Add(new DataColumn("Weighted Time"));

            double totalTime = 0;
            double totalWeightedTime = 0;
            double wordCount = 0;
          
            foreach (LinkedQuestion lq in questionList)
            {
                // construct the row
                DataRow r = dt.NewRow();

                // basic info
                r["Qnum"] = lq.Qnum;
                r["VarName"] = lq.VarName.RefVarName;
                r["VarLabel"] = "<strong><em>" + lq.VarName.VarLabel + "</em></strong>\r\n" + lq.RespOptions + "\r\n" + lq.NRCodes;
                r["Question"] = lq.GetQuestionText();

                string filterExpList = "";

                Dictionary<string, string> filterExpressions = new Dictionary<string, string>();
                foreach (List<FilterInstruction> list in lq.FilterList)
                {
                    foreach (FilterInstruction fi in list)
                    {
                        LinkedQuestion found = CurrentTiming.QuestionAt(fi.VarName);
                        if (found == null)
                            continue;
                        if (!filterExpressions.ContainsKey(fi.VarName.ToLower()))
                            filterExpressions.Add(found.VarName.RefVarName.ToLower(), "<strong>" + fi.FilterExpression + "</strong>" + "\r\n" + found.VarName.VarLabel + "\r\n" + found.RespOptions);
                    }

                }

                foreach (string f in filterExpressions.Values)
                {
                    if (filterExpList.Contains(f))
                        continue;

                    filterExpList += f + "\r\n";
                }


                r["Filters"] = filterExpList;
                r["Word Count"] = lq.WordCount();
                

                // timing info
                double time = lq.GetTiming(CurrentTiming.WPM, CurrentTiming.SmartWordCount, CurrentTiming.IncludeNotes);
                double weightedTime;
                double weight = lq.Weight.Value;
                double words = lq.WordCount();



                if (!CurrentTiming.IsForTiming(lq))
                    time = 0;
                else
                    wordCount += words;

                totalTime += time;
                

                if (weight < 0)
                {
                    weightedTime = 0;
                    totalWeightedTime += 0;
                }
                else
                {
                    weightedTime = time * weight;
                    totalWeightedTime += time * weight;
                }

                r["Weight"] = weight;
                r["Weight Source"] = lq.Weight.Source;
                r["Weighted Time"] = weightedTime.ToString("N2");
                 

                dt.Rows.Add(r);
            }

            DataRow total = dt.NewRow();

            total["VarName"] = "Total";
            total["Weighted Time"] = ((double)totalWeightedTime / 60).ToString("N2") + " mins";
            total["Word Count"] = wordCount;
                
            dt.Rows.Add(total);

            DataRow total2 = dt.NewRow();

            total2["VarName"] = "Total";
            total2["VarLabel"] = "WPM:" + CurrentTiming.WPM;
            total2["Weighted Time"] = (double)totalWeightedTime / 60 + " mins";
            total2["Word Count"] = wordCount;

            dt.Rows.InsertAt(total2, 0);

            CurrentTiming.TotalTime = (double)totalTime / 60;
            CurrentTiming.TotalWeightedTime = (double)totalWeightedTime / 60;

            return dt;
        }

        /// <summary>
        /// Generates a table with all questions in the survey. Includes columns for DSVs and filter lists
        /// </summary>
        /// <param name="questionList"></param>
        /// <param name="removeNoFilter"></param>
        /// <returns></returns>
        private DataTable MissingWeightsReport(List<LinkedQuestion> questionList, bool removeNoFilter = true)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("Qnum"));
            dt.Columns.Add(new DataColumn("VarName"));

            dt.Columns.Add(new DataColumn("Question"));
            dt.Columns.Add(new DataColumn("VarLabel"));
           
            dt.Columns.Add(new DataColumn("Filters"));
            dt.Columns.Add(new DataColumn("Weight"));

            foreach (LinkedQuestion lq in questionList)
            {
                if (lq.Weight.Value > -1)
                    continue;

                
                string filterExpList = "";

                Dictionary<string, string> filterExpressions = new Dictionary<string, string>();
                foreach (List<FilterInstruction> list in lq.FilterList )
                {
                    foreach (FilterInstruction fi in list)
                    {
                        LinkedQuestion found = CurrentTiming.QuestionAt(fi.VarName);
                        if (found == null)
                            continue;
                        if (!filterExpressions.ContainsKey(fi.VarName.ToLower()))
                            filterExpressions.Add(found.VarName.RefVarName.ToLower(), "<strong>" + fi.FilterExpression + "</strong>" + "\r\n" + found.VarName.VarLabel + "\r\n" + found.RespOptions);
                    }

                }

                foreach (string f in filterExpressions.Values)
                {
                    if (filterExpList.Contains(f))
                        continue;

                    filterExpList += f + "\r\n";
                }

                DataRow r = dt.NewRow();

                // basic info
                r["Qnum"] = lq.Qnum;
                r["VarName"] = lq.VarName.RefVarName;
                r["Filters"] = filterExpList;
                r["Question"] = lq.GetQuestionText();
                r["VarLabel"] = lq.VarName.VarLabel;
                
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

        private void cmdRefreshLists_Click(object sender, EventArgs e)
        {
            RefreshLists();
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

        private void findFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindFilter frm = new FindFilter(CurrentTiming.Questions);
            frm.Show();
        }

        private void smartWordCountToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            CurrentTiming.SmartWordCount = smartWordCountToolStripMenuItem.Checked;
        }
    }
}

