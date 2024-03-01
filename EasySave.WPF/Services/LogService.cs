using EasySave.Models;
using EasySave.Services.Factories;
using EasySave.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace EasySave.Services
{
    public class LogService : ILogService
    {
        private IFileService _fileService { get; set; }

        private static object _lock = new object();

        public LogService(IFileServiceFactory fileServiceFactory) 
        {
            string type = Properties.Settings.Default.LogsFileType;
            string filePath = Path.Combine(Properties.Settings.Default.LogsFolderPath, $"{DateTime.Now:dd_MM_yyyy}.{type}");
            _fileService = fileServiceFactory.CreateFileService(type, filePath);
        }

        public List<Log> GetAll()
        {
            List<Log> list;

            lock (_lock)
            {
                list = _fileService.Read<Log>();
            }

            if (list == null)
                return new List<Log>();

            return list;
        }

        public Log Create(Log entity)
        {
            List<Log> list = GetAll();

            list.Add(entity);

            lock (_lock)
            {
                _fileService.Write<Log>(list);
            }

            return entity;
        }
    }
}
