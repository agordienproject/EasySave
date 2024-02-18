using EasySave.Models;
using EasySave.Services.Factories;
using EasySave.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EasySave.Services
{
    public class BackupJobService : DataService<BackupJobInfo>, IBackupJobService
    {
        public BackupJobService(IFileServiceFactory fileServiceFactory) 
        {
            string filePath = Path.Combine(Properties.Settings.Default.StateFolderPath, Properties.Settings.Default.StateFileName);
            _fileService = fileServiceFactory.CreateFileService("json", filePath);
        }

    }



}
