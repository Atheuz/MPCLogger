using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace MPCLoggerCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            var filename = @"C:\Users\Lasse\Downloads\The.Flash.2014.S02E08.720p.HDTV.X264-DIMENSION.mkv"; // args[0];  //
            Process process = new Process();
            var programfiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            var cccp = "Combined Community Codec Pack";
            var mpc = "MPC";
            var mpchc = Path.Combine(programfiles, cccp, mpc, "mpc-hc.exe");
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
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
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
