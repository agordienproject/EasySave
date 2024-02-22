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
            List<string> encryptedExtensions, 
            List<string> prioritizedExtensions,
            string businessAppName, 
            int maxKoToTransfert) 
        {
            CurrentCulture = currentCulture;
            StateFolderPath = stateFolderPath;
            StateFileName = stateFileName;
            LogsFolderPath = logsFolderPath;
            LogsFileType = logsFileType;
            EncryptedExtensions = encryptedExtensions;
            BusinessAppName = businessAppName;
            MaxKoToTransfert = maxKoToTransfert;
            PrioritizedExtensions = prioritizedExtensions;
        }

        public string CurrentCulture { get; set; }
        public string StateFolderPath { get; set; }
        public string StateFileName { get; set; }
        public string LogsFolderPath { get; set; }
        public string LogsFileType { get; set; }
        public List<string> EncryptedExtensions { get; set; }
        public string BusinessAppName { get; set; }
        public int MaxKoToTransfert { get; set; }
        public List<string> PrioritizedExtensions { get; set; }

    }


}
