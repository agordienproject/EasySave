using EasySave.Models;

namespace EasySave.Services.Interfaces
{
    public interface IAppSettingsService
    {
        //Task<AppSettings?> GetAppSettings();
        Task SetAppSettings(AppSettings? appSettings);
    }
}
