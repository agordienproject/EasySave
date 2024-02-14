using EasySave.DataAccess.Services.Factories;
using EasySave.Domain.Models;
using EasySave.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text.Json;

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
                
            }

            return appSettings;
        }

        public async Task SetAppSettings(AppSettings appSettings)
        {
            var options = new JsonSerializerOptions { WriteIndented = true, };
            using FileStream openStream = File.Open(_filePath, FileMode.Truncate);
            await JsonSerializer.SerializeAsync(openStream, appSettings, options);
        }


    }
}
