using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SeriesOrganizer.Properties;

namespace SeriesOrganizer
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            this.baseDirTextbox.Text = Settings.Default.baseDir;
            this.repositoryDirTextBox.Text = Settings.Default.repositoryDir;
        }

        private void baseDirButton_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.baseDirTextbox.Text = folderBrowserDialog1.SelectedPath+"\\";
            }
        }

        private void repositoryDirButton_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.repositoryDirTextBox.Text = folderBrowserDialog1.SelectedPath + "\\";
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Settings.Default.baseDir = this.baseDirTextbox.Text;
            Settings.Default.repositoryDir = this.repositoryDirTextBox.Text;
            Settings.Default.Save();
        }
    }
}
