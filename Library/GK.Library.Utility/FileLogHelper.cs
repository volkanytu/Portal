using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GK.Library.Utility
{
    public class FileLogHelper
    {
        public static void LogEvent(string message, string logPath)
        {
            try
            {
                string logPathFileToday = logPath + DateTime.Now.ToString("yyyy.MM.dd") + ".txt";

                string logMessage = String.Format("Log Date: {0}Log Message: {1} *-----------*-----------*-----------*", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + Environment.NewLine, Environment.NewLine + message + Environment.NewLine);

                if (!string.IsNullOrEmpty(logPath))
                {
                    if (!Directory.Exists(logPath))
                    {
                        Directory.CreateDirectory(logPath);
                    }
                    if (!File.Exists(logPathFileToday))
                    {
                        FileStream _fs = new FileStream(logPathFileToday, FileMode.OpenOrCreate);
                        _fs.Close();
                        File.AppendAllText(logPathFileToday, logMessage + Environment.NewLine);
                    }
                    else
                    {
                        FileStream _fs = new FileStream(logPathFileToday, FileMode.Open);
                        _fs.Close();
                        File.AppendAllText(logPathFileToday, logMessage + Environment.NewLine);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void LogFunction(string functionName, string message, string logPath)
        {
            try
            {
                string logDirectoryToday = logPath + DateTime.Now.ToString("yyyy.MM.dd");

                string logFilePathApplication = logDirectoryToday + @"\" + functionName + ".txt";

                string logMessage = String.Format("{0}{1}---------------------------------", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + Environment.NewLine, message + Environment.NewLine);


                if (!string.IsNullOrEmpty(logPath))
                {
                    if (!Directory.Exists(logDirectoryToday))
                    {
                        Directory.CreateDirectory(logDirectoryToday);
                    }

                    if (!File.Exists(logFilePathApplication))
                    {
                        //FileStream _fs = new FileStream(logFilePathApplication, FileMode.OpenOrCreate);
                        //_fs.Close();
                        File.AppendAllText(logFilePathApplication, logMessage + Environment.NewLine);
                    }
                    else
                    {
                        //FileStream _fs = new FileStream(logFilePathApplication, FileMode.Open);
                        //_fs.Close();
                        File.AppendAllText(logFilePathApplication, logMessage + Environment.NewLine);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


    }
}