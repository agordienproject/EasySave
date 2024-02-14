using EasySave.Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Domain.Services
{
    public interface IAppSettingsService
    {
        Task<AppSettings?> GetAppSettings();
        Task SetAppSettings(AppSettings appSettings);
    }
}
