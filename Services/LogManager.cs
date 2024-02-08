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
    public class LogManager : ILogManager
    {
        private readonly IFileManager _jsonFileManager;

        private List<Log>? _logs;

        public LogManager()
        {
            _jsonFileManager = new JsonFileManager(AppSettingsJson.GetLogsFilePath());
        }

        public void ReadLogs()
        {
            _logs = _jsonFileManager.Read<Log>();
        }

        public void WriteLogs()
        {
            _jsonFileManager.Write(_logs);
        }

        public void CreateLog(Log log)
        {
            ReadLogs();

            _logs.Add(log);

            WriteLogs();
        }

    }
}
