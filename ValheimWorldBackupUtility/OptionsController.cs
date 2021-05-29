namespace ValheimWorldBackupUtility
{
    public class OptionsController
    {
        public string ?worldName;
        public int ?interval;
        public bool backupEverything
        {
            get { return Program.options.worldName == null; }
        }

        public void setWorldName(string input)
        {
            if (input.Length < 1)
            {
                return;
            }

            worldName = input;
        }

        public bool setInterval(int input)
        {
            if (input < 1)
            {
                return false;
            }

            interval = input;
            return true;
        }
    }
}