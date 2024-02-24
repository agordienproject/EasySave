using EasySave.Models;
using System.Text.Json;
using System.IO;
using EasySave.Services.Interfaces;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;

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
                Properties.Settings.Default.EncryptedExtensions.Cast<string>().ToList(),
                Properties.Settings.Default.PrioritizedExtensions.Cast<string>().ToList(),
                Properties.Settings.Default.BusinessAppName,
                Properties.Settings.Default.MaxKoToTransfert
            );
        }

        public static async Task SaveAppSettings(AppSettings appSettings)
        {
            Properties.Settings.Default.CurrentCulture = appSettings.CurrentCulture;
            Properties.Settings.Default.StateFolderPath = appSettings.StateFolderPath;
            Properties.Settings.Default.StateFileName = appSettings.StateFileName;
            Properties.Settings.Default.LogsFolderPath = appSettings.LogsFolderPath;
            Properties.Settings.Default.LogsFileType = appSettings.LogsFileType;
            
            StringCollection strings_encrypted = new StringCollection();
            strings_encrypted.AddRange([.. appSettings.EncryptedExtensions]);
            Properties.Settings.Default.EncryptedExtensions = strings_encrypted;

            StringCollection strings_prioritized = new StringCollection();
            strings_prioritized.AddRange([.. appSettings.PrioritizedExtensions]);
            Properties.Settings.Default.PrioritizedExtensions = strings_prioritized;

            Properties.Settings.Default.BusinessAppName = appSettings.BusinessAppName;

            Properties.Settings.Default.MaxKoToTransfert = appSettings.MaxKoToTransfert;

            Properties.Settings.Default.IsRestarting = true;

            Properties.Settings.Default.Save();

            Process.Start(Process.GetCurrentProcess().MainModule.FileName);
            Application.Current.Shutdown();
        }
    }
}
