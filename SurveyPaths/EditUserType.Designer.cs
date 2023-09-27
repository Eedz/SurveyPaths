namespace SurveyPaths
{
    partial class EditUserType
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
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboSurvey = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lstDefinition = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmdAddResponse = new System.Windows.Forms.Button();
            this.cmdSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(102, 64);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(191, 20);
            this.txtDescription.TabIndex = 0;
            // 
            // txtWeight
            // 
            this.txtWeight.Location = new System.Drawing.Point(102, 90);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.Size = new System.Drawing.Size(191, 20);
            this.txtWeight.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Description";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Weight";
            // 
            // cboSurvey
            // 
            this.cboSurvey.FormattingEnabled = true;
            this.cboSurvey.Location = new System.Drawing.Point(102, 37);
            this.cboSurvey.Name = "cboSurvey";
            this.cboSurvey.Size = new System.Drawing.Size(191, 21);
            this.cboSurvey.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Survey";
            // 
            // lstDefinition
            // 
            this.lstDefinition.FormattingEnabled = true;
            this.lstDefinition.Location = new System.Drawing.Point(102, 116);
            this.lstDefinition.Name = "lstDefinition";
            this.lstDefinition.Size = new System.Drawing.Size(106, 173);
            this.lstDefinition.TabIndex = 6;
            this.lstDefinition.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lstDefinition_KeyPress);
            this.lstDefinition.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstDefinition_KeyUp);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Definition";
            // 
            // cmdAddResponse
            // 
            this.cmdAddResponse.Location = new System.Drawing.Point(214, 116);
            this.cmdAddResponse.Name = "cmdAddResponse";
            this.cmdAddResponse.Size = new System.Drawing.Size(19, 23);
            this.cmdAddResponse.TabIndex = 8;
            this.cmdAddResponse.Text = "+";
            this.cmdAddResponse.UseVisualStyleBackColor = true;
            this.cmdAddResponse.Click += new System.EventHandler(this.cmdAddResponse_Click);
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(255, 277);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(75, 23);
            this.cmdSave.TabIndex = 9;
            this.cmdSave.Text = "Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // frmEditUserType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 312);
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.cmdAddResponse);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lstDefinition);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboSurvey);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtWeight);
            this.Controls.Add(this.txtDescription);
            this.Name = "frmEditUserType";
            this.Text = "Edit User Type Definition";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboSurvey;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lstDefinition;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button cmdAddResponse;
        private System.Windows.Forms.Button cmdSave;
    }
}