using EasySave.Models;
using EasySave.Services.Factories;
using EasySave.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EasySave.Services
{
    public class LogManager : ILogManager
    {
        private readonly IFileManager _fileService;

        private List<Log>? _logs;

        public LogManager(IConfiguration configuration, IFileServiceFactory fileServiceFactory)
        {
            _fileService = fileServiceFactory.CreateFileService(configuration["LogFileType"],AppSettingsJson.GetLogsFilePath());
        }

        public void ReadLogs()
        {
            _logs = _fileService.Read<Log>();
        }

        public void WriteLogs()
        {
            _fileService.Write(_logs);
        }

        public void CreateLog(Log log)
        {
            ReadLogs();

            _logs.Add(log);

            WriteLogs();
        }

    }
}
