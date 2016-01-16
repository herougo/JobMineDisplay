namespace JobMineDisplay
{
    partial class Form1
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
            this.btnXmlToNewDatabase = new System.Windows.Forms.Button();
            this.btnXmlToPastDatabase = new System.Windows.Forms.Button();
            this.cbSqlWhere = new System.Windows.Forms.ComboBox();
            this.btnAltDisplay = new System.Windows.Forms.Button();
            this.btnRank1 = new System.Windows.Forms.Button();
            this.btnRank2 = new System.Windows.Forms.Button();
            this.btnRank3 = new System.Windows.Forms.Button();
            this.btnRank4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnXmlToNewDatabase
            // 
            this.btnXmlToNewDatabase.Location = new System.Drawing.Point(2, 2);
            this.btnXmlToNewDatabase.Name = "btnXmlToNewDatabase";
            this.btnXmlToNewDatabase.Size = new System.Drawing.Size(119, 33);
            this.btnXmlToNewDatabase.TabIndex = 9;
            this.btnXmlToNewDatabase.Text = "Xml to New Database";
            this.btnXmlToNewDatabase.UseVisualStyleBackColor = true;
            this.btnXmlToNewDatabase.Click += new System.EventHandler(this.btnXmlToNewDatabase_Click);
            // 
            // btnXmlToPastDatabase
            // 
            this.btnXmlToPastDatabase.Location = new System.Drawing.Point(127, 2);
            this.btnXmlToPastDatabase.Name = "btnXmlToPastDatabase";
            this.btnXmlToPastDatabase.Size = new System.Drawing.Size(119, 33);
            this.btnXmlToPastDatabase.TabIndex = 8;
            this.btnXmlToPastDatabase.Text = "Xml to Past Database";
            this.btnXmlToPastDatabase.UseVisualStyleBackColor = true;
            this.btnXmlToPastDatabase.Click += new System.EventHandler(this.btnXmlToPastDatabase_Click);
            // 
            // cbSqlWhere
            // 
            this.cbSqlWhere.FormattingEnabled = true;
            this.cbSqlWhere.Items.AddRange(new object[] {
            "SELECT * FROM tblPastJobPosting",
            "SELECT * FROM tblNewJobPosting",
            "SELECT * FROM tblNewJobPosting WHERE application_status=\'Posted\'",
            "SELECT * FROM tblPastJobPosting WHERE unit_name LIKE \'%Head Office%\'"});
            this.cbSqlWhere.Location = new System.Drawing.Point(252, 9);
            this.cbSqlWhere.Name = "cbSqlWhere";
            this.cbSqlWhere.Size = new System.Drawing.Size(420, 21);
            this.cbSqlWhere.TabIndex = 10;
            this.cbSqlWhere.Text = "SELECT * FROM tblPastJobPosting";
            // 
            // btnAltDisplay
            // 
            this.btnAltDisplay.Location = new System.Drawing.Point(891, 2);
            this.btnAltDisplay.Name = "btnAltDisplay";
            this.btnAltDisplay.Size = new System.Drawing.Size(101, 33);
            this.btnAltDisplay.TabIndex = 11;
            this.btnAltDisplay.Text = "Alternate View";
            this.btnAltDisplay.UseVisualStyleBackColor = true;
            this.btnAltDisplay.Click += new System.EventHandler(this.btnAltDisplay_Click);
            // 
            // btnRank1
            // 
            this.btnRank1.BackColor = System.Drawing.Color.Lime;
            this.btnRank1.Location = new System.Drawing.Point(848, 2);
            this.btnRank1.Name = "btnRank1";
            this.btnRank1.Size = new System.Drawing.Size(37, 33);
            this.btnRank1.TabIndex = 12;
            this.btnRank1.UseVisualStyleBackColor = false;
            this.btnRank1.Click += new System.EventHandler(this.btnRank1_Click);
            // 
            // btnRank2
            // 
            this.btnRank2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnRank2.Location = new System.Drawing.Point(805, 2);
            this.btnRank2.Name = "btnRank2";
            this.btnRank2.Size = new System.Drawing.Size(37, 33);
            this.btnRank2.TabIndex = 13;
            this.btnRank2.UseVisualStyleBackColor = false;
            this.btnRank2.Click += new System.EventHandler(this.btnRank2_Click);
            // 
            // btnRank3
            // 
            this.btnRank3.BackColor = System.Drawing.Color.Yellow;
            this.btnRank3.Location = new System.Drawing.Point(762, 2);
            this.btnRank3.Name = "btnRank3";
            this.btnRank3.Size = new System.Drawing.Size(37, 33);
            this.btnRank3.TabIndex = 14;
            this.btnRank3.UseVisualStyleBackColor = false;
            this.btnRank3.Click += new System.EventHandler(this.btnRank3_Click);
            // 
            // btnRank4
            // 
            this.btnRank4.BackColor = System.Drawing.Color.Red;
            this.btnRank4.Location = new System.Drawing.Point(719, 2);
            this.btnRank4.Name = "btnRank4";
            this.btnRank4.Size = new System.Drawing.Size(37, 33);
            this.btnRank4.TabIndex = 15;
            this.btnRank4.UseVisualStyleBackColor = false;
            this.btnRank4.Click += new System.EventHandler(this.btnRank4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 562);
            this.Controls.Add(this.btnRank4);
            this.Controls.Add(this.btnRank3);
            this.Controls.Add(this.btnRank2);
            this.Controls.Add(this.btnRank1);
            this.Controls.Add(this.btnAltDisplay);
            this.Controls.Add(this.cbSqlWhere);
            this.Controls.Add(this.btnXmlToPastDatabase);
            this.Controls.Add(this.btnXmlToNewDatabase);
            this.Name = "Form1";
            this.Text = "n";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnXmlToNewDatabase;
        private System.Windows.Forms.Button btnXmlToPastDatabase;
        private System.Windows.Forms.ComboBox cbSqlWhere;
        private System.Windows.Forms.Button btnAltDisplay;
        private System.Windows.Forms.Button btnRank1;
        private System.Windows.Forms.Button btnRank2;
        private System.Windows.Forms.Button btnRank3;
        private System.Windows.Forms.Button btnRank4;


    }
}

