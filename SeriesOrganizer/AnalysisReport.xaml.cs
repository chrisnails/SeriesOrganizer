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
using System.IO;

namespace SeriesOrganizer
{
    /// <summary>
    /// Interaktionslogik für AnalysisReport.xaml
    /// </summary>
    public partial class AnalysisReport : Window
    {
        private string repositoryDir;
        public AnalysisReport(string repositoryDir)
        {
            this.repositoryDir = repositoryDir;
            InitializeComponent();
            this.fill_Analysis();
        }

        private void fill_Analysis()
        {
            analysisView.Items.Clear();
            foreach (String fileName in Directory.GetFiles(repositoryDir, "*.*", SearchOption.AllDirectories))
            {
                if (SeriesOrganizerEpisode.IsSeriesEpisode(fileName))
                {
                    SeriesOrganizerEpisode file = new SeriesOrganizerEpisode(fileName,true);
                    if ((fileName != file.SuggestedFolder + "\\" + file.FileName))
                    {
                        ListViewItem item = new ListViewItem();
                        item.Content = file;
                        item.MouseDoubleClick += new MouseButtonEventHandler(analysisView_MouseDoubleClick);
                        analysisView.Items.Add(item);
                    }
                }
            }
        }

        private void analysisView_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (analysisView.SelectedItems.Count == 1)
            {
                foreach (ListViewItem item in analysisView.SelectedItems)
                {
                    SeriesOrganizerEpisode episode = (SeriesOrganizerEpisode)item.Content;
                    MessageBox.Show("Full string:" + episode.fullFileString + "\nsuggested:" + episode.SuggestedFolder + "\\" + episode.FileName);
                }
            }
            
        }

        private void startAnalysis_Click(object sender, RoutedEventArgs e)
        {
            this.fill_Analysis();
        }
    }
}
