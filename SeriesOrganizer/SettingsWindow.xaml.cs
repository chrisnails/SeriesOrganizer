using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using SeriesOrganizer.Properties;

namespace SeriesOrganizer
{
    /// <summary>
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        public SettingsWindow()
        {
            InitializeComponent();
            folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog1.ShowNewFolderButton = true;
            
            this.baseDirTextBox.Text = Settings.Default.baseDir;
            this.repoDirTextBox.Text = Settings.Default.repositoryDir;
        }

        private void baseDirButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = this.baseDirTextBox.Text;
            if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.baseDirTextBox.Text = folderBrowserDialog1.SelectedPath + "\\";
            }
        }

        private void repositoryDirButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = this.repoDirTextBox.Text;
            if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.repoDirTextBox.Text = folderBrowserDialog1.SelectedPath + "\\";
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Settings.Default.baseDir = this.baseDirTextBox.Text;
            Settings.Default.repositoryDir = this.repoDirTextBox.Text;
            Settings.Default.Save();
            this.DialogResult = true;
        }
    }
}
