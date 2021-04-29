namespace SurveyPaths
{
    partial class frmTiming
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
            this.cboScheme = new System.Windows.Forms.ComboBox();
            this.cmdStartTiming = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.loadTimingRunToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.lstDefinition = new System.Windows.Forms.ListBox();
            this.dgvUserTypes = new System.Windows.Forms.DataGridView();
            this.cmdNewUserType = new System.Windows.Forms.Button();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.method2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.method3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserTypes)).BeginInit();
            this.SuspendLayout();
            // 
            // cboSurvey
            // 
            this.cboSurvey.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSurvey.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSurvey.FormattingEnabled = true;
            this.cboSurvey.Location = new System.Drawing.Point(56, 42);
            this.cboSurvey.Name = "cboSurvey";
            this.cboSurvey.Size = new System.Drawing.Size(144, 21);
            this.cboSurvey.TabIndex = 0;
            this.cboSurvey.SelectedIndexChanged += new System.EventHandler(this.cboSurvey_SelectedIndexChanged);
            // 
            // cboScheme
            // 
            this.cboScheme.FormattingEnabled = true;
            this.cboScheme.Location = new System.Drawing.Point(56, 72);
            this.cboScheme.Name = "cboScheme";
            this.cboScheme.Size = new System.Drawing.Size(144, 21);
            this.cboScheme.TabIndex = 1;
            this.cboScheme.SelectedIndexChanged += new System.EventHandler(this.cboScheme_SelectedIndexChanged);
            // 
            // cmdStartTiming
            // 
            this.cmdStartTiming.Location = new System.Drawing.Point(56, 282);
            this.cmdStartTiming.Name = "cmdStartTiming";
            this.cmdStartTiming.Size = new System.Drawing.Size(144, 23);
            this.cmdStartTiming.TabIndex = 2;
            this.cmdStartTiming.Text = "New Timing Run";
            this.cmdStartTiming.UseVisualStyleBackColor = true;
            this.cmdStartTiming.Click += new System.EventHandler(this.cmdStartTiming_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Survey";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Method";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.loadTimingRunToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(512, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // loadTimingRunToolStripMenuItem1
            // 
            this.loadTimingRunToolStripMenuItem1.Name = "loadTimingRunToolStripMenuItem1";
            this.loadTimingRunToolStripMenuItem1.Size = new System.Drawing.Size(68, 20);
            this.loadTimingRunToolStripMenuItem1.Text = "Compare";
            this.loadTimingRunToolStripMenuItem1.Click += new System.EventHandler(this.loadTimingRunToolStripMenuItem1_Click);
            // 
            // lstDefinition
            // 
            this.lstDefinition.FormattingEnabled = true;
            this.lstDefinition.IntegralHeight = false;
            this.lstDefinition.Location = new System.Drawing.Point(404, 99);
            this.lstDefinition.Name = "lstDefinition";
            this.lstDefinition.Size = new System.Drawing.Size(101, 178);
            this.lstDefinition.TabIndex = 6;
            this.lstDefinition.Visible = false;
            // 
            // dgvUserTypes
            // 
            this.dgvUserTypes.AllowUserToAddRows = false;
            this.dgvUserTypes.AllowUserToDeleteRows = false;
            this.dgvUserTypes.AllowUserToResizeRows = false;
            this.dgvUserTypes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUserTypes.Location = new System.Drawing.Point(56, 99);
            this.dgvUserTypes.Name = "dgvUserTypes";
            this.dgvUserTypes.RowHeadersVisible = false;
            this.dgvUserTypes.Size = new System.Drawing.Size(342, 177);
            this.dgvUserTypes.TabIndex = 7;
            this.dgvUserTypes.Visible = false;
            this.dgvUserTypes.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUserTypes_CellClick);
            this.dgvUserTypes.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUserTypes_CellDoubleClick);
            this.dgvUserTypes.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvUserTypes_DataBindingComplete);
            // 
            // cmdNewUserType
            // 
            this.cmdNewUserType.Location = new System.Drawing.Point(306, 72);
            this.cmdNewUserType.Name = "cmdNewUserType";
            this.cmdNewUserType.Size = new System.Drawing.Size(92, 21);
            this.cmdNewUserType.TabIndex = 8;
            this.cmdNewUserType.Text = "New User Type";
            this.cmdNewUserType.UseVisualStyleBackColor = true;
            this.cmdNewUserType.Visible = false;
            this.cmdNewUserType.Click += new System.EventHandler(this.cmdNewUserType_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.method2ToolStripMenuItem,
            this.method3ToolStripMenuItem});
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.loadToolStripMenuItem.Text = "Load";
            // 
            // method2ToolStripMenuItem
            // 
            this.method2ToolStripMenuItem.Name = "method2ToolStripMenuItem";
            this.method2ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.method2ToolStripMenuItem.Text = "Method 2";
            this.method2ToolStripMenuItem.Click += new System.EventHandler(this.method2ToolStripMenuItem_Click);
            // 
            // method3ToolStripMenuItem
            // 
            this.method3ToolStripMenuItem.Name = "method3ToolStripMenuItem";
            this.method3ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.method3ToolStripMenuItem.Text = "Method 3";
            this.method3ToolStripMenuItem.Click += new System.EventHandler(this.method3ToolStripMenuItem_Click);
            // 
            // frmTiming
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 312);
            this.Controls.Add(this.cmdNewUserType);
            this.Controls.Add(this.dgvUserTypes);
            this.Controls.Add(this.lstDefinition);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdStartTiming);
            this.Controls.Add(this.cboScheme);
            this.Controls.Add(this.cboSurvey);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmTiming";
            this.Text = "Survey Timing";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserTypes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboSurvey;
        private System.Windows.Forms.ComboBox cboScheme;
        private System.Windows.Forms.Button cmdStartTiming;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem loadTimingRunToolStripMenuItem1;
        private System.Windows.Forms.ListBox lstDefinition;
        private System.Windows.Forms.DataGridView dgvUserTypes;
        private System.Windows.Forms.Button cmdNewUserType;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem method2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem method3ToolStripMenuItem;
    }
}