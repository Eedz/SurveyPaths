using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ITCLib;

namespace SurveyPaths
{
    public partial class frmEditUserType : Form
    {
        string folderPath = "\\\\psychfile\\psych$\\psych-lab-gfong\\SMG\\Access\\Survey Timing\\User Type Definitions\\";
        public Respondent UserType;

        public frmEditUserType()
        {
            InitializeComponent();

            UserType = new Respondent();

            cboSurvey.DataSource = DBAction.GetSurveyList();


            cboSurvey.DataBindings.Add(new Binding("SelectedItem", UserType, "Survey"));
            txtDescription.DataBindings.Add(new Binding("Text", UserType, "Description"));
            txtWeight.DataBindings.Add(new Binding("Text", UserType, "Weight"));
            lstDefinition.DataSource = UserType.Responses;
        }

        public frmEditUserType(Respondent r)
        {
            InitializeComponent();

            UserType = r;

            cboSurvey.DataSource = DBAction.GetSurveyList();
         

            cboSurvey.DataBindings.Add(new Binding("SelectedItem", UserType, "Survey"));
            txtDescription.DataBindings.Add(new Binding("Text", UserType, "Description"));
            txtWeight.DataBindings.Add(new Binding("Text", UserType, "Weight"));
            lstDefinition.DataSource = UserType.Responses;
        }

        public frmEditUserType(string survey)
        {
            InitializeComponent();

            UserType = new Respondent();
            UserType.Survey = survey;
            cboSurvey.DataSource = DBAction.GetSurveyList();


            cboSurvey.DataBindings.Add(new Binding("SelectedItem", UserType, "Survey"));
            txtDescription.DataBindings.Add(new Binding("Text", UserType, "Description"));
            txtWeight.DataBindings.Add(new Binding("Text", UserType, "Weight"));
            lstDefinition.DataSource = UserType.Responses;
        }

        private void lstDefinition_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void lstDefinition_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                int index = lstDefinition.SelectedIndex;
                UserType.Responses.RemoveAt(index);
            }
        }

        private void cmdAddResponse_Click(object sender, EventArgs e)
        {
            
            Survey s = DBAction.GetSurveyInfo(UserType.Survey);
            List<SurveyQuestion> surveyQuestions = DBAction.GetSurveyQuestions(s).ToList();
            EnterResponse frm = new EnterResponse(surveyQuestions);

            frm.ShowDialog();

            if (frm.Response != null)
            {
                UserType.Responses.Add(frm.Response);
            }
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            // check if exists
            string filename = folderPath + UserType.Description + ".xml";
            if (File.Exists(filename))
                if (MessageBox.Show("This user type already exists, do you want to overwrite?", "Overwrite?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    // overwrite?
                    File.WriteAllText(filename, UserType.SaveToXML());
                }
                else
                {

                }

            
        }
    }
}
