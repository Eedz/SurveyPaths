namespace SurveyPaths
{
    partial class EnterResponse
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
            this.cmdOK = new System.Windows.Forms.Button();
            this.rtbQuestionText = new System.Windows.Forms.RichTextBox();
            this.cboResponse = new System.Windows.Forms.ComboBox();
            this.cboVarName = new System.Windows.Forms.ComboBox();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(21, 272);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(61, 41);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // rtbQuestionText
            // 
            this.rtbQuestionText.Location = new System.Drawing.Point(21, 76);
            this.rtbQuestionText.Name = "rtbQuestionText";
            this.rtbQuestionText.Size = new System.Drawing.Size(301, 163);
            this.rtbQuestionText.TabIndex = 1;
            this.rtbQuestionText.Text = "";
            // 
            // cboResponse
            // 
            this.cboResponse.FormattingEnabled = true;
            this.cboResponse.Location = new System.Drawing.Point(21, 245);
            this.cboResponse.Name = "cboResponse";
            this.cboResponse.Size = new System.Drawing.Size(130, 21);
            this.cboResponse.TabIndex = 2;
            // 
            // cboVarName
            // 
            this.cboVarName.FormattingEnabled = true;
            this.cboVarName.Location = new System.Drawing.Point(24, 40);
            this.cboVarName.Name = "cboVarName";
            this.cboVarName.Size = new System.Drawing.Size(131, 21);
            this.cboVarName.TabIndex = 3;
            this.cboVarName.SelectedIndexChanged += new System.EventHandler(this.cboVarName_SelectedIndexChanged);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Location = new System.Drawing.Point(98, 273);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(88, 39);
            this.cmdCancel.TabIndex = 4;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // EnterResponse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 333);
            this.ControlBox = false;
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cboVarName);
            this.Controls.Add(this.cboResponse);
            this.Controls.Add(this.rtbQuestionText);
            this.Controls.Add(this.cmdOK);
            this.Name = "EnterResponse";
            this.Text = "EnterResponse";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.RichTextBox rtbQuestionText;
        private System.Windows.Forms.ComboBox cboResponse;
        private System.Windows.Forms.ComboBox cboVarName;
        private System.Windows.Forms.Button cmdCancel;
    }
}