﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


//added by me
using System.IO;
using System.Text.RegularExpressions;
using SeriesOrganizer.Properties;
using SeriesOrganizer.Localization;


namespace SeriesOrganizer
{
    public partial class MainForm : Form
    {
        private string baseDir;
        private string repositoryDir;

        public MainForm()
        {
            InitializeComponent();

            baseDir = Settings.Default.baseDir;
            repositoryDir = Settings.Default.repositoryDir;

            fileSystemWatcher1.Path = baseDir;
        }

        private void fillView()
        {
            listView1.Items.Clear();
            foreach (String fileName in Directory.GetFiles(baseDir))
            {
                if (SeriesOrganizerEpisode.IsSeriesEpisode(fileName))
                {

                    SeriesOrganizerEpisode file = new SeriesOrganizerEpisode(fileName);

                    //Insert into View
                    string[] row = { file.FileName, file.SeriesName, file.Season.ToString(), file.Episode.ToString(), file.SuggestedFolder, file.SuggestedFileName };
                    var listViewItem = new ListViewItem(row);

                    listView1.Items.Add(listViewItem);

                }
            }
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            this.toolStripStatusLabel1.Text = listView1.Items.Count + strings.countItemsFound;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            fillView();
        }


        private void moveSelectedEpisodeToSuggestedFolder()
        {
            if (listView1.SelectedItems.Count == 1)
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    try
                    {
                        Directory.Move(baseDir + item.SubItems[0].Text, item.SubItems[4].Text + "\\" + Path.GetFileName(item.SubItems[0].Text));
                        // MessageBox.Show(item.SubItems[0].Text + "\n\n ----->\n\n " + item.SubItems[4].Text);
                    }
                    catch (DirectoryNotFoundException)
                    {
                        if (MessageBox.Show(strings.directoryMissingCreate, strings.createDirectory, MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            Directory.CreateDirectory(item.SubItems[4].Text);
                        }
                    }
                    //   fillView(); update is now handled by the file system watcher
                }
            }
        }



        private void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {
            this.fillView();
        }

        private void downloadsEntpackenToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string notAccessable = "";

            //Disable the file system watcher, so the view isn't filled multiple times
            fileSystemWatcher1.EnableRaisingEvents = false;

            foreach (String dirName in Directory.GetDirectories(baseDir))
            {
                Match match = Regex.Match(Path.GetFileName(dirName), @"(S[0-9][0-9]E[0-9][0-9])", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    foreach (String fileName in Directory.GetFiles(dirName))
                    {

                        if (SeriesOrganizerEpisode.IsSeriesEpisode(fileName))
                        {
                            try
                            {
                                Directory.Move(fileName, baseDir + Path.GetFileName(fileName));
                                Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(dirName,
                                    Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                                    Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                            }
                            catch (IOException)
                            {
                                notAccessable += fileName + "\n";
                            }
                        }
                    }
                }
            }
            if (notAccessable != "")
                MessageBox.Show(notAccessable);
            fillView();

            //Re-Enable The fileSystemWatcher
            fileSystemWatcher1.EnableRaisingEvents = true;
        }



        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm settings = new SettingsForm();
            settings.ShowDialog();
        }

        #region BackgroundWorker for Repository Analysis
        /// <summary>
        /// BackgorundWorker and Events für Repository Analysis
        /// </summary>

        private void analyzeRepositoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = strings.analyzingRepository;
            toolStripProgressBar1.Visible = true;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            BackgroundWorker worker = sender as BackgroundWorker;

            string errors = "";
            int anzahl = Directory.GetFiles(repositoryDir, "*.*", SearchOption.AllDirectories).Length;
            int countProgress = 0;
            foreach (String fileName in Directory.GetFiles(repositoryDir, "*.*", SearchOption.AllDirectories))
            {
                if (SeriesOrganizerEpisode.IsSeriesEpisode(fileName))
                {

                    SeriesOrganizerEpisode file = new SeriesOrganizerEpisode(fileName, true);

                    if (fileName != file.SuggestedFolder + "\\" + file.FileName)
                    {
                        errors += fileName + "\n";
                    }

                }
                countProgress++;

                double percent = (100 / (double)anzahl) * countProgress;

                worker.ReportProgress((int)Math.Round(percent));

            }
            MessageBox.Show(errors);
            worker.ReportProgress(101);

        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            this.toolStripProgressBar1.Value = e.ProgressPercentage;
            if (this.toolStripProgressBar1.Value == 101)
            {
                this.toolStripProgressBar1.Visible = false;
                this.toolStripStatusLabel1.Text = strings.analysisComplete;
            }
        }

        #endregion

        private void renameSelectedEpisode()
        {

            if (listView1.SelectedItems.Count == 1)
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    try
                    {
                        Directory.Move(baseDir + item.SubItems[0].Text, baseDir + this.showRenameDialog(strings.filename, strings.filename, item.SubItems[0].Text));
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }

                }
            }
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.moveSelectedEpisodeToSuggestedFolder();
        }

        private void renameFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.renameSelectedEpisode();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            this.moveSelectedEpisodeToSuggestedFolder();
        }

        public string showRenameDialog(string text, string caption, string currentValue = "")
        {
            Form prompt = new Form();
            prompt.Width = 500;
            prompt.Height = 150;
            prompt.Text = caption;
            prompt.StartPosition = FormStartPosition.CenterParent;
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70 };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            textBox.Text = currentValue;
            prompt.ShowDialog();
            return textBox.Text;
        }


    }
}
