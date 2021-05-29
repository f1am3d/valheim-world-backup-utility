using System;

namespace ValheimWorldBackupUtility
{
    public class ConsoleController
    {
        public void sayHello()
        {
            Console.WriteLine($"Valheim World Backup Utility v.{Program.VERSION}");
        }

        public void readOptions()
        {
            readWorldName();
            readInterval();
        }

        public void readWorldName()
        {
            Console.WriteLine("World name (empty to backup all):");

            string input = Console.ReadLine();

            Program.options.setWorldName(input);
        }

        public void readInterval()
        {
            Console.WriteLine("Interval in hours (1h - default):");

            try
            {
                string input = Console.ReadLine();
                int interval = int.Parse(input ?? "1");
                
                bool result = Program.options.setInterval(interval);
                
                if(!result) throw new Exception();
            }
            catch
            {
                Console.WriteLine("Invalid interval");
                readInterval();
            }
        }

        public void readExitConfirmation()
        {
            Console.WriteLine("Write \"QUIT\" to stop the app.");

            string input = Console.ReadLine();

            if (input.ToLower() != "quit") readExitConfirmation();
        }
    }
}