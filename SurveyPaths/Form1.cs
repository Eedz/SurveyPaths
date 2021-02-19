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
    

    public partial class Form1 : Form
    {
        SurveyTiming CurrentTiming;

        BindingSource bs;
        BindingSource bsRun;

        List<Respondent> RespondentList;

        List<string> CustomList;
        //public string SavedRunLocation = "\\\\psychfile\\psych$\\psych-lab-gfong\\SMG\\Access\\Survey Timing\\Saved Timing Runs";
       // public string WeightLocation = "\\\\psychfile\\psych$\\psych-lab-gfong\\SMG\\Access\\Survey Timing";
        public string TimingFolder = "\\\\psychfile\\psych$\\psych-lab-gfong\\SMG\\Access\\Survey Timing";

        public Form1()
        {
            InitializeComponent();

            bs = new BindingSource();
            bsRun = new BindingSource();
            
            cboSurvey.DataSource = DBAction.GetSurveyList();
            cboSurvey.SelectedItem = null;
            
            cboSurvey.SelectedIndexChanged += cboSurvey_SelectedIndexChanged;       

            CustomList = new List<string>();
            AddRespondents();
            NewTimingRun();

            

            cboTimingScheme.DataSource = Enum.GetValues(typeof(TimingReportType));

            cboSurvey.DataBindings.Add(new Binding("SelectedItem", bsRun, "SurveyCode"));

            cboTimingScheme.DataBindings.Add(new Binding("SelectedItem", bsRun, "ReportType"));

            cboUserType.DataBindings.Add(new Binding("SelectedItem", bsRun, "User"));

            cboMaxMin.DataSource = Enum.GetValues(typeof(TimingType));
            cboMaxMin.DataBindings.Add(new Binding("SelectedItem", bsRun, "TimingPath"));

            txtTimingTitle.DataBindings.Add("Text", bsRun, "Title", true, DataSourceUpdateMode.OnPropertyChanged);
            txtWPM.DataBindings.Add(new Binding("Text", bsRun, "WPM"));
            txtStartAt.DataBindings.Add(new Binding("Text", bsRun, "StartQ"));
            lstResponses.DataSource = new BindingList<Answer>(CurrentTiming.User.Responses);

        }

        

        private void DoCustomList()
        {
            CustomList = new List<string>();
            CustomList.Add("SB205");
            CustomList.Add("SB207");
            CustomList.Add("SB211");
            CustomList.Add("NC912");
            CustomList.Add("CH801");
            CustomList.Add("CH811");
            CustomList.Add("CH813");
            CustomList.Add("CH815");
            CustomList.Add("CH817");
            CustomList.Add("KN211");
            CustomList.Add("KN231");
            CustomList.Add("KN241");
            CustomList.Add("KN246");
            CustomList.Add("KN259");
            CustomList.Add("KN257");
            CustomList.Add("KN221");
            CustomList.Add("KN234");
            CustomList.Add("KN244");
            CustomList.Add("KN248");
            CustomList.Add("WL655");
            CustomList.Add("WL427");
            CustomList.Add("ET115");
            CustomList.Add("ET249");
            CustomList.Add("PS212");
            CustomList.Add("PR218");
            CustomList.Add("NC301");
            //CustomList.Add("NC302");
            CustomList.Add("NC307");
            CustomList.Add("NC317");
            CustomList.Add("EQ321");
            CustomList.Add("EQ412");
            CustomList.Add("NC365");
            CustomList.Add("EC409");
            CustomList.Add("EC386");
            CustomList.Add("EK420");
            CustomList.Add("EK426");
            CustomList.Add("EK427");
            CustomList.Add("EK428");
            CustomList.Add("EK429");
            CustomList.Add("EP308");
            CustomList.Add("EP302");
            CustomList.Add("EP100");
            CustomList.Add("EP450");
            CustomList.Add("EP152");
            CustomList.Add("EP170");
            CustomList.Add("EP341");
            CustomList.Add("EP140");
            CustomList.Add("EP601");
            CustomList.Add("EP608");
            CustomList.Add("EA501");
            CustomList.Add("EK270");
            CustomList.Add("ES322");
            CustomList.Add("RE431");
            CustomList.Add("RE411");
            CustomList.Add("ED508");
            CustomList.Add("RE201");
            CustomList.Add("EI325");
            CustomList.Add("EC718");
            CustomList.Add("EC712");
            CustomList.Add("EC716");
            CustomList.Add("EC710");
            CustomList.Add("EC714");
            CustomList.Add("EC735");
            CustomList.Add("EC720");
            CustomList.Add("EC775");
            CustomList.Add("EC772");
            CustomList.Add("EC779");
            CustomList.Add("EF821");
            CustomList.Add("EC739");
            CustomList.Add("DI260");
            CustomList.Add("DI262");

            


        }

        private void DoCustomList14Oct()
        {
            CustomList = new List<string>();
            CustomList.Add("SB226");

            CustomList.Add("CH977o");
            CustomList.Add("KN451");
            CustomList.Add("KN803");
            CustomList.Add("KN802");
            CustomList.Add("LM365");
            CustomList.Add("LM367");
            CustomList.Add("LM368");
            CustomList.Add("LM369");
            CustomList.Add("LM366");
            CustomList.Add("ET435");
            CustomList.Add("ET437");
            CustomList.Add("ET440");
            CustomList.Add("ET535");
            CustomList.Add("ET537");
            CustomList.Add("ET539");
            CustomList.Add("ET250");
            CustomList.Add("ET252");
            CustomList.Add("ET723");
            CustomList.Add("ET729");
            CustomList.Add("ET724");
            CustomList.Add("ET725");
            CustomList.Add("ET775");
            CustomList.Add("ET728");
            CustomList.Add("ET735");
            CustomList.Add("ET737");
            CustomList.Add("ET759");
            CustomList.Add("ET760");
            CustomList.Add("ET766");
            CustomList.Add("ET767");
            
            CustomList.Add("AD601");
            CustomList.Add("AD606");
            CustomList.Add("PU680");
            CustomList.Add("ES420");
            CustomList.Add("EP102");
            CustomList.Add("EA814");
            CustomList.Add("EA816");
            CustomList.Add("EA810");
            CustomList.Add("EA820");
            CustomList.Add("EA818");

            CustomList.Add("DI712");
            CustomList.Add("DI703");
            CustomList.Add("DI706");

            CustomList.Add("DE403");
            CustomList.Add("DE821");
            CustomList.Add("DE831");
            CustomList.Add("DE826");
            CustomList.Add("DE836");
            CustomList.Add("DE841");
            CustomList.Add("DE846");
            CustomList.Add("DE851");

            CustomList.Add("DE931");
            CustomList.Add("DE932");
            CustomList.Add("DE933");


        }

        private void DoCustomList14OctCombined()
        {
            CustomList = new List<string>();
            CustomList.Add("SB226");

            CustomList.Add("CH977o");
            CustomList.Add("KN451");
            CustomList.Add("KN803");
            CustomList.Add("KN802");
            CustomList.Add("LM365");
            CustomList.Add("LM367");
            CustomList.Add("LM368");
            CustomList.Add("LM369");
            CustomList.Add("LM366");
            CustomList.Add("ET435");
            CustomList.Add("ET437");
            CustomList.Add("ET440");
            CustomList.Add("ET535");
            CustomList.Add("ET537");
            CustomList.Add("ET539");
            CustomList.Add("ET250");
            CustomList.Add("ET252");
            CustomList.Add("ET723");
            CustomList.Add("ET729");
            CustomList.Add("ET724");
            CustomList.Add("ET725");
            CustomList.Add("ET775");
            CustomList.Add("ET728");
            CustomList.Add("ET735");
            CustomList.Add("ET737");
            CustomList.Add("ET759");
            CustomList.Add("ET760");
            CustomList.Add("ET766");
            CustomList.Add("ET767");

            CustomList.Add("AD601");
            CustomList.Add("AD606");
            CustomList.Add("PU680");
            CustomList.Add("ES420");
            CustomList.Add("EP102");
            CustomList.Add("EA814");
            CustomList.Add("EA816");
            CustomList.Add("EA810");
            CustomList.Add("EA820");
            CustomList.Add("EA818");

            CustomList.Add("DI712");
            CustomList.Add("DI703");
            CustomList.Add("DI706");

            CustomList.Add("DE403");
            CustomList.Add("DE821");
            CustomList.Add("DE831");
            CustomList.Add("DE826");
            CustomList.Add("DE836");
            CustomList.Add("DE841");
            CustomList.Add("DE846");
            CustomList.Add("DE851");

            CustomList.Add("DE931");
            CustomList.Add("DE932");
            CustomList.Add("DE933");

            CustomList.Add("SB205");
            CustomList.Add("SB207");
            CustomList.Add("SB211");
            CustomList.Add("NC912");
            CustomList.Add("CH801");
            CustomList.Add("CH811");
            CustomList.Add("CH813");
            CustomList.Add("CH815");
            CustomList.Add("CH817");
            CustomList.Add("KN211");
            CustomList.Add("KN231");
            CustomList.Add("KN241");
            CustomList.Add("KN246");
            CustomList.Add("KN259");
            CustomList.Add("KN257");
            CustomList.Add("KN221");
            CustomList.Add("KN234");
            CustomList.Add("KN244");
            CustomList.Add("KN248");
            CustomList.Add("WL655");
            CustomList.Add("WL427");
            CustomList.Add("ET115");
            CustomList.Add("ET249");
            CustomList.Add("PS212");
            CustomList.Add("PR218");
            CustomList.Add("NC301");
            //CustomList.Add("NC302");
            CustomList.Add("NC307");
            CustomList.Add("NC317");
            CustomList.Add("EQ321");
            CustomList.Add("EQ412");
            CustomList.Add("NC365");
            CustomList.Add("EC409");
            CustomList.Add("EC386");
            CustomList.Add("EK420");
            CustomList.Add("EK426");
            CustomList.Add("EK427");
            CustomList.Add("EK428");
            CustomList.Add("EK429");
            CustomList.Add("EP308");
            CustomList.Add("EP302");
            CustomList.Add("EP100");
            CustomList.Add("EP450");
            CustomList.Add("EP152");
            CustomList.Add("EP170");
            CustomList.Add("EP341");
            CustomList.Add("EP140");
            CustomList.Add("EP601");
            CustomList.Add("EP608");
            CustomList.Add("EA501");
            CustomList.Add("EK270");
            CustomList.Add("ES322");
            CustomList.Add("RE431");
            CustomList.Add("RE411");
            CustomList.Add("ED508");
            CustomList.Add("RE201");
            CustomList.Add("EI325");
            CustomList.Add("EC718");
            CustomList.Add("EC712");
            CustomList.Add("EC716");
            CustomList.Add("EC710");
            CustomList.Add("EC714");
            CustomList.Add("EC735");
            CustomList.Add("EC720");
            CustomList.Add("EC775");
            CustomList.Add("EC772");
            CustomList.Add("EC779");
            CustomList.Add("EF821");
            CustomList.Add("EC739");
            CustomList.Add("DI260");
            CustomList.Add("DI262");
        }

        private void AddRespondents()
        {
            RespondentList = new List<Respondent>();
            RespondentList.Add(UserTypeDefs.CreateBlankRespondent());

            RespondentList.Add(UserTypeDefs.CreateKRA1TripleUser());
            RespondentList.Add(UserTypeDefs.CreateKRA1_SM_EC());
            RespondentList.Add(UserTypeDefs.CreateKRA1_SM_HN());
            RespondentList.Add(UserTypeDefs.CreateKRA1_EC_HN());
            RespondentList.Add(UserTypeDefs.CreateKRA1_HN());
            RespondentList.Add(UserTypeDefs.CreateKRA1_SM());
            RespondentList.Add(UserTypeDefs.CreateKRA1_EC());

            RespondentList.Add(UserTypeDefs.CreateSpanishRespondent());

            RespondentList.Add(UserTypeDefs.CreateESUT1());
            RespondentList.Add(UserTypeDefs.CreateESUT2());
            RespondentList.Add(UserTypeDefs.CreateESUT3());
            RespondentList.Add(UserTypeDefs.CreateESUT4());

            RespondentList.Add(UserTypeDefs.Create6E2UT1());
            RespondentList.Add(UserTypeDefs.Create6E2UT2());
            RespondentList.Add(UserTypeDefs.Create6E2UT3());
            RespondentList.Add(UserTypeDefs.Create6E2UT4());

            RespondentList.Add(UserTypeDefs.CreateNZL3SmokerVaper());
            RespondentList.Add(UserTypeDefs.CreateNZL3NonSmokerVaper());
            RespondentList.Add(UserTypeDefs.CreateNZL3SmokerNonVaper());
            RespondentList.Add(UserTypeDefs.CreateNZL3NonSmokerNonVaper());

            RespondentList.Add(UserTypeDefs.CreateMYS1NonSmokerNonEcig());
            RespondentList.Add(UserTypeDefs.CreateMYS1SmokerCombinedNonEcig());
            RespondentList.Add(UserTypeDefs.CreateMYS1SmokerCombinedEcig());
            RespondentList.Add(UserTypeDefs.CreateMYS1SimpleSmoker());
            RespondentList.Add(UserTypeDefs.CreateMYS1SimpleNonSmoker());
            RespondentList.Add(UserTypeDefs.CreateMYS1UT1());
            RespondentList.Add(UserTypeDefs.CreateMYS1UT2());
            RespondentList.Add(UserTypeDefs.CreateMYS1UT3());

        }

        private void FilterUserTypes()
        {
            if (CurrentTiming.SurveyCode == null)
                cboUserType.DataSource = RespondentList;
            else
            {
                var users = RespondentList.Where(x => x.Survey.Equals(CurrentTiming.SurveyCode) || x.Survey.Equals("")).ToList();
                if (users.Count() == 0)
                    cboUserType.DataSource = RespondentList.Where(x => x.Survey.Equals("")).ToList();
                else
                    cboUserType.DataSource = users;
            }
                
        }

        private void NewTimingRun()
        {
            CurrentTiming = new SurveyTiming();
            //CurrentTiming.StartQ = 53;//es2.5
            //CurrentTiming.StartQ = 65;//es2.5
            CurrentTiming.ReportType = TimingReportType.TimingWholeSurvey;
            
            cboSurvey.SelectedItem = "NZL3.5";
             
            bsRun.DataSource = CurrentTiming;

            RefreshCurrentRecord();
        }

        /// <summary>
        /// Change the current survey. Get a new set of questions.
        /// </summary>
        /// <param name="survey"></param>
        private void ChangeSurvey(string surveyCode)
        {
            CurrentTiming.SurveyCode = surveyCode;
            Survey survey = DBAction.GetSurveyInfo(surveyCode);
            var qs = DBAction.GetSurveyQuestions(survey).ToList();

            // populate user type list
            FilterUserTypes();

            CurrentTiming.Questions.Clear();
            
            foreach (SurveyQuestion q in qs)
            {
                // include only custom list
                //if (CustomList.Count != 0 && !CustomList.Any(x => x.Equals(q.VarName.RefVarName)))
                //    continue;

                //// exclude custom list
                //if (CustomList.Count != 0 && CustomList.Any(x => x.Equals(q.VarName.RefVarName)))
                //    continue;

                if (IsOtherSpecify(qs, q))
                    continue;

                CurrentTiming.Questions.Add(new LinkedQuestion(q));
            }
            
            PopulateNextQuestionsString();
            PopulateFilters();

            bs.DataSource = CurrentTiming.Questions;
            
            bindingNavigator1.BindingSource = bs;
            bs.CurrentChanged += Bs_CurrentChanged;

            txtVarName.DataBindings.Clear();
            txtVarName.DataBindings.Add("Text", bs, "VarName.FullVarName");

            txtQnum.DataBindings.Clear();
            txtQnum.DataBindings.Add("Text", bs, "Qnum");

            txtWeight.DataBindings.Clear();
            txtWeight.DataBindings.Add("Text", bs, "Weight.Value");

            

            UpdateResponses();
            UpdateWeights();
            UpdateTiming();
            UpdateVarList();
        }

        /// <summary>
        /// Update the list of responses with those stored in the Respondent .
        /// </summary>
        private void UpdateResponses()
        {
            
            lstResponses.DataSource = new BindingList<Answer>(CurrentTiming.User.Responses);
            
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

        private void UpdateWeights(bool overrideStartQ = false)
        {
            if (CurrentTiming.Questions == null) return;
            lstWeightedQuestionList.Items.Clear();
            lstWeightedQuestionList.View = System.Windows.Forms.View.Details;

            lstUnweightedQuestionList.Items.Clear();
            lstUnweightedQuestionList.View = System.Windows.Forms.View.Details;

            List<LinkedQuestion> list;

            if (CurrentTiming.ReportType == TimingReportType.TimingUser && CurrentTiming.UserQuestions.Count() > 0)
                list = CurrentTiming.UserQuestions;
            else
                list = CurrentTiming.Questions;

            foreach (LinkedQuestion lq in list)
            {

                if (lq.PreP.StartsWith("Ask all."))
                {
                    lq.Weight.Source = "A";
                    lq.Weight.Value = 1;
                }

                if (lq.VarName.RefVarName.StartsWith("Z"))
                {
                    lq.Weight.Value = 0;
                    lq.Weight.Source = "A";
                }

                if (lq.IsDerived())
                {
                    lq.Weight.Value = 0;
                    lq.Weight.Source = "A";
                }

                if (lq.VarName.RefVarName.StartsWith("BI9"))
                {
                    lq.Weight.Value = 0;
                    lq.Weight.Source = "A";
                }

                ListViewItem li = new ListViewItem(new string[] { lq.Qnum, lq.VarName.RefVarName, lq.VarName.VarLabel, lq.Weight.Value.ToString(), lq.Weight.Source, lq.WordCount().ToString(), lq.GetTiming(CurrentTiming.WPM).ToString() });
                li.Tag = lq;

                if (lq.Weight.Value == -1)
                    lstUnweightedQuestionList.Items.Add(li);
                else
                    lstWeightedQuestionList.Items.Add(li);



                FormatListItem(li, GetQuestionType(li));
            }

            lblMissingWeights.Text = "Missing weights: " + lstUnweightedQuestionList.Items.Count;
        }

        private void RefreshLists()
        {
            if (CurrentTiming.Questions == null) return;

            lstWeightedQuestionList.Items.Clear();
            lstWeightedQuestionList.View = System.Windows.Forms.View.Details;

            lstUnweightedQuestionList.Items.Clear();
            lstUnweightedQuestionList.View = System.Windows.Forms.View.Details;

            List<LinkedQuestion> list;

            if (CurrentTiming.ReportType == TimingReportType.TimingUser)
            {
                list = CurrentTiming.UserQuestions;

                foreach (LinkedQuestion lq in list)
                {

                    ListViewItem li = new ListViewItem(new string[] { lq.Qnum, lq.VarName.RefVarName, lq.VarName.VarLabel, lq.Weight.Value.ToString(), lq.Weight.Source, lq.WordCount().ToString() });
                    li.Tag = lq;

                    lstWeightedQuestionList.Items.Add(li);

                    FormatListItem(li, GetQuestionType(li));
                }
                lstUnweightedQuestionList.Visible = false;
                lblMissingWeights.Visible = false;
            }
            else
            {
                list = CurrentTiming.Questions;

                foreach (LinkedQuestion lq in list)
                {

                    ListViewItem li = new ListViewItem(new string[] { lq.Qnum, lq.VarName.RefVarName, lq.VarName.VarLabel, lq.Weight.Value.ToString(), lq.Weight.Source, lq.WordCount().ToString() });
                    li.Tag = lq;

                    if (lq.Weight.Value == -1)
                        lstUnweightedQuestionList.Items.Add(li);
                    else
                        lstWeightedQuestionList.Items.Add(li);


                    FormatListItem(li, GetQuestionType(li));
                }

                lblMissingWeights.Text = "Missing weights: " + lstUnweightedQuestionList.Items.Count;
            }
        }

        private void UpdateTiming()
        {
            if (CurrentTiming.WPM < 50)
                return;

            double totalTime;
            lblTotalTime.Text = "";
            lblTotalTime.Text = "Questions: " + CurrentTiming.Questions.Count();

            totalTime = CurrentTiming.GetTiming(CurrentTiming.WPM);

            if (chkWeightedToggle.Checked)
            {
                for (int i = CurrentTiming.WPM - 50; i < CurrentTiming.WPM; i += 10)
                {
                    lblTotalTime.Text += "\r\nTime (" + i + " WPM): " + CurrentTiming.GetWeightedTiming(i).ToString("N2") + " mins weighted";
                }

                lblTotalTime2.Text = "Time (" + CurrentTiming.WPM + " WPM): " + CurrentTiming.GetWeightedTiming(CurrentTiming.WPM).ToString("N2") + " mins  weighted";

                lblTotalTime3.Text = "";
                for (int i = CurrentTiming.WPM + 10; i <= CurrentTiming.WPM + 50; i += 10)
                {
                    lblTotalTime3.Text += "Time (" + i + " WPM): " + CurrentTiming.GetWeightedTiming(i).ToString("N2") + " mins weighted\r\n";
                }

                if (!string.IsNullOrEmpty(txtTargetTime.Text))
                {
                    txtTargetTimeWPM.Text = CurrentTiming.GetTargetWPM(Double.Parse(txtTargetTime.Text)).ToString("N2");
                }
            }
            else
            {
                for (int i = CurrentTiming.WPM - 50; i < CurrentTiming.WPM; i += 10)
                {
                    lblTotalTime.Text += "\r\nTime (" + i + " WPM): " + CurrentTiming.GetTiming(i).ToString("N2") + " mins";
                }

                lblTotalTime2.Text = "Time (" + CurrentTiming.WPM + " WPM): " + totalTime.ToString("N2") + " mins";

                lblTotalTime3.Text = "";
                for (int i = CurrentTiming.WPM + 10; i <= CurrentTiming.WPM + 50; i += 10)
                {
                    lblTotalTime3.Text += "Time (" + i + " WPM): " + CurrentTiming.GetTiming(i).ToString("N2") + " mins\r\n";
                }

                if (!string.IsNullOrEmpty(txtTargetTime.Text))
                {
                    txtTargetTimeWPM.Text = CurrentTiming.GetTargetWPM(Double.Parse(txtTargetTime.Text)).ToString("N2");
                }
            }

            

            CurrentTiming.TotalTime = totalTime;
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
                    LinkedQuestion next = CurrentTiming.Questions.Find(x => x.VarName.FullVarName.Equals(s));

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
                    LinkedQuestion next = CurrentTiming.Questions.Find(x => x.VarName.FullVarName.Equals(s));

                    q.PossibleNext.Add(i, next);
                    i++;
                }
            }
        }
        #endregion

        #region PreP Analysis
        /// <summary>
        /// For each question, get any VarNames appearing in the PreP and then get a reference to that question in the master list and
        /// add this reference to the list of filter questions.
        /// </summary>
        private void PopulateFilters()
        {
            // build a list of non-standard VarNames in the survey. These may appear in filters.
            List<string> nonStd = new List<string>();
            foreach (LinkedQuestion q in CurrentTiming.Questions)
            {
                if (!Regex.IsMatch(q.VarName.RefVarName, "[A-Z][A-Z][0-9][0-9][0-9][a-z]*"))
                    nonStd.Add(q.VarName.RefVarName);
            }

            foreach (LinkedQuestion q in CurrentTiming.Questions)
            {
                string prep = q.PreP;
                //if (q.PreP.StartsWith("Ask if any of"))
                //{
                //    prep = q.ExpandPreP();
                //}

                // get the list of filter instructions for this question (both standard and non-standard VarNames)
                var filters = q.GetFilterInstructions();
                filters.AddRange(q.GetFilterInstructions(nonStd));

                // move on if there are no filters
                if (filters.Count == 0)
                    continue;

                foreach ( FilterInstruction fi in filters)
                {
                    if (fi.VarName.Equals(q.VarName.RefVarName))
                        continue;

                    // add the SurveyQuestion represented by this filter instruction to the list of FilteredOn questions
                    LinkedQuestion filterVar = CurrentTiming.Questions.Find(x => x.VarName.RefVarName.Equals(fi.VarName));
                    if (filterVar != null)
                    {
                        q.FilteredOn.Add(filterVar);
                    }
                }
                
                List<string> scenarios = new List<string>();

                // loop through filter instructions
                // if found in prep, replace with a letter until we are reduced to the form A AND B OR C 
                // add each filter instruction to the dictionary with its letter as the key
                // now go through the prep and try to create scenarios using the letters and AND/OR conditions
                // 
                // after the scenarios are constructed, replace the keys with the values

                
                prep = prep.Replace(" OR ", " or ");
                prep = prep.Replace("[", "(");
                prep = prep.Replace("]", ")");
                prep = prep.Replace("<>", "!");

                Dictionary<string, FilterInstruction> filterLookup = new Dictionary<string, FilterInstruction>();
                int c = 65;
                foreach (FilterInstruction f in filters)
                {
                    if (f.AnyOf)
                    {

                    }
                    

                    // TODO some filters are contained in others
                    //if (prep.Contains("(" + f.ToString() + ")"))
                    //{

                    prep = Utilities.ReplaceFirstOccurrence(prep, f.ToString().Replace("<>", "!"), "$" + ((char)c).ToString() + "$");
                    //}
                    //else
                    //{                    
                    //    prep = Utilities.ReplaceFirstOccurrence(prep, f.ToString(), "($" + ((char)c).ToString() + "$)");
                    //}
                    
                    filterLookup.Add("$" + ((char)c).ToString() + "$", f);
                    c++;
                }

                string[] prepLines = prep.Split(new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in prepLines)
                {
                    if (!line.StartsWith("Ask if"))
                        continue;

                    try
                    { 
                        List<Token> tokens = GetTokens(line);

                        List<Token> postFix = PostFixUtils.infixToPostfixList(tokens);
                        List<string> cases = new List<string>();
                    
                        cases = PostFixUtils.GetCases( PostFixUtils.getInfixCases(postFix));

                        

                        foreach (string ca in cases)
                        {
                            List<FilterInstruction> l = new List<FilterInstruction>();

                            // extract the keys from ca and add to list
                            foreach (KeyValuePair<string, FilterInstruction> p in filterLookup)
                            {
                                if (ca.Contains(p.Key))
                                    l.Add(p.Value);
                            }

                            q.FilterList.Add(l);
                        }
                    }
                    catch (Exception)
                    {
                        int i = 0;
                    }
                }


                if (q.FilteredOn.Count() > 0 && q.FilterList.Count == 0)
                    CurrentTiming.NotInterpreted.Add(q);
                else
                {

                    bool found = false;
                    foreach (LinkedQuestion fo in q.FilteredOn)
                    {
                        // if prep has a varname
                        // but there is no scenario for it, add to uninterpreted list
                        foreach (List<FilterInstruction> fi in q.FilterList)
                        {
                            foreach (FilterInstruction f in fi)
                            {
                                if (f.VarName.Equals(fo.VarName.RefVarName))
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (found) break;
                        }


                    }
                    if (!found)
                        CurrentTiming.NotInterpreted.Add(q);
                }
            }

            
        }


        private List<Token> GetTokens(string input)
        {
            Tokenizer tk = new Tokenizer(input);

            List<Token> tokens = tk.Tokenize().ToList();
            tokens.RemoveAll(item => item == null);

            // TODO check final joined string for these:
                        
            // contains ()
            // contains (or or)
            // contains (and and)

            if (tokens.Count == 0)
                return tokens;

            string tokenString = string.Join("", tokens);          

            bool invalidStart = false; bool invalidEnd = false; bool emptyParens = false;
            bool badOR = false, badAND = false;
            bool toFix;

            invalidStart = (tokenString.StartsWith("or") || tokenString.StartsWith("and") || tokenString.StartsWith(")"));

            invalidEnd = (tokenString.EndsWith("or") || tokenString.EndsWith("and") || tokenString.EndsWith("("));

            emptyParens = (tokenString.Contains("()"));

            badOR = (tokenString.Contains("(or") || tokenString.Contains("or)") || tokenString.Contains("oror"));
            badAND = (tokenString.Contains("(and") || tokenString.Contains("and)") || tokenString.Contains("andand"));

            toFix = invalidStart || invalidEnd || emptyParens || badOR || badAND;

            while (toFix)
            {
                // starts with OR AND closed bracket
                while (tokenString.StartsWith("or") || tokenString.StartsWith("and") || tokenString.StartsWith(")"))
                {
                    tokens[0] = null;
                    tokens.RemoveAll(item => item == null);
                    tokenString = string.Join("", tokens);
                }

                // ends with OR AND open bracket
                while (tokenString.EndsWith("or") || tokenString.EndsWith("and") || tokenString.EndsWith("("))
                {
                    tokens[tokens.Count - 1] = null;
                    tokens.RemoveAll(item => item == null);
                    tokenString = string.Join("", tokens);
                }

                // contains empty parentheses
                while (tokenString.Contains("()"))
                {
                    for (int i = 0; i < tokens.Count; i++)
                    {
                        if (tokens[i] is OpenParenthesisToken && tokens[i + 1] is ClosedParenthesisToken)
                        {
                            tokens[i] = null;
                            tokens[i + 1] = null;
                        }
                    }
                    tokens.RemoveAll(item => item == null);
                    tokenString = string.Join("", tokens);
                }

                
                for (int i = 0; i < tokens.Count; i++)
                {
                    if (tokens[i] is OrToken)
                    {
                        if (i > 0 && tokens[i - 1] is OpenParenthesisToken && tokens[i + 1] is ClosedParenthesisToken)
                        {
                            tokens[i - 1] = null;
                            tokens[i + 1] = null;
                            tokens[i] = null;
                        }
                        else if (i < tokens.Count - 1 && tokens[i + 1] is ClosedParenthesisToken)
                        {
                            tokens[i] = null;
                        }
                        else if (i > 0 && tokens[i - 1] is OpenParenthesisToken)
                        {
                            tokens[i] = null;
                        } else if (i < tokens.Count - 1 && tokens[i + 1] is OrToken)
                        {
                            tokens[i + 1] = null;
                        }

                    }
                }
                
                tokens.RemoveAll(item => item == null);
                tokenString = string.Join("", tokens);

                for (int i = 0; i < tokens.Count; i++)
                {
                    if (tokens[i] is AndToken)
                    {
                        if (i > 0 && tokens[i - 1] is OpenParenthesisToken && tokens[i + 1] is ClosedParenthesisToken)
                        {
                            tokens[i - 1] = null;
                            tokens[i + 1] = null;
                            tokens[i] = null;
                        }
                        else if (i < tokens.Count - 1 && tokens[i + 1] is ClosedParenthesisToken)
                        {
                            tokens[i] = null;
                        }
                        else if (i > 0 && tokens[i - 1] is OpenParenthesisToken)
                        {
                            tokens[i] = null;
                        }
                        else if (i < tokens.Count - 1 && tokens[i + 1] is AndToken)
                        {
                            tokens[i + 1] = null;
                        }
                    }
                }
                tokens.RemoveAll(item => item == null);
                tokenString = string.Join("", tokens);

                while (Regex.IsMatch(tokenString, "\\(\\$[A-Z]\\$\\)"))
                {
                    // remove brackets around a variable ($A$)
                    for (int i = 0; i < tokens.Count; i++)
                    {
                        if (tokens[i] is VariableToken)
                        {
                            if (i > 0 && tokens[i - 1] is OpenParenthesisToken && tokens[i + 1] is ClosedParenthesisToken)
                            {
                                tokens[i - 1] = null;
                                tokens[i + 1] = null;

                            }
                        }

                    }
                    tokens.RemoveAll(item => item == null);
                    tokenString = string.Join("", tokens);
                }


                invalidStart = (tokenString.StartsWith("or") || tokenString.StartsWith("and") || tokenString.StartsWith(")"));
                invalidEnd = (tokenString.EndsWith("or") || tokenString.EndsWith("and") || tokenString.EndsWith("("));
                emptyParens = (tokenString.Contains("()"));

                badOR = (tokenString.Contains("(or") || tokenString.Contains("or)") || tokenString.Contains("oror"));
                badAND = (tokenString.Contains("(and") || tokenString.Contains("and)") || tokenString.Contains("andand"));

                toFix = invalidStart || invalidEnd || emptyParens || badOR || badAND;
            }

            return tokens;
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
            List<LinkedQuestion> list = (List<LinkedQuestion>)bs.List;
            ExportToXML(list);
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

            CurrentTiming = LoadXMLFile(File.ReadAllText(filePath));
            
            bsRun.DataSource = CurrentTiming;

            bsRun.ResetBindings(true);
            RefreshLists();
            UpdateTiming();
        }

        // View
        private void filterTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LinkedQuestion current = (LinkedQuestion)bs.Current;
            Form2 frm = new Form2(current, ViewBy.Filters);
            frm.Visible = true;
        }

        private void routingTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LinkedQuestion current = (LinkedQuestion)bs.Current;
            Form2 frm = new Form2(current, ViewBy.Routing);
            frm.Visible = true;
        }


        // Weights
        private void generateSASCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //GenerateSAS frm = new GenerateSAS();

            //frm.ShowDialog();

            //if (frm.DialogResult == DialogResult.OK)
            //{
            //    string sas = GenerateSASCodeOverallFreq(frm.Wave.ISO_Code, frm.Wave.WaveCode);
            //    string filename = CurrentTiming.SurveyCode + "-FrequencyCode.txt";
            //    File.WriteAllText(filename, sas);
                
            //    System.Diagnostics.Process.Start(filename);
            //}

            
            

        }

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
                //UpdateWeights();
                UpdateTiming();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error importing weight. Ensure that the file is formatted correctly.");
            }

        }

        private void assign1ToMissingWeightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (LinkedQuestion lq in CurrentTiming.Questions)
            {
                if (lq.Weight.Value == -1)
                {
                    lq.Weight.Value = 1;
                }

            }

            UpdateWeights();
        }

        private void assign1ToScreeningSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (LinkedQuestion lq in CurrentTiming.Questions)
            {
                if (lq.GetQnumValue() < CurrentTiming.StartQ && !lq.IsDerived())
                {
                    lq.Weight.Value = 1;
                }

            }

            UpdateWeights(true);
        }

        // Reports
        private void dSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OutputReport(Report(CurrentTiming.Questions, false), CurrentTiming.SurveyCode + " - DSV Report");
        }
         
        private void comparisonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CompareUsers frm = new CompareUsers(RespondentList);

            frm.ShowDialog();

            Respondent usertype1, usertype2;
            TimingType path1, path2;

            if (frm.DialogResult == DialogResult.OK)
            {
                usertype1 = frm.user1;
                usertype2 = frm.user2;

                path1 = frm.user1Path;
                path2 = frm.user2Path;
                
            }
            else
            {
                return;
            }

            // compare USER type 1's list of questions with USERTYPE2



            CurrentTiming.TimingPath = path1;
            if (path1 == TimingType.Max)
                usertype1 = CurrentTiming.GetMaxUser(usertype1);
            else if (path1 == TimingType.Min)
                usertype1 = CurrentTiming.GetMinUser(usertype1);
            
            CurrentTiming.User = usertype1;

            List<LinkedQuestion> tripleList = CurrentTiming.GetRespondentQuestions(usertype1);

            CurrentTiming.TimingPath = path2;
            if (path2 == TimingType.Max)
                usertype2 = CurrentTiming.GetMaxUser(usertype2);
            else if (path2 == TimingType.Min)
                usertype2 = CurrentTiming.GetMinUser(usertype2);
          
            CurrentTiming.User = usertype2;

            List<LinkedQuestion> cigHTPList = CurrentTiming.GetRespondentQuestions(usertype2);

            
           

            // get questions for both

            

            ReportSurvey survey1 = new ReportSurvey(usertype1.Description);

            foreach (LinkedQuestion q in tripleList)
            {
                q.VarName.FullVarName = Utilities.RemoveHighlightTags(q.VarName.FullVarName);
                
                SurveyQuestion newQ = new SurveyQuestion();

                newQ.SurveyCode = q.SurveyCode;
                newQ.VarName.FullVarName = q.VarName.FullVarName;
                newQ.VarName.RefVarName = q.VarName.RefVarName;
                newQ.Qnum = q.Qnum;
                newQ.PreP = q.PreP;
                newQ.PreI = q.PreI;
                newQ.PreA = q.PreA;
                newQ.LitQ = q.LitQ;
                newQ.PstI = q.PstI;
                newQ.PstP = q.PstP;
                newQ.RespName = q.RespName;
                newQ.RespOptions = q.RespOptions;
                newQ.NRName = q.NRName;
                newQ.NRCodes = q.NRCodes;
                
                survey1.Questions.Add(newQ);
            }

            survey1.VarLabelCol = true;
            

            ReportSurvey survey2 = new ReportSurvey(usertype2.Description);
            foreach (LinkedQuestion q in cigHTPList)
            {

                q.VarName.FullVarName = Utilities.RemoveHighlightTags(q.VarName.FullVarName);

                SurveyQuestion newQ = new SurveyQuestion();

                newQ.SurveyCode = q.SurveyCode;
                newQ.VarName.FullVarName = q.VarName.FullVarName;
                newQ.VarName.RefVarName = q.VarName.RefVarName;
                newQ.Qnum = q.Qnum;
                newQ.PreP = q.PreP;
                newQ.PreI = q.PreI;
                newQ.PreA = q.PreA;
                newQ.LitQ = q.LitQ;
                newQ.PstI = q.PstI;
                newQ.PstP = q.PstP;
                newQ.RespName = q.RespName;
                newQ.RespOptions = q.RespOptions;
                newQ.NRName = q.NRName;
                newQ.NRCodes = q.NRCodes;

                survey2.Questions.Add(newQ);

            }

            

            survey2.VarLabelCol = true;

            DoCompareReport(survey1, survey2, usertype1.Description.Replace("/", "-") + " vs. " + usertype2.Description.Replace("/", "-") + " Comparison", true);

            ReportSurvey survey1skipped = new ReportSurvey(usertype1.Description + " skipped");
            ReportSurvey survey2skipped = new ReportSurvey(usertype2.Description + " skipped");

            foreach (Answer a in usertype1.Responses)
            {
                if (!a.Skipped)
                    continue;

                var skipped = CurrentTiming.Questions.FirstOrDefault(x => x.VarName.RefVarName.Equals(a.VarName));

                if (skipped != null)
                {
                    SurveyQuestion newQ = new SurveyQuestion();

                    newQ.SurveyCode = skipped.SurveyCode;
                    newQ.VarName.FullVarName = skipped.VarName.FullVarName;
                    newQ.VarName.RefVarName = skipped.VarName.RefVarName;
                    newQ.Qnum = skipped.Qnum;
                    newQ.PreP = skipped.PreP;
                    newQ.PreI = skipped.PreI;
                    newQ.PreA = skipped.PreA;
                    newQ.LitQ = skipped.LitQ;
                    newQ.PstI = skipped.PstI;
                    newQ.PstP = skipped.PstP;
                    newQ.RespName = skipped.RespName;
                    newQ.RespOptions = skipped.RespOptions;
                    newQ.NRName = skipped.NRName;
                    newQ.NRCodes = skipped.NRCodes;

                    survey1skipped.Questions.Add(newQ);
                }

            }

            foreach (Answer a in usertype2.Responses)
            {
                if (!a.Skipped)
                    continue;

                var skipped = CurrentTiming.Questions.FirstOrDefault(x => x.VarName.RefVarName.Equals(a.VarName));

                if (skipped != null)
                {
                    SurveyQuestion newQ = new SurveyQuestion();

                    newQ.SurveyCode = skipped.SurveyCode;
                    newQ.VarName.FullVarName = skipped.VarName.FullVarName;
                    newQ.VarName.RefVarName = skipped.VarName.RefVarName;
                    newQ.Qnum = skipped.Qnum;
                    newQ.PreP = skipped.PreP;
                    newQ.PreI = skipped.PreI;
                    newQ.PreA = skipped.PreA;
                    newQ.LitQ = skipped.LitQ;
                    newQ.PstI = skipped.PstI;
                    newQ.PstP = skipped.PstP;
                    newQ.RespName = skipped.RespName;
                    newQ.RespOptions = skipped.RespOptions;
                    newQ.NRName = skipped.NRName;
                    newQ.NRCodes = skipped.NRCodes;

                    survey2skipped.Questions.Add(newQ);
                }

            }

            DoCompareReport(survey1skipped, survey2skipped, usertype1.Description.Replace("/", "-") + " vs. " + usertype2.Description.Replace("/", "-") + " Comparison (skipped)", false);
        }

        private void DoCompareReport(ReportSurvey s1, ReportSurvey s2, string filename, bool hideIdentical)
        {
            
            SurveyReport SR = new SurveyReport();

            SR.FileName = "\\\\psychfile\\psych$\\psych-lab-gfong\\SMG\\Access\\Reports\\ISR\\";
            string customFileName = filename;

            SR.AddSurvey(s1);
            SR.AddSurvey(s2);
            SR.ShowQuestion = false;
            SR.SurveyCompare.HideIdenticalQuestions = hideIdentical;
            SR.SurveyCompare.ReInsertDeletions = true;
            SR.GenerateReport();
            SR.OutputReportTableXML(customFileName);
        }

        private void currentTimingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Respondent r;
            TimingType path;


            if (CurrentTiming.ReportType == TimingReportType.TimingWholeSurvey)
            {
                CurrentTiming.TimingPath = TimingType.Undefined;
                CurrentTiming.User = new Respondent();
               
            }
            else if (CurrentTiming.ReportType == TimingReportType.TimingUser)
            {

                path = GetMinMax();

                if (path == TimingType.Max)
                    r = CurrentTiming.GetMaxUser(GetCurrentRespondent());
                else if (path == TimingType.Min)
                    r = CurrentTiming.GetMinUser(GetCurrentRespondent());
                else
                    r = GetCurrentRespondent();

                CurrentTiming.TimingPath = path;
                CurrentTiming.User = r;

                CurrentTiming.UserQuestions = CurrentTiming.GetRespondentQuestions(r, true);
                
            }
            else if (CurrentTiming.ReportType == TimingReportType.Undefined)
            {
                MessageBox.Show("Invalid Timing Scheme.");
                return;
            }
            else 
            {
                r = GetCurrentRespondent();
                CurrentTiming.User = GetCurrentRespondent();
                CurrentTiming.UserQuestions = CurrentTiming.GetRespondentQuestions(r);
               
            }

            

            DataTable report;
            string fileName;
            switch (CurrentTiming.ReportType)
            {
                case TimingReportType.TimingUser:
                    report = ReportTimingTable(CurrentTiming.User, CurrentTiming.UserQuestions, false);
                    fileName = CurrentTiming.SurveyCode + " - " + CurrentTiming.User.Description + " - " + CurrentTiming.TimingPath + " - Timing Report (" + CurrentTiming.User.WeightedTime.ToString("N2") + ", " + CurrentTiming.WPM + "wpm)";
                    break;
                case TimingReportType.TimingWholeSurvey:
                    report = WholeSurveyTimingTable(CurrentTiming.User, CurrentTiming.Questions);
                    fileName = CurrentTiming.SurveyCode + " - Whole Survey Timing Report (" + CurrentTiming.User.WeightedTime.ToString("N2") + ", " + CurrentTiming.WPM + "wpm)";
                    break;
                default:
                    report = new DataTable();
                    fileName = "";
                    break;
            }



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

            Survey survey = DBAction.GetSurveyInfo(CurrentTiming.SurveyCode);

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
            LinkedQuestion lq = list.Find(x => x.VarName.FullVarName.Equals(btn.Tag.ToString()));
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



            List<LinkedQuestion> directFilterList = CurrentTiming.GetDirectFilterVarList(CurrentQuestion);
            List<LinkedQuestion> indirectFilterList = new List<LinkedQuestion>();
            if (CurrentQuestion.GetQnumValue() > CurrentTiming.StartQ)
            {
                

                foreach (LinkedQuestion q in directFilterList)
                {
                    indirectFilterList.AddRange(CurrentTiming.GetIndirectFilterVarList(q));
                }
                indirectFilterList = indirectFilterList.Except(directFilterList).ToList();
            }
            

            txtDirectFilters.Text = string.Join("\r\n", directFilterList);
            txtIndirectFilters.Text = string.Join("\r\n", indirectFilterList);
        }

        #endregion

        #region Events

        private void cmdShow_Click(object sender, EventArgs e)
        {
            if (CurrentTiming.TimingPath == TimingType.Max)
            {
                ShowMaxQuestions();
            }
            else if (CurrentTiming.TimingPath == TimingType.Min)
            {
                ShowMinQuestions();
            }
            else if (CurrentTiming.TimingPath == TimingType.Undefined)
            {
                ShowUndefinedQuestions();
            }

            RefreshLists();
        }

        private void cmdTime_Click(object sender, EventArgs e)
        {
            // get survey
            // get user

            CurrentTiming.TimingPath = GetMinMax();
            CurrentTiming.User = GetUser();


            // get timing scheme (by user or whole survey)
            CurrentTiming.ReportType = GetTimingScheme();
            if (CurrentTiming.ReportType != TimingReportType.TimingWholeSurvey)
            {
                List<LinkedQuestion> list = CurrentTiming.GetRespondentQuestions(CurrentTiming.ReportType == TimingReportType.TimingUser);
                CurrentTiming.UserQuestions = list;
            }

            UpdateTiming();
        }

        private void cboSurvey_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSurvey.SelectedItem != null)
                ChangeSurvey((string)cboSurvey.SelectedItem);
        }

        private void cboTimingScheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTimingScheme.SelectedItem == null)
                return;

            TimingReportType scheme = (TimingReportType)cboTimingScheme.SelectedItem;
            //string scheme = (string)cboTimingScheme.SelectedItem;

            switch (scheme)
            {
                case TimingReportType.TimingUser: // "User Based":
                    CurrentTiming.ReportType = TimingReportType.TimingUser;
                    cboUserType.Enabled = true;
                    cboMaxMin.Enabled = true;
                    cmdShow.Enabled = true;
                    cmdAllQuestions.Enabled = true;
                    break;
                case TimingReportType.TimingWholeSurvey: //"Whole Survey":
                case TimingReportType.Undefined:
                    CurrentTiming.ReportType = TimingReportType.TimingWholeSurvey;
                    cboUserType.Enabled = false;
                    cboMaxMin.Enabled = false;
                    cmdShow.Enabled = false ;
                    cmdAllQuestions.Enabled = false;
                    break;
                
            }
        }

        /// <summary>
        /// Refresh the VarName list to include all questions in the survey.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdAllQuestions_Click(object sender, EventArgs e)
        {
            bs.DataSource = CurrentTiming.Questions;
            UpdateVarList();
        }

        /// <summary>
        /// Open the form to enter a Response to a question.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdAddResponse_Click(object sender, EventArgs e)
        {
            EnterResponse frm = new EnterResponse(CurrentTiming.Questions);
            frm.ShowDialog();

            if (frm.Response != null)
            {
                CurrentTiming.User.Responses.Add(frm.Response);
                UpdateResponses();
            }
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

        private void lstUnweightedQuestionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstUnweightedQuestionList.SelectedItems.Count == 0)
                return;

            string sel = lstUnweightedQuestionList.SelectedItems[0].SubItems[1].Text; // the selected Var
            int varPos = CurrentTiming.QuestionIndex(sel);                          // the index of that var in the list
            bs.Position = varPos;
        }

        private void cboUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboUserType.SelectedItem == null)
            {
                CurrentTiming.User = null;
                return;
            }

            CurrentTiming.User = (Respondent)cboUserType.SelectedItem;

            

            UpdateResponses();
        }

        

        #endregion

        private string GetFilePath()
        {
            return TimingFolder + "\\" + CurrentTiming.SurveyCode ;
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
            BindingList<SurveyQuestion> previousWaveQs = DBAction.GetSurveyQuestions(previousWave);
        
            
            foreach (LinkedQuestion q in CurrentTiming.Questions)
            {
                // skip those with weights
                if (q.Weight.Value > -1)
                    continue;
                // skip those without filters
                if (q.FilterList.Count == 0)
                    continue;

                var match = previousWaveQs.FirstOrDefault(x => x.VarName.RefVarName.Equals(q.VarName.RefVarName));

                // skip if same varname in previous wave has different filter
                if (match == null || !match.PreP.Equals(q.PreP))
                    continue;

                s.Append("%filterStat(newVar = " + q.VarName.FullVarName + ", filter = (");
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

                //% filterStat(newVar = BQ49151, filter = (bBQ49141 = 4));
                //% filterStat(newVar = SO49412, filter = (bFR49309v in (1:6)));
                //% filterStat(newVar = SO49407, filter = (bSO49415 in (2:5) or bSO49419 in (2:5)));
                //% filterStat(newVar = BR49608, filter = (bBR49310 = 1));
                //% filterStat(newVar = BR49613, filter = (bBR49310 = 1));
                //% filterStat(newVar = BR49630, filter = (bBR49626 = 1));
                //% filterStat(newVar = ET49324, filter = (bET49328 ne 4));
                //% filterStat(newVar = LM49745, filter = (bBR49327 = 1));
                //% filterStat(newVar = LM49735, filter = (bBR49327 = 1));
                //% filterStat(newVar = LM49746, filter = (bBR49327 = 1));
                //% filterStat(newVar = LM49744, filter = (bBR49327 = 1));
                //% filterStat(newVar = LM49749, filter = (bBR49327 = 1));
                //% filterStat(newVar = EF49420, filter = (bNC49304 in (1:3) and bEC49375 = 3));
                //% filterStat(newVar = EA49145, filter = (bNC49302 in (1, 2)));

                
            }
           

            return s.ToString();
        }

        private string GenerateSASCodeOverallFreq(string project, string projectWave)
        {
            StringBuilder s = new StringBuilder();

            //s.AppendLine("libname itc" + project + " \"D:\\users\\m6yan\\My Data\\ITC\\6Europe\"");
            s.AppendLine("libname itc" + project + " \"[dataset file path]\"");
            s.AppendLine();
            s.AppendLine("Proc format cntlin = itc" + project + ".itc" + project + "_formats; run;");
            s.AppendLine();
            s.AppendLine("Data Core;");
            s.AppendLine("    set itc" + project + ".itc" + project + "_core;");
            s.AppendLine("    if country = 6;");
            s.AppendLine("run; Proc sort; by uniqid; run;");
            s.AppendLine();
            s.AppendLine("Data SP2;");
            s.AppendLine("    set itc" + project + ".itc" + project + "_Wave2;");
            s.AppendLine("    keep _all_");

            return s.ToString();
        }

        private void ExportToXML (List<LinkedQuestion> list)
        {
            string DataFile;

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = GetFilePath(); 
                saveFileDialog.Filter = "XML documents (*.xml)|*.xml";
                saveFileDialog.FilterIndex = 2;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = CurrentTiming.Title + " (" + DateTime.Now.ToString().Replace(":", ".") + ")";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DataFile = saveFileDialog.FileName;
                }
                else
                {
                    return;
                }
            }

            XmlDocument timingData = new XmlDocument();
            
            XmlNode surveyNode = timingData.CreateElement("SurveyTiming");
            timingData.AppendChild(surveyNode);

            XmlAttribute surveyCode = timingData.CreateAttribute("Survey");
            surveyCode.Value = CurrentTiming.SurveyCode;
            surveyNode.Attributes.Append(surveyCode);

            XmlAttribute runTitle = timingData.CreateAttribute("RunTitle");
            runTitle.Value = CurrentTiming.Title;
            surveyNode.Attributes.Append(runTitle);

            XmlNode timing = timingData.CreateElement("Time");
            XmlAttribute time = timingData.CreateAttribute("Mins");
            time.Value = CurrentTiming.TotalTime.ToString();
            timing.Attributes.Append(time);
            XmlAttribute wpm = timingData.CreateAttribute("WPM");
            wpm.Value = CurrentTiming.WPM.ToString();
            timing.Attributes.Append(wpm);
            XmlAttribute startQ = timingData.CreateAttribute("StartQ");
            startQ.Value = CurrentTiming.StartQ.ToString();
            timing.Attributes.Append(startQ);


            surveyNode.AppendChild(timing);

            XmlNode questions = timingData.CreateElement("Questions");
            surveyNode.AppendChild(questions);

            // add all varnames and their filter vars
            foreach (LinkedQuestion q in list)
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



            File.WriteAllText(DataFile, timingData.InnerXml);

            

        }

        

        private SurveyTiming LoadXMLFile(string xmlRunData)
        {
            SurveyTiming newRun = new SurveyTiming();
            XmlDocument runData = new XmlDocument();



            runData.LoadXml(xmlRunData);
            newRun.SurveyCode = runData.SelectSingleNode("/SurveyTiming").Attributes["Survey"].InnerText;
            newRun.Title = runData.SelectSingleNode("/SurveyTiming").Attributes["RunTitle"].InnerText;
            newRun.TotalTime = Double.Parse(runData.SelectSingleNode("/SurveyTiming/Time").Attributes["Mins"].InnerText) * 60;
            newRun.WPM = Int32.Parse( runData.SelectSingleNode("/SurveyTiming/Time").Attributes["WPM"].InnerText);
            newRun.StartQ = Int32.Parse(runData.SelectSingleNode("/SurveyTiming/Time").Attributes["StartQ"].InnerText);
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

                        foreach(XmlNode resps in scenario.SelectNodes("/Responses/Response"))
                        {
                            fi.ValuesStr.Add(resps.Attributes["ResponseValue"].InnerText);
                        }
                        list.Add(fi);
                        
                    }
                    lq.FilterList.Add(list);
                }

                newRun.Questions.Add(lq);
                
            }


            return newRun;
        }

        private TimingType GetMinMax()
        {
            if (cboMaxMin.SelectedItem == null)
            {
                MessageBox.Show("Invalid Min/Max selection.");
                return TimingType.Undefined;
            }
            else if ((TimingType)cboMaxMin.SelectedItem == TimingType.Max)
            {
                return TimingType.Max;
            }
            else if ((TimingType)cboMaxMin.SelectedItem == TimingType.Min)
            {
                return TimingType.Min;
            }
            else
            {
                return TimingType.Undefined;
            }
        }

       

        private TimingReportType GetTimingScheme()
        {
            if (cboTimingScheme.SelectedItem == null)
                return TimingReportType.Undefined;

            if (cboTimingScheme.SelectedItem.Equals("User Based"))
            {
                return TimingReportType.TimingUser;
            }
            if (cboTimingScheme.SelectedItem.Equals("Whole Survey"))
            {
                return TimingReportType.TimingWholeSurvey;
            }
            else
            {
                return TimingReportType.Undefined;
            }

        }

        private Respondent GetCurrentRespondent()
        {
            if (cboUserType.SelectedItem == null)
                return new Respondent();

            Respondent r = new Respondent((Respondent)cboUserType.SelectedItem);

            //return r;

            return (Respondent)cboUserType.SelectedItem;
        }

        private Respondent GetUser()
        {
            // get min/max user
            Respondent r;
            TimingType type = GetMinMax();
            if (type == TimingType.Max)
            {
                r = CurrentTiming.GetMaxUser(GetCurrentRespondent());
            }
            else if (type == TimingType.Min)
            {
                r = CurrentTiming.GetMinUser(GetCurrentRespondent());
            }
            else
            {
                r = GetCurrentRespondent();
            }

            return r;
        }


        private void ImportWeights(string filePath)
        {

            if (string.IsNullOrEmpty(filePath))
                return;

            string[] lines = System.IO.File.ReadAllLines(filePath);

            try
            {

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

                    LinkedQuestion q2 = CurrentTiming.Questions.SingleOrDefault(x => x.VarName.RefVarName.Equals(varname));

                    if (q2 != null && q2.Weight.Source != "A") // do not override automatic weights
                        q2.Weight = new Weight(weight, source);

                   
                }
            }catch (Exception e)
            {
                int i = 0;
            }
            
        }

        private void ImportWeightsFromWord(string filePath)
        {

            if (string.IsNullOrEmpty(filePath))
                return;

           

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

                    double weight = double.Parse( row.Elements<TableCell>().Last().GetCellText());
                    
                    LinkedQuestion q2 = CurrentTiming.Questions.SingleOrDefault(x => x.VarName.RefVarName.Equals(varname));

                    if (q2 != null)
                        q2.Weight = new Weight(weight, source);

                   
                }

                
               
            }
        


        string[] lines = System.IO.File.ReadAllLines(filePath);

            try
            {

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

                    LinkedQuestion q2 = CurrentTiming.Questions.SingleOrDefault(x => x.VarName.RefVarName.Equals(varname));

                    if (q2 != null)
                        q2.Weight = new Weight(weight, source);


                }
            }
            catch (Exception)
            {
                int i = 0;
            }

        }

   

        private void ShowMaxQuestions()
        {

            // get currently selected respondent
            Respondent max = CurrentTiming.GetMaxUser(CurrentTiming.User);

            CurrentTiming.User = max;

            // get the list of questions that this respondent should answer
            CurrentTiming.UserQuestions = CurrentTiming.GetRespondentQuestions(max, true);
            // refresh the form's list of questions
            bs.DataSource = CurrentTiming.UserQuestions;
            UpdateVarList();
            UpdateTiming();
            
            UpdateResponses();
        }

        private void ShowMinQuestions()
        {
            // get currently selected respondent
            Respondent min = CurrentTiming.GetMinUser(CurrentTiming.User);

            CurrentTiming.User = min;

            // get the list of questions that this respondent should answer
            CurrentTiming.UserQuestions = CurrentTiming.GetRespondentQuestions(min, true);
            // refresh the form's list of questions
            bs.DataSource = CurrentTiming.UserQuestions;
            UpdateVarList();
            UpdateTiming();
            
            UpdateResponses();
        }

        private void ShowUndefinedQuestions()
        {

            // get the list of questions that this respondent should answer
            CurrentTiming.UserQuestions = CurrentTiming.GetRespondentQuestions(true);
            // refresh the form's list of questions
            bs.DataSource = CurrentTiming.UserQuestions;
            UpdateVarList();
            UpdateTiming();

            UpdateResponses();
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

        private DataTable WholeSurveyTimingTable(Respondent resp, List<LinkedQuestion> questionList)
        {
            // create the data table
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("Qnum"));
            dt.Columns.Add(new DataColumn("VarName"));
            dt.Columns.Add(new DataColumn("Question"));
            dt.Columns.Add(new DataColumn("VarLabel"));
            dt.Columns.Add(new DataColumn("Word Count"));
            dt.Columns.Add(new DataColumn("Timing Estimate"));
            dt.Columns.Add(new DataColumn("Weight"));
            dt.Columns.Add(new DataColumn("Weighted Time"));

            double totalTime = 0;
            double totalWeightedTime = 0;
          
            foreach (LinkedQuestion lq in questionList)
            {
                // construct the row
                DataRow r = dt.NewRow();

                // basic info
                r["Qnum"] = lq.Qnum;
                r["VarName"] = lq.VarName.RefVarName;
                r["Question"] = lq.GetQuestionText();
                r["VarLabel"] = "<strong><em>" + lq.VarName.VarLabel + "</em></strong>\r\n" + lq.RespOptions + "\r\n" + lq.NRCodes;

                // timing info
                double time = lq.GetTiming(CurrentTiming.WPM);
                double weight = lq.Weight.Value;

                if (lq.IsDerived())
                    time = 0;

                r["Timing Estimate"] = time;
                r["Weight"] = weight;

                if (weight < 0)
                    r["Weighted Time"] = 0;
                else
                    r["Weighted Time"] = (time * weight).ToString("N2");

                totalTime += time;
                if (weight > 0)
                    totalWeightedTime += time * weight;
                else
                    totalWeightedTime += 0;
                dt.Rows.Add(r);
            }

            DataRow total = dt.NewRow();

            total["VarName"] = "Total";
            total["Timing Estimate"] = ((double)totalTime / 60).ToString("N2") + " mins";
            //total["Timing Estimate"] = totalTime;
            total["Weighted Time"] = ((double)totalWeightedTime / 60).ToString("N2") + " mins";

            dt.Rows.Add(total);

            DataRow total2 = dt.NewRow();

            total2["VarName"] = "Total";
            total2["VarLabel"] = CurrentTiming.User.Description;
            total2["Timing Estimate"] = (double)totalTime / 60 + " mins";
            total2["Weighted Time"] = (double)totalWeightedTime / 60 + " mins";

            dt.Rows.InsertAt(total2, 0);

            resp.TotalTime = (double)totalTime / 60;
            resp.WeightedTime = (double)totalWeightedTime / 60;

            return dt;
        }

        private DataTable ReportTimingTable(Respondent resp, List<LinkedQuestion> questionList, bool removeNoFilter = true)
        {
            // create the data table
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("Qnum"));
            dt.Columns.Add(new DataColumn("VarName"));
            dt.Columns.Add(new DataColumn("Response"));
            dt.Columns.Add(new DataColumn("Question"));
            dt.Columns.Add(new DataColumn("VarLabel"));
            dt.Columns.Add(new DataColumn("DSV"));
            dt.Columns.Add(new DataColumn("Used As Filter For"));
            dt.Columns.Add(new DataColumn("Timing Estimate"));
            dt.Columns.Add(new DataColumn("Weight"));
            dt.Columns.Add(new DataColumn("Weighted Time"));
            dt.Columns.Add(new DataColumn("Filters"));

            double totalTime = 0;
            double totalWeightedTime = 0;

            // TEMPORARY
            //List<LinkedQuestion> questions2 = new List<LinkedQuestion>();
            //if (CurrentTiming.SurveyCode.Equals("6E2"))
            //{
            //    Survey survey2 = DBAction.GetSurveyInfo("ES2.5");
            //    var qs2 = DBAction.GetSurveyQuestions(survey2).ToList();


            //    foreach (SurveyQuestion q in qs2)
            //    {
            //        questions2.Add(new LinkedQuestion(q));
            //    }

            //}
            //else if (CurrentTiming.SurveyCode.Equals("ES2.5"))
            //{
            //    Survey survey2 = DBAction.GetSurveyInfo("6E2");
            //    var qs2 = DBAction.GetSurveyQuestions(survey2).ToList();

            //    foreach (SurveyQuestion q in qs2)
            //    {
            //        questions2.Add(new LinkedQuestion(q));
            //    }

            //}


            // for each question, 
            // if there is an answer provided, 
            // get the list of questions that are directly and indirectly impacted by this answer, and create a list with varlabel etc.
            // otherwise, the list is empty
            foreach (LinkedQuestion lq in questionList)
            {
                // TEMPORARY
                //if (questions2.Contains(lq, new LinkedQuestionComparer()))
                //    continue;

                //if (Int32.Parse(lq.Qnum.Substring(0, 3)) >= 294 && Int32.Parse(lq.Qnum.Substring(0, 3)) <= 349)
                //    continue;


                int directFilterCount = 0;
                int indirectFilterCount = 0;
                string filterList, filterExpList = "";
                Dictionary<string, string> filterExpressions = new Dictionary<string, string>();
                filterList = "";

                var response = resp.Responses.Find(x => x.VarName.Equals(lq.VarName.RefVarName));
                if (response != null)
                {
                    FilterInstruction fi = new FilterInstruction();
                    fi.VarName = lq.VarName.RefVarName;
                    fi.Oper = Operation.Equals;
                    fi.ValuesStr.Add(response.ResponseCode);
                    fi.FilterExpression = fi.VarName + "=" + fi.ValuesStr[0];

                    List<LinkedQuestion> directFilterList = CurrentTiming.GetDirectFilterConditionList(fi);
                    directFilterCount = directFilterList.Count();

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
                        foreach(string k in filterExpressions.Keys)
                        {
                            if (filterExpressions[k].Equals(f))
                            {
                                filterExpList += k + "\r\n";
                            }
                        }

                        filterExpList += "\r\n";
                    }

                    List<LinkedQuestion> indirectFilterList = new List<LinkedQuestion>();

                    if (lq.GetQnumValue() > CurrentTiming.StartQ)
                    {
                       
                        foreach (LinkedQuestion q in CurrentTiming.GetIndirectFilterVarList(lq))
                        {
                            if (CurrentTiming.QuestionHasIndirectFilter(q, fi))
                                indirectFilterList.Add(q);

                        }

                        indirectFilterList = indirectFilterList.Except(directFilterList).ToList();
                        indirectFilterCount = indirectFilterList.Count();
                    }

                    filterList = UsedAsFilterColumn(lq, directFilterList, indirectFilterList, fi);
                    
                }
                else
                {
                    response = new Answer("", "");

                    filterList = "";
                }

                if (directFilterCount == 0 && removeNoFilter)
                    continue;

                // construct the row
                DataRow r = dt.NewRow();

                // basic info
                r["Qnum"] = lq.Qnum;
                r["VarName"] = lq.VarName.RefVarName;
                if (!string.IsNullOrEmpty(response.ToString()))
                    r["Response"] = response.ToString();
                r["Filters"] = filterExpList;
                r["Question"] = lq.GetQuestionText();
                r["VarLabel"] = "<strong><em>" + lq.VarName.VarLabel + "</em></strong>\r\n" + lq.RespOptions + "\r\n" + lq.NRCodes;

                // filter info
                if (directFilterCount > 0 || indirectFilterCount > 0)
                {
                    r["DSV"] = directFilterCount + indirectFilterCount;
                }

                r["Used As Filter For"] = filterList;

                // timing info
                double time = lq.GetTiming(CurrentTiming.WPM);
                double weight = lq.Weight.Value; //GetQuestionWeight(lq, resp);

                if (lq.IsDerived())
                    time = 0;

                r["Timing Estimate"] = time;
                r["Weight"] = weight;

                if (weight <0)
                    r["Weighted Time"] = 0;
                else 
                    r["Weighted Time"] = (time * weight).ToString("N2");

                totalTime += time;
                if (weight >0)
                    totalWeightedTime += time * weight;
                else
                    totalWeightedTime += 0;
                dt.Rows.Add(r);
            }

            DataRow total = dt.NewRow();

            total["VarName"] = "Total";
            total["Timing Estimate"] = ((double)totalTime / 60).ToString("N2") + " mins";
            //total["Timing Estimate"] = totalTime;
            total["Weighted Time"] = ((double)totalWeightedTime / 60).ToString("N2") + " mins";

            dt.Rows.Add(total);

            DataRow total2 = dt.NewRow();

            total2["VarName"] = "Total";
            total2["VarLabel"] = CurrentTiming.User.Description;
            total2["Timing Estimate"] = (double)totalTime / 60 + " mins";
            total2["Weighted Time"] = (double)totalWeightedTime / 60 + " mins";

            dt.Rows.InsertAt(total2, 0);

            resp.TotalTime = (double)totalTime / 60;
            resp.WeightedTime = (double)totalWeightedTime / 60;

            return dt;
        }    

        /// <summary>
        /// Generates a table with all questions in the survey. Includes columns for DSVs and filter lists
        /// </summary>
        /// <param name="questionList"></param>
        /// <param name="removeNoFilter"></param>
        /// <returns></returns>
        private DataTable Report(List<LinkedQuestion> questionList, bool removeNoFilter = true)
        { 
            
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("Qnum"));
            dt.Columns.Add(new DataColumn("VarName"));
            
            dt.Columns.Add(new DataColumn("Question"));
            dt.Columns.Add(new DataColumn("VarLabel"));
            dt.Columns.Add(new DataColumn("DSV"));
            dt.Columns.Add(new DataColumn("Used As Filter For"));
           //// dt.Columns.Add(new DataColumn("Weight"));
           // dt.Columns.Add(new DataColumn("Weight Source"));
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
             //   r["Weight"] = lq.Weight.Value;
              //  r["Weight Source"] = lq.Weight.Source;

                dt.Rows.Add(r);
            }
            
            
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

        private void OutputReport(DataTable dt, string customFileName)
        {
            SurveyReport SR = new SurveyReport();
            SR.Surveys.Add(new ReportSurvey(CurrentTiming.SurveyCode));
            SR.FileName = "\\\\psychfile\\psych$\\psych-lab-gfong\\SMG\\Access\\Reports\\ISR\\";
            
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

        private void cmdMeanTimes_Click(object sender, EventArgs e)
        {
            List<double> gridValues = new List<double>();
            string maxGridVar, minGridVar; 
            int gridCount=0;
            double totalGrid = 0, maxGrid=0, minGrid=-1, meanGrid=0;

            List<double> ynValues = new List<double>();
            string maxYNVar, minYNVar;
            int ynCount=0;
            double totalYN=0,maxYN=0, minYN=-1, meanYN=0;

            List<double> standValues = new List<double>();
            string maxStandVar, minStandVar;
            int standCount=0;
            double totalStand = 0, maxStand=0, minStand=-1, meanStand=0;


            foreach(LinkedQuestion lq in CurrentTiming.Questions)
            {
                double time = lq.GetTiming(CurrentTiming.WPM);

                if (time == 0)
                    continue;

                if (lq.PreI.Contains ("program as grid"))
                {
                    gridCount++;
                    totalGrid += time;
                    gridValues.Add((int)time);

                    if (time > maxGrid)
                    {
                        maxGrid = time;
                        maxGridVar = lq.VarName.RefVarName;
                    }

                    if (minGrid == -1 || time < minGrid)
                    {
                        minGrid = time;
                        minGridVar = lq.VarName.RefVarName;
                    }
                    
                }else if (lq.RespName.Equals("yesno") || lq.RespName.Equals("truefals"))
                {
                    ynCount++;
                    totalYN += time;
                    ynValues.Add((int)time);

                    if (time > maxYN)
                    {
                        maxYN = time;
                        maxYNVar = lq.VarName.RefVarName;
                    }

                    if (minYN == -1 || time < minYN)
                    {
                        minYN = time;
                        minYNVar = lq.VarName.RefVarName;
                    }
                }else if ((lq.Qnum.Length==3 || lq.Qnum.EndsWith("a")) && !lq.VarName.FullVarName.StartsWith("Z"))
                {
                    standCount++;
                    totalStand += time;
                    standValues.Add((int)time);

                    if (time > maxStand)
                    {
                        maxStand = time;
                        maxStandVar = lq.VarName.RefVarName;
                    }

                    if (minStand == -1 || time < minStand)
                    {
                        minStand = time;
                        minStandVar = lq.VarName.RefVarName;
                    }
                        
                 
                }
            }

            decimal medianGrid = GetMedian(gridValues.ToArray());
            decimal medianYN = GetMedian(ynValues.ToArray());
            decimal medianStand = GetMedian(standValues.ToArray());

            meanGrid = (double)totalGrid / gridCount;
            meanYN = (double)totalYN / ynCount;
            meanStand = (double)totalStand / standCount;
        }
        
        public decimal GetMedian(IEnumerable<double> source)
        {
            // Create a copy of the input, and sort the copy
            double[] temp = source.ToArray();
            Array.Sort(temp);

            int count = temp.Length;
            if (count == 0)
            {
                throw new InvalidOperationException("Empty collection");
            }
            else if (count % 2 == 0)
            {
                // count is even, average two middle elements
                double a = temp[count / 2 - 1];
                double b = temp[count / 2];
                return (decimal) (a + b) / 2m;
            }
            else
            {
                // count is odd, return the middle element
                return (decimal) temp[count / 2];
            }
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

        private void chkWeightedToggle_CheckedChanged(object sender, EventArgs e)
        {
            UpdateTiming();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void cmdRefreshLists_Click(object sender, EventArgs e)
        {
            RefreshLists();
        }



        #region Not Implemented or Not working or Temporary

        private void FrequencyTable(DataTable dt)
        {
            // frequenct table
            SurveyReport freq = new SurveyReport();
            freq.Surveys.Add(new ReportSurvey("NZL3"));
            freq.FileName = "\\\\psychfile\\psych$\\psych-lab-gfong\\SMG\\Access\\Reports\\ISR\\";
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

            lblUniverse.Text = "Ask " + lq.VarName.FullVarName + " if:";
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

