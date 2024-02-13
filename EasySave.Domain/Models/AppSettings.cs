using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Domain.Models
{
    public class AppSettings
    {
        public AppSettings(string backupJobsFolderPath, string backupJobsJsonFileName, string statesFolderPath, string statesJsonFileName, string logsFolderPath, string currentCulture)
        {
            BackupJobsFolderPath = backupJobsFolderPath;
            BackupJobsJsonFileName = backupJobsJsonFileName;
            StatesFolderPath = statesFolderPath;
            StatesJsonFileName = statesJsonFileName;
            LogsFolderPath = logsFolderPath;
            CurrentCulture = currentCulture;
        }

        public string BackupJobsFolderPath { get; set; }
        public string BackupJobsJsonFileName { get; set; }
        public string StatesFolderPath { get; set; }
        public string StatesJsonFileName { get; set; }
        public string LogsFolderPath { get; set; }
        public string CurrentCulture { get; set; }
    }
}
