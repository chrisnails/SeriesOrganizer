namespace SeriesOrganizer
{
    partial class SettingsForm
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
            this.baseDirLabel = new System.Windows.Forms.Label();
            this.repositoryDirLabel = new System.Windows.Forms.Label();
            this.baseDirTextbox = new System.Windows.Forms.TextBox();
            this.repositoryDirTextBox = new System.Windows.Forms.TextBox();
            this.baseDirButton = new System.Windows.Forms.Button();
            this.repositoryDirButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.saveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // baseDirLabel
            // 
            this.baseDirLabel.AutoSize = true;
            this.baseDirLabel.Location = new System.Drawing.Point(108, 55);
            this.baseDirLabel.Name = "baseDirLabel";
            this.baseDirLabel.Size = new System.Drawing.Size(43, 13);
            this.baseDirLabel.TabIndex = 0;
            this.baseDirLabel.Text = "baseDir";
            // 
            // repositoryDirLabel
            // 
            this.repositoryDirLabel.AutoSize = true;
            this.repositoryDirLabel.Location = new System.Drawing.Point(111, 100);
            this.repositoryDirLabel.Name = "repositoryDirLabel";
            this.repositoryDirLabel.Size = new System.Drawing.Size(65, 13);
            this.repositoryDirLabel.TabIndex = 1;
            this.repositoryDirLabel.Text = "repositoryDir";
            // 
            // baseDirTextbox
            // 
            this.baseDirTextbox.Location = new System.Drawing.Point(180, 57);
            this.baseDirTextbox.Name = "baseDirTextbox";
            this.baseDirTextbox.Size = new System.Drawing.Size(100, 20);
            this.baseDirTextbox.TabIndex = 2;
            // 
            // repositoryDirTextBox
            // 
            this.repositoryDirTextBox.Location = new System.Drawing.Point(182, 93);
            this.repositoryDirTextBox.Name = "repositoryDirTextBox";
            this.repositoryDirTextBox.Size = new System.Drawing.Size(100, 20);
            this.repositoryDirTextBox.TabIndex = 3;
            // 
            // baseDirButton
            // 
            this.baseDirButton.Location = new System.Drawing.Point(314, 55);
            this.baseDirButton.Name = "baseDirButton";
            this.baseDirButton.Size = new System.Drawing.Size(75, 23);
            this.baseDirButton.TabIndex = 4;
            this.baseDirButton.Text = "Browse";
            this.baseDirButton.UseVisualStyleBackColor = true;
            this.baseDirButton.Click += new System.EventHandler(this.baseDirButton_Click);
            // 
            // repositoryDirButton
            // 
            this.repositoryDirButton.Location = new System.Drawing.Point(314, 88);
            this.repositoryDirButton.Name = "repositoryDirButton";
            this.repositoryDirButton.Size = new System.Drawing.Size(75, 23);
            this.repositoryDirButton.TabIndex = 5;
            this.repositoryDirButton.Text = "Browse";
            this.repositoryDirButton.UseVisualStyleBackColor = true;
            this.repositoryDirButton.Click += new System.EventHandler(this.repositoryDirButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(282, 252);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 6;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 423);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.repositoryDirButton);
            this.Controls.Add(this.baseDirButton);
            this.Controls.Add(this.repositoryDirTextBox);
            this.Controls.Add(this.baseDirTextbox);
            this.Controls.Add(this.repositoryDirLabel);
            this.Controls.Add(this.baseDirLabel);
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SettingsForm";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label baseDirLabel;
        private System.Windows.Forms.Label repositoryDirLabel;
        private System.Windows.Forms.TextBox baseDirTextbox;
        private System.Windows.Forms.TextBox repositoryDirTextBox;
        private System.Windows.Forms.Button baseDirButton;
        private System.Windows.Forms.Button repositoryDirButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button saveButton;
    }
}