using EasySave.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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

        public static string GetApplicationExeDirectory()
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
        public static async Task SetCurrentCulture(string cultureName)
        {
            string appRoot = GetApplicationExeDirectory();
            string fileName = "appsettings.json";
            string relativeFilePath = Path.Combine(appRoot, fileName);

            // Lecture du fichier JSON
            using (FileStream fileStream = File.Open(relativeFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                AppSettings appSettings = JsonSerializer.Deserialize<AppSettings>(fileStream);
                Console.WriteLine(appSettings.CurrentCulture);

                // Vérification si la langue choisie est déjà actuelle
                if (appSettings.CurrentCulture != cultureName)
                {
                    // Mise à jour de la langue choisie dans le JSON
                    appSettings.CurrentCulture = cultureName;
                    Console.WriteLine(appSettings.CurrentCulture);

                    // Réinitialiser la position de lecture du flux à zéro
                    fileStream.Seek(0, SeekOrigin.Begin);

                    // Écrire le JSON modifié dans le flux
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    JsonSerializer.Serialize(fileStream, appSettings, options);
                    fileStream.SetLength(fileStream.Position);

                    Console.WriteLine($"{Resources.Language.UpdateLanguage} : {cultureName}");
                }
                else
                {
                    Console.WriteLine($"{Resources.Language.ErrorSameLanguage}");
                }
            } // Le bloc using se termine ici, ce qui garantit que le flux de fichier est fermé une fois sorti du bloc.
        }


    }
}
