using System;
using System.IO;

namespace ValheimWorldBackupUtility
{
    
    abstract class Program
    {
        public static readonly string VERSION = "0.9.0";
        public static readonly int APP_ID = 892970;
        public static BackupController backup = new BackupController();
        public static OptionsController options = new OptionsController();
        public static ConsoleController console = new ConsoleController();
        
        
        static void Main(string[] args)
        {
            console.sayHello();
            console.readOptions();

            try
            {
                backup.start();
            }
            catch (FileNotFoundException error)
            {
                Console.WriteLine(error.Message);
            }
            catch (DirectoryNotFoundException error)
            {
                Console.WriteLine(error.Message);
            }
            catch
            {
                Console.WriteLine("Unknown error.");
                exit();
            }
            
            console.readExitConfirmation();
        }

        public static void exit()
        {
            backup.stop();
            Environment.Exit(0);  
        }
    }
}