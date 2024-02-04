using EasySave.Models;
using EasySave.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Services
{
    public class LogManager
    {
        private readonly IConfiguration _configuration;
        private readonly IJsonFileManager _jsonFileManager;

        private List<Log>? _logs;

        public LogManager(IConfiguration configuration)
        {
            _configuration = configuration;
            _jsonFileManager = new JsonFileManager(Path.Combine(_configuration.GetValue<string>("LogsFolderPath"), DateTime.Now.ToString() + ".json"));
        }

        public List<Log> GetLogs()
        {
            ReadLogs();
            return _logs;
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
