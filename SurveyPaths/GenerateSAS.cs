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
namespace SurveyPaths
{
    public partial class GenerateSAS : Form
    {
        public Survey Surv;
        public GenerateSAS()
        {
            InitializeComponent();
            Surv = new Survey();
           
            cboSurvey.DisplayMember = "SurveyCode";
            cboSurvey.ValueMember = "SID";
            cboSurvey.DataSource = DBAction.GetAllSurveysInfo();

            
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cboSurvey_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboSurvey.SelectedItem == null)
                return;

            Surv = (Survey) cboSurvey.SelectedItem;
        }
    }
}
