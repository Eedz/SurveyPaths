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
    public partial class CompareUsers : Form
    {

        public Respondent user1;
        public Respondent user2;

        public TimingType user1Path;
        public TimingType user2Path;

        public CompareUsers(List<Respondent> userTypeList)
        {
            InitializeComponent();

            cboUser1.DataSource = new List<Respondent>(userTypeList);
            cboUser2.DataSource = new List<Respondent>(userTypeList);
            cboUser1Path.DataSource = Enum.GetValues(typeof(TimingType));
            cboUser2Path.DataSource = Enum.GetValues(typeof(TimingType));
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            user1 = (Respondent) cboUser1.SelectedItem;
            user2 = (Respondent)cboUser2.SelectedItem;

            user1Path = (TimingType)cboUser1Path.SelectedItem;
            user2Path = (TimingType)cboUser2Path.SelectedItem;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            user1 = null;
            user2 = null;
            Close();
        }
    }
}
