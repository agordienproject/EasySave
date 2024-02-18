using EasySave.Models;
using System.Text.Json;
using System.IO;
using EasySave.Services.Interfaces;
using System.Collections.Specialized;

namespace EasySave.Services
{
    public class AppSettingsService
    {
        public static async Task<AppSettings?> LoadAppSettings()
        {
            return new AppSettings
            (
                Properties.Settings.Default.CurrentCulture,
                Properties.Settings.Default.StateFolderPath,
                Properties.Settings.Default.StateFileName,
                Properties.Settings.Default.LogsFolderPath,
                Properties.Settings.Default.LogsFileType,
                Properties.Settings.Default.AuthorizedExtensions.Cast<string>().ToList()
            );
        }

        public static async Task SaveAppSettings(AppSettings appSettings)
        {
            Properties.Settings.Default.CurrentCulture = appSettings.CurrentCulture;
            Properties.Settings.Default.StateFolderPath = appSettings.StateFolderPath;
            Properties.Settings.Default.StateFileName = appSettings.StateFileName;
            Properties.Settings.Default.LogsFolderPath = appSettings.LogsFolderPath;
            Properties.Settings.Default.LogsFileType = appSettings.LogsFileType;
            StringCollection strings = new StringCollection();
            strings.AddRange([.. appSettings.AuthorizedExtensions]);
            Properties.Settings.Default.AuthorizedExtensions = strings;

            Properties.Settings.Default.Save();
        }
    }
}
