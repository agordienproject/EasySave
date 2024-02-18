using System.Collections.ObjectModel;

namespace EasySave.Models
{
    public class AppSettings
    {
        public AppSettings(string currentCulture, 
            string stateFolderPath, 
            string stateFileName, 
            string logsFolderPath, 
            string logsFileType, 
            List<string> authorizedExtensions)
        {
            CurrentCulture = currentCulture;
            StateFolderPath = stateFolderPath;
            StateFileName = stateFileName;
            LogsFolderPath = logsFolderPath;
            LogsFileType = logsFileType;
            AuthorizedExtensions = authorizedExtensions;
        }

        public string CurrentCulture { get; set; }
        public string StateFolderPath { get; set; }
        public string StateFileName { get; set; }
        public string LogsFolderPath { get; set; }
        public string LogsFileType { get; set; }
        public List<string> AuthorizedExtensions { get; set; }
    }


}
