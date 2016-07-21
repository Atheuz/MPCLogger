using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace MPCLoggerCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            var filename = args[0]; //@"C:\Users\grave\Downloads\[HorribleSubs] Re Zero kara Hajimeru Isekai Seikatsu - 12 [1080p].mkv";// 
            Process process = new Process();
            var programfiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            var cccp = "Combined Community Codec Pack 64bit";
            var mpc = "MPC";
            var mpchc = Path.Combine(programfiles, cccp, mpc, "mpc-hc64.exe");
            process.StartInfo.FileName = mpchc;
            process.StartInfo.Arguments = "\""+filename+"\"";
            LogOpening(filename, "START");
            process.Start();

        }

        static void LogOpening(string fp, string startOrFinish)
        {
            // Get universal sorting time, example: 2013-05-05 17:53
            string currentDateTime = DateTime.Now.ToString("u");
            // Create the output string using the datetime and the filepath.
            string logOutput = String.Format("{0} - {1,-6} - {2}", currentDateTime, startOrFinish, fp);
            string userProfileDirectory = System.Environment.GetEnvironmentVariable("USERPROFILE");
            string DropBoxDirectory = DropboxPath();
            string logFile = Path.Combine(DropBoxDirectory, "MPC-HC-alt.log");

            if (!File.Exists(logFile))
            {
                File.Create(logFile);
            }
            // Append to the file.
            using (StreamWriter w = File.AppendText(logFile))
            {
                w.WriteLine(Regex.Replace(logOutput, @"\r\n?|\n", ""));
            }
        }

        static string DropboxPath()
        {
            try
            {
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                Console.WriteLine(appDataPath);
                string dbPath = System.IO.Path.Combine(appDataPath, "Dropbox\\host.db");
                var lines = System.IO.File.ReadAllLines(dbPath);
                var dbBase64Text = Convert.FromBase64String(lines[1]);
                string folderPath = System.Text.ASCIIEncoding.ASCII.GetString(dbBase64Text);
                return folderPath;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
