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
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace SurveyPaths
{
    public partial class frmTiming : Form
    {
        string folderPath = "\\\\psychfile\\psych$\\psych-lab-gfong\\SMG\\Access\\Survey Timing\\User Type Definitions\\";
        string TimingFolder = "\\\\psychfile\\psych$\\psych-lab-gfong\\SMG\\Access\\Survey Timing\\";
        List<SelectableRespondent> UserTypes;

        public frmTiming()
        {
            InitializeComponent();

            UserTypes = new List<SelectableRespondent>();

            cboSurvey.DataSource = DBAction.GetSurveyList();
            cboSurvey.SelectedItem = null;

            cboScheme.Items.Add("Method 2 (User Type)");
            cboScheme.Items.Add("Method 3 (Whole Survey)");
            cmdStartTiming.Top = 101;
            SaveUserTypes();
            
        }

        private void cmdStartTiming_Click(object sender, EventArgs e)
        {
            string scheme = (string) cboScheme.SelectedItem;
            string survey = (string)cboSurvey.SelectedItem;

            switch (scheme)
            {
                case "Method 2 (User Type)":
                    double sum = 0;
                    List<Respondent> chosenUsers = new List<Respondent>();

                    foreach(SelectableRespondent r in UserTypes)
                    {
                        if (r.Selected)
                        {
                            Respondent chosen = new Respondent(r);
                            chosenUsers.Add(chosen);
                            sum += chosen.Weight;
                        }
                    }

                    if (sum != 1)
                    {
                        // re-weight respondents
                        ReWeightRespondents(chosenUsers);
                    }
                    
                    if (chosenUsers.Count() == 0)
                    {
                        MessageBox.Show("Select at least one user type for timing.");
                        return;
                    }

                    frmUserTiming frm2 = new frmUserTiming(survey, chosenUsers);
                    frm2.Show();
                    break;
                case "Method 3 (Whole Survey)":
                    frmSurveyTiming frm1 = new frmSurveyTiming(survey);
                    frm1.Show();
                    break;
            }
        }

        // TODO
        private void method2ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // get path
            var filePath = string.Empty;

            // user common open file dialog here
            using (CommonOpenFileDialog folderBrowser = new CommonOpenFileDialog())
            {
                folderBrowser.IsFolderPicker = true;
                folderBrowser.InitialDirectory = TimingFolder;

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

           

            var files = Directory.GetFiles(filePath, "*.xml");
            List<UserTiming> UserTimings = new List<UserTiming>();
 
            foreach (string f in files)
            {
                UserTimings.Add(new UserTiming(File.ReadAllText(f)));
            }

        }

        // TODO
        private void method3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // get path
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = TimingFolder;
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


            try
            {
                SurveyTiming run = new SurveyTiming(File.ReadAllText(filePath));
                frmSurveyTiming frm = new frmSurveyTiming(run);
                frm.Show();
            }
            catch( Exception ex)
            {
                MessageBox.Show("Invalid saved run.");
                return;
            }
            
        }

        private void loadTimingRunToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SavedRunList frm = new SavedRunList();
            frm.Show();
        }

        

        private void cboSurvey_SelectedIndexChanged(object sender, EventArgs e)
        {
            

            
        }

        private void dgvUserTypes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            //if (e.ColumnIndex == 2)
           // {
                Respondent r = UserTypes[e.RowIndex];
                lstDefinition.DataSource = r.Responses;
            //}
        }

        private void dgvUserTypes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            if (e.ColumnIndex == 2)
            {
                Respondent r = UserTypes[e.RowIndex];
                frmEditUserType frm = new frmEditUserType(r);
                frm.ShowDialog();
            }
        }

        private void cboScheme_SelectedIndexChanged(object sender, EventArgs e)
        {

            string scheme = (string)cboScheme.SelectedItem;

            UpdateScheme(scheme);

            if (scheme.Equals("Method 3 (Whole Survey)"))
                return;

            string selected = (string)cboSurvey.SelectedItem;

            GetUserTypes(selected);

            

            //foreach (SelectableRespondent r in UserTypes)
            //{
            //    string[] row = new string[] { r.Description, r.Weight.ToString(), r.Selected.ToString() };
            //    dgvUserTypes.Rows.Add(row);
            //}
        }

        private void UpdateScheme(string scheme)
        {
            if (scheme.Equals("Method 3 (Whole Survey)"))
            {
                cmdNewUserType.Visible = false;
                dgvUserTypes.Visible = false;
                lstDefinition.Visible = false;
                cmdStartTiming.Top = 101;
                return;
            }

            cmdStartTiming.Top = 284;
            cmdNewUserType.Visible = true;
            dgvUserTypes.Visible = true;
            lstDefinition.Visible = true;
        }

        private void GetUserTypes(string survey)
        {
            
            var files = Directory.GetFiles(folderPath);
            UserTypes.Clear();

            foreach (string f in files)
            {
                SelectableRespondent r = new SelectableRespondent(new Respondent(File.ReadAllText(f)));

                if (r.Survey.Equals(survey))
                    UserTypes.Add(r);
            }

            dgvUserTypes.DataSource = null;
            dgvUserTypes.DataSource = UserTypes;
        }

        private void SaveUserTypes()
        {
            
            Respondent r = UserTypeDefs.Create6E2UT1();
            File.WriteAllText(folderPath + "6E2 User type 1.xml", r.SaveToXML());
            r = UserTypeDefs.Create6E2UT2();
            File.WriteAllText(folderPath + "6E2 User type 2.xml", r.SaveToXML());
            r = UserTypeDefs.Create6E2UT3();
            File.WriteAllText(folderPath + "6E2 User type 3.xml", r.SaveToXML());
            r = UserTypeDefs.Create6E2UT4();
            File.WriteAllText(folderPath + "6E2 User type 4.xml", r.SaveToXML());
            r = UserTypeDefs.CreateESUT1();
            File.WriteAllText(folderPath + "ES2.5 User type 1.xml", r.SaveToXML());
            r = UserTypeDefs.CreateESUT2();
            File.WriteAllText(folderPath + "ES2.5 User type 2.xml", r.SaveToXML());
            r = UserTypeDefs.CreateESUT3();
            File.WriteAllText(folderPath + "ES2.5 User type 3.xml", r.SaveToXML());
            r = UserTypeDefs.CreateESUT4();
            File.WriteAllText(folderPath + "ES2.5 User type 4.xml", r.SaveToXML());

            r = UserTypeDefs.CreateJP3_NSH_User();
            File.WriteAllText(folderPath + "JP3 Nonsmoker HTP User.xml", r.SaveToXML());
            r = UserTypeDefs.CreateJP3_NSNH_User();
            File.WriteAllText(folderPath + "JP3 Nonsmoker NonHTP User.xml", r.SaveToXML());
            r = UserTypeDefs.CreateJP3_NSNH_User();
            File.WriteAllText(folderPath + "JP3 Nonsmoker NonHTP User.xml", r.SaveToXML());
            r = UserTypeDefs.CreateJP3_SH_User();
            File.WriteAllText(folderPath + "JP3 Smoker HTP User.xml", r.SaveToXML());
            r = UserTypeDefs.CreateJP3_SNH_User();
            File.WriteAllText(folderPath + "JP3 Smoker NonHTP User.xml", r.SaveToXML());
            r = UserTypeDefs.CreateJP3_XSH_User();
            File.WriteAllText(folderPath + "JP3 ExSmoker HTP User.xml", r.SaveToXML());

            r = UserTypeDefs.CreateJP2_NSH_User();
            File.WriteAllText(folderPath + "JP2 Nonsmoker HTP User.xml", r.SaveToXML());
            r = UserTypeDefs.CreateJP2_NSNH_User();
            File.WriteAllText(folderPath + "JP2 Nonsmoker NonHTP User.xml", r.SaveToXML());
            r = UserTypeDefs.CreateJP2_NSNH_User();
            File.WriteAllText(folderPath + "JP2 Nonsmoker NonHTP User.xml", r.SaveToXML());
            r = UserTypeDefs.CreateJP2_SH_User();
            File.WriteAllText(folderPath + "JP2 Smoker HTP User.xml", r.SaveToXML());
            r = UserTypeDefs.CreateJP2_SNH_User();
            File.WriteAllText(folderPath + "JP2 Smoker NonHTP User.xml", r.SaveToXML());
            r = UserTypeDefs.CreateJP2_XSH_User();
            File.WriteAllText(folderPath + "JP2 ExSmoker HTP User.xml", r.SaveToXML());

            r = UserTypeDefs.CreateNLD2_UT1();
            File.WriteAllText(folderPath + "NLD2 UT1.xml", r.SaveToXML());
            r = UserTypeDefs.CreateNLD2_UT2();
            File.WriteAllText(folderPath + "NLD2 UT2.xml", r.SaveToXML());
            r = UserTypeDefs.CreateNLD2_UT3();
            File.WriteAllText(folderPath + "NLD2 UT3.xml", r.SaveToXML());
            r = UserTypeDefs.CreateNLD2_UT4();
            File.WriteAllText(folderPath + "NLD2 UT4.xml", r.SaveToXML());
        }

        private void dgvUserTypes_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewColumn col in dgvUserTypes.Columns)
            {
                switch (col.HeaderText)
                {
                    case "Survey":
                    case "TotalMaxTime":
                    case "TotalMinTime":
                    case "TotalTime":
                    case "WeightedTime":
                        col.Visible = false;
                        break;
                }

                dgvUserTypes.AutoResizeColumns();
            }
            
        }

        private void cmdNewUserType_Click(object sender, EventArgs e)
        {
            if (cboSurvey.SelectedItem == null)
                return;

            string selected = (string)cboSurvey.SelectedItem;

            frmEditUserType frm = new frmEditUserType(selected);

            frm.ShowDialog();

            GetUserTypes(selected);

        }

        private void ReWeightRespondents(List<Respondent> users)
        {
            double sum = Math.Round(users.Sum(x => x.Weight),4);
            foreach (Respondent r in users)
            {
                r.Weight =Math.Round(r.Weight / sum,3);
            }
        }

        
    }
}
