using EasySave.DataAccess.Services.Factories;
using EasySave.Domain.Models;
using EasySave.Domain.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.DataAccess.Services
{
    public class LogService : DataService<Log>, ILogService
    {
        public LogService(IConfiguration configuration, IFileServiceFactory fileServiceFactory) 
        {
            string type = configuration["DataFilesTypes:LogsFileType"];
            string filePath = Path.Combine(configuration["DataFilesLocation:LogsFolderPath"], $"{DateTime.Now:dd_MM_yyyy}.{type}");
            _fileService = fileServiceFactory.CreateFileService(type, filePath);
        }
    }
}
