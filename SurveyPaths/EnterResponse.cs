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
    public partial class EnterResponse : Form
    {
        SurveyQuestion CurrentQuestion;
        BindingSource bs;
        public Answer Response { get; set; }
        public EnterResponse(List<SurveyQuestion> questions )
        {
            InitializeComponent();

            bs = new BindingSource()
            {
                DataSource = questions.Where(x => !(string.IsNullOrEmpty(x.RespOptions))).ToList()
            };
            bs.PositionChanged += Bs_PositionChanged;
            cboVarName.DataSource = questions.Where(x=>!(string.IsNullOrEmpty(x.RespOptions))).ToList();
            cboVarName.DisplayMember = "VarName.RefVarName";

        }

        private void Bs_PositionChanged(object sender, EventArgs e)
        {
            CurrentQuestion = (SurveyQuestion)bs.Current;
            rtbQuestionText.Rtf = "";
            rtbQuestionText.Rtf = CurrentQuestion.GetQuestionTextRich();

        }

        private void cboVarName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox ctl = sender as ComboBox;

            SurveyQuestion selected = (SurveyQuestion)ctl.SelectedItem;

            LoadQuestion(selected);
        }

        private void LoadQuestion(SurveyQuestion lq)
        {
            rtbQuestionText.Rtf = "";
            rtbQuestionText.Rtf = lq.GetQuestionTextRich();

            cboResponse.DataSource = lq.GetRespNumbers();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            SurveyQuestion q = (SurveyQuestion)cboVarName.SelectedItem;
            int response = Int32.Parse((string)cboResponse.SelectedItem);
            string responseCode = (string)cboResponse.SelectedItem;
            Response = new Answer(q.VarName.RefVarName, responseCode);
            Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Response = null;
            Close();
        }
    }
}
