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
    public partial class frmFindFilter : Form
    {
        List<LinkedQuestion> Questions;

        public frmFindFilter(List<LinkedQuestion> sourceList)
        {
            InitializeComponent();
            Questions = sourceList;
        }

        private void cmdFindFilter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFilter.Text))
                FindFilter(txtFilter.Text);
        }

        private void FindFilter(string filter)
        {
            var found = Questions.Where(x => x.PreP.Contains(filter));
            if (found.Count() == 0)
            {
                MessageBox.Show("No matches found!");
                return;
            }
            lstQuestionList.Items.Clear();
            lstQuestionList.View = System.Windows.Forms.View.Details;

            foreach (LinkedQuestion lq in found)
            {
                ListViewItem li = new ListViewItem(new string[] { lq.Qnum, lq.VarName.RefVarName, lq.VarName.VarLabel, lq.Weight.Value.ToString(), lq.Weight.Source, lq.WordCount().ToString() });
                li.Tag = lq;

                lstQuestionList.Items.Add(li);

                //FormatListItem(li, GetQuestionType(li));
            }
        }
    }
}
