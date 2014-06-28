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
using System.Windows.Navigation;
using System.Windows.Shapes;


using System.IO;
using System.Text.RegularExpressions;

using SeriesOrganizer.Properties;
using SeriesOrganizer.Localization;

namespace SeriesOrganizer
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private struct EpisodeMove
        {
            public string fromDir;
            public string toDir;

            public EpisodeMove(string fromDir, string toDir)
            {
                this.fromDir = fromDir;
                this.toDir = toDir;
            }
        }

        private string filter;

        private List<EpisodeMove> moveHistory;

        private string baseDir;
        private string repositoryDir;

        private FileSystemWatcher fileSystemWatcher1;
        

        public MainWindow()
        {
            InitializeComponent();
            baseDir = Settings.Default.baseDir;
            repositoryDir = Settings.Default.repositoryDir;

            fileSystemWatcher1 = new FileSystemWatcher();
            fileSystemWatcher1.Path = baseDir;
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.Changed += new System.IO.FileSystemEventHandler(this.fileSystemWatcher1_Changed);
            this.fileSystemWatcher1.Created += new System.IO.FileSystemEventHandler(this.fileSystemWatcher1_Changed);
            this.fileSystemWatcher1.Deleted += new System.IO.FileSystemEventHandler(this.fileSystemWatcher1_Changed);
            this.fileSystemWatcher1.Renamed += new System.IO.RenamedEventHandler(this.fileSystemWatcher1_Changed);

            

            moveHistory = new List<EpisodeMove>();
            filter = "";
            fillView();
        }

        private void fillView()
        {
            listView1.Items.Clear();
            foreach (String fileName in Directory.GetFiles(baseDir))
            {
                if (SeriesOrganizerEpisode.IsSeriesEpisode(fileName))
                {
                    SeriesOrganizerEpisode file = new SeriesOrganizerEpisode(fileName);
                    ListViewItem item = new ListViewItem();
                    item.Content = file;
                    item.MouseDoubleClick += new MouseButtonEventHandler(listView1_MouseDoubleClick);
                    item.ContextMenu = (ContextMenu)this.Resources["EpsodeContextMenu"];

                    listView1.Items.Add(item);
                }
            }
            
            this.toolStripStatusLabel1.Content = listView1.Items.Count + strings.countItemsFound;
        }


       


        public bool Contains(object de)
        {

            ListViewItem lvi = de as ListViewItem;
            SeriesOrganizerEpisode episode = (SeriesOrganizerEpisode)lvi.Content;
                        //Return members whose Orders have not been filled

            if (episode.FileName.IndexOf(this.filter, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void setFilter(String text)
        {
            this.filter = text;
            listView1.Items.Filter = new Predicate<object>(Contains);
        }

        private void selectLetter(Key key)
        {

        }

        private void moveSelectedEpisodeToSuggestedFolder()
        {
            if (listView1.SelectedItems.Count == 1)
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    SeriesOrganizerEpisode episode = (SeriesOrganizerEpisode)item.Content;

                    try
                    {
                        Directory.Move(baseDir + episode.FileName, episode.SuggestedFolder + "\\" + episode.FileName);
                        moveHistory.Add(new EpisodeMove(baseDir + episode.FileName,episode.SuggestedFolder + "\\" + episode.FileName));
                       // MessageBox.Show(baseDir + ((SeriesOrganizerEpisode)item.Content).FileName + "\n\n ----->\n\n " + ((SeriesOrganizerEpisode)item.Content).SuggestedFolder + "\\" + ((SeriesOrganizerEpisode)item.Content).FileName);
                    }
                    catch (DirectoryNotFoundException)
                    {
                        

                        if (MessageBox.Show(strings.directoryMissingCreate, strings.createDirectory, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            Directory.CreateDirectory(episode.SuggestedFolder);
                            this.moveSelectedEpisodeToSuggestedFolder();
                        }
                    }
                    //   fillView(); update is now handled by the file system watcher
                }
            }
        }

        private void unpackDownloads()
        {
            string notAccessable = "";

            //Disable the file system watcher, so the view isn't filled multiple times
            fileSystemWatcher1.EnableRaisingEvents = false;

            foreach (String dirName in Directory.GetDirectories(baseDir))
            {
                Match match = Regex.Match(System.IO.Path.GetFileName(dirName), @"(S[0-9][0-9]E[0-9][0-9])", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    foreach (String fileName in Directory.GetFiles(dirName))
                    {

                        if (SeriesOrganizerEpisode.IsSeriesEpisode(fileName))
                        {
                            try
                            {
                                Directory.Move(fileName, baseDir + System.IO.Path.GetFileName(fileName));
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

        #region BackgroundWorker for Repository Analysis
        /// <summary>
        /// BackgorundWorker and Events für Repository Analysis
        /// </summary>

        private void analyzeRepositoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Content = strings.analyzingRepository;
            toolStripProgressBar1.Visibility = System.Windows.Visibility.Visible;
            AnalysisReport ar = new AnalysisReport(repositoryDir);
            ar.Show();
            
        }

        

        #endregion

        #region EventHandlers

        private void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {
            this.listView1.Dispatcher.Invoke(new Action(() => { this.fillView(); }));
        }

        private void listView1_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            this.moveSelectedEpisodeToSuggestedFolder();
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow();
            if (settings.ShowDialog() == true)
            {
                baseDir = Settings.Default.baseDir;
                repositoryDir = Settings.Default.repositoryDir;
                this.fillView();
            }
        }

        private void undoMoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (moveHistory.Count > 0)
            {
                EpisodeMove lastMove = moveHistory[moveHistory.Count - 1];
                moveHistory.RemoveAt(moveHistory.Count - 1);
                Directory.Move(lastMove.toDir, lastMove.fromDir);
            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void unpackButton_Click(object sender, RoutedEventArgs e)
        {
            this.unpackDownloads();
        }

     

        private void unpackButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.unpackDownloads();
            Application.Current.Shutdown();
        }

     

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           // if (searchField.Text != "TextBox")
           // {
                setFilter(searchField.Text);
           // }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                setFilter(searchField.Text);
            }
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            selectLetter(e.Key);
        }

        #endregion

    }
}
