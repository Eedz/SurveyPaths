namespace SurveyPaths
{
    partial class SavedRunList
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
            this.cboSurvey = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lstToCompare = new System.Windows.Forms.ListBox();
            this.cmdAddTiming = new System.Windows.Forms.Button();
            this.cmdCompare = new System.Windows.Forms.Button();
            this.cmdRevoveTiming = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.chRunName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chRunType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cboSurvey
            // 
            this.cboSurvey.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSurvey.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSurvey.FormattingEnabled = true;
            this.cboSurvey.Location = new System.Drawing.Point(61, 67);
            this.cboSurvey.Name = "cboSurvey";
            this.cboSurvey.Size = new System.Drawing.Size(121, 21);
            this.cboSurvey.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Survey";
            // 
            // lstToCompare
            // 
            this.lstToCompare.FormattingEnabled = true;
            this.lstToCompare.Location = new System.Drawing.Point(427, 94);
            this.lstToCompare.Name = "lstToCompare";
            this.lstToCompare.Size = new System.Drawing.Size(196, 69);
            this.lstToCompare.TabIndex = 3;
            // 
            // cmdAddTiming
            // 
            this.cmdAddTiming.Location = new System.Drawing.Point(346, 94);
            this.cmdAddTiming.Name = "cmdAddTiming";
            this.cmdAddTiming.Size = new System.Drawing.Size(75, 23);
            this.cmdAddTiming.TabIndex = 4;
            this.cmdAddTiming.Text = "->";
            this.cmdAddTiming.UseVisualStyleBackColor = true;
            this.cmdAddTiming.Click += new System.EventHandler(this.cmdAddTiming_Click);
            // 
            // cmdCompare
            // 
            this.cmdCompare.Location = new System.Drawing.Point(427, 169);
            this.cmdCompare.Name = "cmdCompare";
            this.cmdCompare.Size = new System.Drawing.Size(75, 23);
            this.cmdCompare.TabIndex = 6;
            this.cmdCompare.Text = "Compare";
            this.cmdCompare.UseVisualStyleBackColor = true;
            this.cmdCompare.Click += new System.EventHandler(this.cmdCompare_Click);
            // 
            // cmdRevoveTiming
            // 
            this.cmdRevoveTiming.Location = new System.Drawing.Point(346, 123);
            this.cmdRevoveTiming.Name = "cmdRevoveTiming";
            this.cmdRevoveTiming.Size = new System.Drawing.Size(75, 23);
            this.cmdRevoveTiming.TabIndex = 7;
            this.cmdRevoveTiming.Text = "<-";
            this.cmdRevoveTiming.UseVisualStyleBackColor = true;
            this.cmdRevoveTiming.Click += new System.EventHandler(this.cmdRevoveTiming_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chRunName,
            this.chRunType});
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(61, 95);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(278, 179);
            this.listView1.TabIndex = 8;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // chRunName
            // 
            this.chRunName.Text = "Title";
            this.chRunName.Width = 200;
            // 
            // chRunType
            // 
            this.chRunType.Text = "Type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Runs";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(278, 33);
            this.label3.TabIndex = 10;
            this.label3.Text = "Compare Timing Runs";
            // 
            // SavedRunList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 291);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.cmdRevoveTiming);
            this.Controls.Add(this.cmdCompare);
            this.Controls.Add(this.cmdAddTiming);
            this.Controls.Add(this.lstToCompare);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboSurvey);
            this.Name = "SavedRunList";
            this.Text = "Compare Timing Runs";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cboSurvey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstToCompare;
        private System.Windows.Forms.Button cmdAddTiming;
        private System.Windows.Forms.Button cmdCompare;
        private System.Windows.Forms.Button cmdRevoveTiming;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader chRunName;
        private System.Windows.Forms.ColumnHeader chRunType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}