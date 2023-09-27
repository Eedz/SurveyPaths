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
    public enum ViewBy { Filters= 1, Routing}
    public partial class FilterTree : Form
    {
        public FilterTree(LinkedQuestion question, ViewBy view )
        {
            InitializeComponent();

            TreeNode root = new TreeNode(question.VarName.VarName);

            if (view == ViewBy.Routing)
            {
                AddChildren(question, root);
            }else if (view == ViewBy.Filters)
            {
                AddFilters(question, root);
            }

            root.Expand();
            treeView1.Nodes.Add(root);
        }

        private void AddChildren(LinkedQuestion question, TreeNode root)
        {
            foreach (KeyValuePair<int, LinkedQuestion> p in question.PossibleNext)
            {
                
                if (p.Value != null) {
                    TreeNode t;
                    t = new TreeNode(p.Key + " - " + p.Value.VarName.VarName);
                    t.Expand();
                    root.Nodes.Add(t);
                    AddChildren(p.Value, t);
                }

                
            }

            
        }

        private void AddFilters(LinkedQuestion question, TreeNode root)
        {
            foreach (LinkedQuestion p in question.FilteredOn)
            {

               
                    TreeNode t;
                    t = new TreeNode(p.VarName.VarName );
                    t.Expand();
                    root.Nodes.Add(t);
                    AddFilters(p, t);
                


            }


        }
    }
}
