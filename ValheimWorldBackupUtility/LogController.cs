using System;
using System.IO;

namespace ValheimWorldBackupUtility
{
    public class LogController
    {
        private string _logFileName = "valheim-world-backup.log";

        public LogController()
        {
            File.CreateText(_logFileName).Close();
        }

        public void addLog(string log)
        {
            DateTime date = DateTime.Now;
            string dateString = string.Format("{0:dd-MM-yyyy HH:mm:ss}", date);
            string logString = $"{dateString} {log}";

            File.WriteAllLines(_logFileName, new[] {logString});
        }
    }
}