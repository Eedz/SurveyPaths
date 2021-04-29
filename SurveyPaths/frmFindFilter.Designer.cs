namespace SurveyPaths
{
    partial class frmFindFilter
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.cmdFindFilter = new System.Windows.Forms.Button();
            this.lstQuestionList = new System.Windows.Forms.ListView();
            this.chQnum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chRefVarName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chVarLabel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chWeight = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chWeightSource = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chWordCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chUser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtFilter
            // 
            this.txtFilter.Location = new System.Drawing.Point(12, 82);
            this.txtFilter.Multiline = true;
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(326, 36);
            this.txtFilter.TabIndex = 0;
            // 
            // cmdFindFilter
            // 
            this.cmdFindFilter.Location = new System.Drawing.Point(349, 86);
            this.cmdFindFilter.Name = "cmdFindFilter";
            this.cmdFindFilter.Size = new System.Drawing.Size(75, 23);
            this.cmdFindFilter.TabIndex = 2;
            this.cmdFindFilter.Text = "Find Filter";
            this.cmdFindFilter.UseVisualStyleBackColor = true;
            this.cmdFindFilter.Click += new System.EventHandler(this.cmdFindFilter_Click);
            // 
            // lstQuestionList
            // 
            this.lstQuestionList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chQnum,
            this.chRefVarName,
            this.chVarLabel,
            this.chWeight,
            this.chWeightSource,
            this.chWordCount,
            this.chTime,
            this.chUser});
            this.lstQuestionList.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstQuestionList.FullRowSelect = true;
            this.lstQuestionList.HideSelection = false;
            this.lstQuestionList.Location = new System.Drawing.Point(12, 124);
            this.lstQuestionList.Name = "lstQuestionList";
            this.lstQuestionList.Size = new System.Drawing.Size(648, 314);
            this.lstQuestionList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstQuestionList.TabIndex = 69;
            this.lstQuestionList.UseCompatibleStateImageBehavior = false;
            // 
            // chQnum
            // 
            this.chQnum.Text = "Qnum";
            // 
            // chRefVarName
            // 
            this.chRefVarName.Text = "RefVarName";
            this.chRefVarName.Width = 100;
            // 
            // chVarLabel
            // 
            this.chVarLabel.Text = "VarLabel";
            this.chVarLabel.Width = 400;
            // 
            // chWeight
            // 
            this.chWeight.Text = "Weight";
            // 
            // chWeightSource
            // 
            this.chWeightSource.Text = "Source";
            // 
            // chWordCount
            // 
            this.chWordCount.Text = "Word Count";
            // 
            // chTime
            // 
            this.chTime.Text = "Time";
            // 
            // chUser
            // 
            this.chUser.Text = "+/-";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 33);
            this.label1.TabIndex = 70;
            this.label1.Text = "Find Filter";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(328, 13);
            this.label2.TabIndex = 71;
            this.label2.Text = "Enter all or part of a filter and get a list of questions that use the filter.";
            // 
            // frmFindFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstQuestionList);
            this.Controls.Add(this.cmdFindFilter);
            this.Controls.Add(this.txtFilter);
            this.Name = "frmFindFilter";
            this.Text = "Find Filter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Button cmdFindFilter;
        private System.Windows.Forms.ListView lstQuestionList;
        private System.Windows.Forms.ColumnHeader chQnum;
        private System.Windows.Forms.ColumnHeader chRefVarName;
        private System.Windows.Forms.ColumnHeader chVarLabel;
        private System.Windows.Forms.ColumnHeader chWeight;
        private System.Windows.Forms.ColumnHeader chWeightSource;
        private System.Windows.Forms.ColumnHeader chWordCount;
        private System.Windows.Forms.ColumnHeader chTime;
        private System.Windows.Forms.ColumnHeader chUser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}