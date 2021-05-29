using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Timers;

namespace ValheimWorldBackupUtility
{
    internal enum FileExt
    {
        fwl,
        db
    }

    public class BackupController
    {
        private Timer _timer;
        private string _worldsRootPath;
        private string _backupPath;

        private string _worldPath
        {
            get
            {
                if (Program.options.worldName != null)
                {
                    return Path.Combine(_worldsRootPath, $"{Program.options.worldName}.fwl");
                }

                return null;
            }
        }

        public BackupController()
        {
            initializePaths();
        }

        public void initializePaths()
        {
            // Server worlds path
            _worldsRootPath = Path.GetFullPath(
                Path.Combine(
                    Environment.ExpandEnvironmentVariables("%AppData%"),
                    @"..\LocalLow\IronGate\Valheim\worlds"
                )
            );

            if (Directory.Exists(_worldsRootPath) == false)
            {
                var message = $"Server worlds directory not found in {_worldsRootPath}";
                
                Program.log.addLog(message);
                throw new DirectoryNotFoundException(message);
            }

            _backupPath = Path.Combine(_worldsRootPath, "backups");
        }

        public void start()
        {
            int interval = Program.options.interval ?? 1;

            if (Program.options.worldName != null && File.Exists(_worldPath) == false)
            {
                var message = $"World with name {Program.options.worldName} not found";
                
                Program.log.addLog(message);
                throw new FileNotFoundException(message);
            }

            Console.WriteLine("Creating initial backup...");

            createBackup();
            
            Console.WriteLine($"Waiting...");
            
            //                 ms   * s  * m  * h
            _timer = new Timer(1000 * 60 * 60 * interval);
            _timer.Elapsed += createBackup;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void createBackup(object source = null, ElapsedEventArgs e = null)
        {
            string[] files = Directory.GetFiles(_worldsRootPath);
            DateTime date = e?.SignalTime ?? DateTime.Now;
            string folderName = $"backup-{string.Format("{0:dd-MM-yyyy_HH-mm}", date)}";
            string folderPath = Path.Combine(_backupPath, folderName);
            
            
            // Create new dir
            if (Directory.Exists(folderPath) == false)
            {
                Directory.CreateDirectory(folderPath);
            }


            foreach (string file in files)
            {
                if (!Program.options.backupEverything)
                {
                    if (Regex.Match(file, Program.options.worldName).Success)
                    {
                        backupFile(file, folderName, folderPath);
                    }

                    continue;
                }

                backupFile(file, folderName, folderPath);
            }

            var message = $"Successfully created backup {folderName}";
            
            Console.WriteLine(message);
            Program.log.addLog(message);
        }

        public void backupFile(string filePath, string folderName, string folderPath)
        {
            FileExt? type = null;
            string fileName;

            // Detect file extension
            if (Regex.Match(filePath, @"\.fwl$").Success)
            {
                type = FileExt.fwl;
            }

            if (Regex.Match(filePath, @"\.db$").Success)
            {
                type = FileExt.db;
            }

            fileName = Regex.Match(filePath, @"\\([\d\w\.]+)$").Groups[1]?.Value;
            
            if (type == null || fileName == null) return;

            string newFilePath = Path.Combine(folderPath, fileName);

            
            // Copy file
            try
            {
                File.Copy(filePath, newFilePath);
            }
            catch (Exception error)
            {
                var message = $"Unable to copy file {filePath}";
                
                Program.log.addLog(message);
                Program.log.addLog(error.StackTrace);
            }
        }

        public void stop()
        {
            _timer.Dispose();
        }
    }
}