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
            List<string> authorizedExtensions, 
            string businessAppName, 
            int maxKoToTransfert)
        {
            CurrentCulture = currentCulture;
            StateFolderPath = stateFolderPath;
            StateFileName = stateFileName;
            LogsFolderPath = logsFolderPath;
            LogsFileType = logsFileType;
            AuthorizedExtensions = authorizedExtensions;
            BusinessAppName = businessAppName;
            MaxKoToTransfert = maxKoToTransfert;
        }

        public string CurrentCulture { get; set; }
        public string StateFolderPath { get; set; }
        public string StateFileName { get; set; }
        public string LogsFolderPath { get; set; }
        public string LogsFileType { get; set; }
        public List<string> AuthorizedExtensions { get; set; }
        public string BusinessAppName { get; set; }
        public int MaxKoToTransfert { get; set; }
    }


}
