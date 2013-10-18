using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using SeriesOrganizer.Properties;


namespace SeriesOrganizer
{
    class SeriesOrganizerEpisode
    {

        private string fileName;
        private string seriesname;
        private int season;
        private int episode;
        private string suggestedfolder;
        private string suggestedFilename;


        public int Season
        {
            get
            {
                return season;
            }
        }

        public int Episode
        {
            get
            {
                return episode;
            }
        }

        public string SuggestedFolder
        {
            get
            {
                return suggestedfolder;
            }
        }

        public string SeriesName
        {
            get
            {
                return seriesname;
            }
        }

        public string FileName
        {
            get
            {
                return Path.GetFileName(fileName);
            }
        }

        public string SuggestedFileName
        {
            get
            {
                return suggestedFilename;
            }
        }

        public SeriesOrganizerEpisode(string fileName, bool ignoreSeasonOne = false)
        {
            this.fileName = fileName;
            season = 0;
            episode = 0;
            seriesname = "";
            Match match = Regex.Match(Path.GetFileName(fileName), @"(S[0-9][0-9]E[0-9][0-9])", RegexOptions.IgnoreCase);



            //Name, Season and Episode
            //************************
            string key = "";
            if (match.Success)
            {

                key = match.Groups[0].Value;
                season = Int16.Parse(key.Substring(1, 2));
                episode = Int16.Parse(key.Substring(4, 2));
                seriesname = Path.GetFileName(fileName).Substring(0, match.Index).Replace(".", " ");

                seriesname = seriesname.Trim();

                if (seriesname.Length > 4) // For Titles like "Mom"
                {
                    for (int j = 2000; j < 2020; j++)
                    {
                        if (seriesname.Substring(seriesname.Length - 4, 4) == j.ToString())
                        {
                            seriesname = seriesname.Substring(0, seriesname.Length - 4);
                        }
                    }
                }

                seriesname = seriesname.Trim();

                //TitleCase / Proper Case
                TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
                seriesname = myTI.ToTitleCase(seriesname);

            }


            //Suggested Folder
            //****************
            suggestedfolder = "";

            if (ignoreSeasonOne)
            {
                if (Directory.Exists(Settings.Default.repositoryDir + seriesname + "\\" + seriesname + " Season 2"))
                {
                    suggestedfolder = Settings.Default.baseDir + "Serien\\" + seriesname + "\\" + seriesname + " Season " + season;
                }
                else
                {
                    suggestedfolder = Settings.Default.baseDir + "Serien\\" + seriesname;
                }
            }
            else
            {
                if (season == 1)
                {
                    suggestedfolder = Settings.Default.baseDir + "Serien\\" + seriesname;
                }
                else
                {
                    suggestedfolder = Settings.Default.baseDir + "Serien\\" + seriesname + "\\" + seriesname + " Season " + season;
                }
            }




            //Suggested Filename
            //******************
            suggestedFilename = seriesname + " " + key + fileName.Substring(fileName.Length - 4);


        }

        public static bool IsSeriesEpisode(string fileName)
        {
            string[] fileFormats = new string[3] { ".mkv", ".avi", ".TS" };
            if (fileFormats.Contains(Path.GetExtension(fileName)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
