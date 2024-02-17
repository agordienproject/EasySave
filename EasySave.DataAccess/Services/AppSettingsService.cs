using EasySave.DataAccess.Services.Factories;
using EasySave.Domain.Models;
using EasySave.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks; // Importez ce namespace pour pouvoir utiliser Task

namespace EasySave.DataAccess.Services
{
    public class AppSettingsService : IAppSettingsService
    {
        public readonly string _filePath;
        
        public AppSettingsService()
        {
            _filePath = "appsettings.json";
        }

        public async Task<AppSettings?> GetAppSettings()
        {
            using FileStream openStream = File.OpenRead(_filePath);

            AppSettings? appSettings = null;
            try
            {
                appSettings = await JsonSerializer.DeserializeAsync<AppSettings>(openStream);
            }
            catch (Exception ex)
            {
                // Gérer les exceptions ici
            }
            return appSettings;
        }

        // Implémentation de la méthode SetAppSettings
        public async Task SetAppSettings(AppSettings appSettings)
        {
            var options = new JsonSerializerOptions { WriteIndented = true, };
            using FileStream openStream = File.Open(_filePath, FileMode.Truncate);
            await JsonSerializer.SerializeAsync(openStream, appSettings, options);
        }

        public static async Task<AppSettings?> AppSettings()
        {
            using FileStream openStream = File.OpenRead("appsettings.json");

            AppSettings? appSettings = null;
            try
            {
                appSettings = await JsonSerializer.DeserializeAsync<AppSettings>(openStream);
            }
            catch (Exception ex)
            {
                // Gérer les exceptions ici
            }
            return appSettings;
        }
    }
}
