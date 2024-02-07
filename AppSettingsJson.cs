using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave
{
    public static class AppSettingsJson
    {
        public static IConfigurationRoot GetAppSettings()
        {
            string applicationExeDirectory = GetApplicationExeDirectory();

            var builder = new ConfigurationBuilder()
            .SetBasePath(applicationExeDirectory)
            .AddJsonFile("appsettings.json");

            return builder.Build();
        }

        private static string GetApplicationExeDirectory()
        {
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var appRoot = Path.GetDirectoryName(location);

            return appRoot;
        }

        public static string GetBackupJobsFilePath()
        {
            string appRoot = GetApplicationExeDirectory();

            string folderPath = GetAppSettings()["BackupJobsFolderPath"];
            string fileName = GetAppSettings()["BackupJobsJsonFileName"];
            
            string relativeFilePath = Path.Combine(folderPath, fileName);

            return Path.Combine(appRoot, relativeFilePath);
        }

        public static string GetStatesFilePath() 
        {
            string appRoot = GetApplicationExeDirectory();

            string folderPath = GetAppSettings()["StatesFolderPath"];
            string fileName = GetAppSettings()["StatesJsonFileName"];

            string relativeFilePath = Path.Combine(folderPath, fileName);

            return Path.Combine(appRoot, relativeFilePath);
        }

        public static string GetLogsFilePath()
        {
            string appRoot = GetApplicationExeDirectory();

            string folderPath = GetAppSettings()["LogsFolderPath"];
            string fileName = $"{DateTime.Now:dd_MM_yyyy}.json";

            string relativeFilePath = Path.Combine(folderPath, fileName);

            return Path.Combine(appRoot, relativeFilePath);
        }
    }
}
