using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using ITCLib;

namespace SurveyPaths
{
    public static class FormUtilities
    {
        /// <summary>
        /// Adds color and formatting to the specified row, based on its QuestionType.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="questionType"></param>
        public static void FormatListItem(ListViewItem row, QuestionType questionType)
        {
            // color row based on type
            row.UseItemStyleForSubItems = true;

            switch (questionType)
            {
                case QuestionType.Series:
                    row.ForeColor = Color.Black;
                    break;
                case QuestionType.Standalone:
                    row.ForeColor = Color.Blue;
                    row.Font = new Font("Arial", 10, FontStyle.Bold);
                    break;

                case QuestionType.Heading:
                    row.ForeColor = Color.Magenta;
                    row.Font = new Font("Arial", 10, FontStyle.Bold);
                    break;
                case QuestionType.InterviewerNote:
                    row.ForeColor = Color.Lime;
                    row.Font = new Font("Arial", 10, FontStyle.Bold);
                    break;
                case QuestionType.Subheading:
                    row.ForeColor = Color.LightBlue;
                    row.Font = new Font("Arial", 10, FontStyle.Bold);
                    break;
            }
        }
    }
}
