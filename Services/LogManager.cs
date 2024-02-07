using EasySave.Models;
using EasySave.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Services
{
    public class LogManager
    {
        private readonly IConfiguration _configuration;
        private readonly IFileManager _jsonFileManager;

        private List<Log>? _logs;

        public LogManager(IConfiguration configuration)
        {
            _configuration = configuration;
            _jsonFileManager = new JsonFileManager(GetLogFilePath());
        }

        public async Task ReadLogs()
        {
            _logs = await _jsonFileManager.Read<Log>();
        }

        public async Task WriteLogs()
        {
            await _jsonFileManager.Write(_logs);
        }

        public async Task CreateLog(Log log)
        {
            await ReadLogs();

            _logs.Add(log);

            await WriteLogs();
        }

        private string GetLogFilePath()
        {
            string logDirectory = @".\Data\Logs\";
            string logFileName = $"{DateTime.Now:dd_MM_yyyy}.json";

            string filePath = Path.Combine(logDirectory, logFileName);

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            if (!File.Exists(filePath))
            {
                using (FileStream fs = File.Create(filePath))
                {
                }
            }

            return filePath;
        }
    }
}
