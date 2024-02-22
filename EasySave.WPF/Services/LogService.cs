using EasySave.Models;
using EasySave.Services.Factories;
using EasySave.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace EasySave.Services
{
    public class LogService : DataService<Log>, ILogService
    {
        public LogService(IFileServiceFactory fileServiceFactory) 
        {
            string type = Properties.Settings.Default.LogsFileType;
            string filePath = Path.Combine(Properties.Settings.Default.LogsFolderPath, $"{DateTime.Now:dd_MM_yyyy}.{type}");
            _fileService = fileServiceFactory.CreateFileService(type, filePath);
        }
    }
}
