using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GK.Library.Common
{
    public class FileHelper
    {
        private static System.Threading.ReaderWriterLockSlim cacheLock = new System.Threading.ReaderWriterLockSlim();

        public static void WriteToText(string fileName, string message, string logPath)
        {
            if (string.IsNullOrWhiteSpace(logPath))
            {
                return;
            }

            string logDirectoryToday = string.Format("{0}{1}", logPath, DateTime.Now.ToString("yyyy.MM.dd"));
            string logFilePathApplication = string.Format(@"{0}\{1}.txt", logDirectoryToday, fileName);

            DirectoryIsNotExistCreateIt(logDirectoryToday);
            FileIsNotExistCreateIt(logFilePathApplication);
            AddMessageToTheFile(logFilePathApplication, message);
        }

        private static void DirectoryIsNotExistCreateIt(string path)
        {
            cacheLock.EnterUpgradeableReadLock();

            try
            {
                if (!Directory.Exists(path))
                {
                    cacheLock.EnterWriteLock();

                    try
                    {
                        Directory.CreateDirectory(path);
                    }
                    finally
                    {
                        cacheLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                cacheLock.ExitUpgradeableReadLock();
            }
        }

        private static void FileIsNotExistCreateIt(string path)
        {
            cacheLock.EnterUpgradeableReadLock();

            try
            {
                if (!File.Exists(path))
                {
                    cacheLock.EnterWriteLock();

                    try
                    {
                        File.Create(path).Dispose();
                    }
                    finally
                    {
                        cacheLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                cacheLock.ExitUpgradeableReadLock();
            }
        }

        private static void AddMessageToTheFile(string path, string message)
        {
            cacheLock.EnterWriteLock();

            try
            {
                using (FileStream file = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(file))
                    {
                        writer.AutoFlush = true;
                        writer.Write(message + Environment.NewLine);
                    }
                }
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }

    }
}