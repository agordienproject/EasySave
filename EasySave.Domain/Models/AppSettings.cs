using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Domain.Models
{

    public class AppSettings
    {
        public AppSettings(Localization localization, DataFilesLocation dataFilesLocation, DataFilesTypes dataFilesTypes)
        {
            Localization = localization;
            DataFilesLocation = dataFilesLocation;
            DataFilesTypes = dataFilesTypes;
        }

        public Localization Localization { get; set; }
        public DataFilesLocation DataFilesLocation { get; set; }
        public DataFilesTypes DataFilesTypes { get; set; }
    }

    public class Localization
    {
        public Localization(string currentCulture)
        {
            CurrentCulture = currentCulture;
        }

        public string CurrentCulture { get; set; }
    }

    public class DataFilesLocation
    {
        public DataFilesLocation(string backupJobsFolderPath, string backupJobsJsonFileName, string statesFolderPath, string statesJsonFileName, string logsFolderPath)
        {
            BackupJobsFolderPath = backupJobsFolderPath;
            BackupJobsJsonFileName = backupJobsJsonFileName;
            StatesFolderPath = statesFolderPath;
            StatesJsonFileName = statesJsonFileName;
            LogsFolderPath = logsFolderPath;
        }

        public string BackupJobsFolderPath { get; set; }
        public string BackupJobsJsonFileName { get; set; }
        public string StatesFolderPath { get; set; }
        public string StatesJsonFileName { get; set; }
        public string LogsFolderPath { get; set; }
    }

    public class DataFilesTypes
    {
        public DataFilesTypes(string backupJobsFileType, string logsFileType, string statesFileType)
        {
            BackupJobsFileType = backupJobsFileType;
            LogsFileType = logsFileType;
            StatesFileType = statesFileType;
        }
        public string BackupJobsFileType { get; set; }
        public string LogsFileType { get; set; }
        public string StatesFileType { get; set; }
    }
}
