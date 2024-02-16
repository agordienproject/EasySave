using EasySave.DataAccess.Services;
using EasySave.DataAccess.Services.Factories;
using EasySave.Domain.Enums;
using EasySave.Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasySave.Domain.Services
{
    public class BackupJobService : DataService<BackupJob>, IBackupJobService
    {
        private readonly IStateService _stateService;
        private readonly ILogService _logService;
        private readonly IConfiguration _configuration;

        public BackupJobService(IConfiguration configuration, IFileServiceFactory fileServiceFactory, IStateService stateService, ILogService logService)
        {
            string type = configuration["DataFilesTypes:BackupJobsFileType"];
            string filePath = Path.Combine(configuration["DataFilesLocation:BackupJobsFolderPath"], configuration["DataFilesLocation:BackupJobsJsonFileName"]);
            base._fileService = fileServiceFactory.CreateFileService(type, filePath);

            _stateService = stateService;
            _logService = logService;
            _configuration = configuration;
        } 

        public async Task ExecuteBackupJobs(List<int> backupJobsIndex)
        {
            List<BackupJob> backupJobs = (List<BackupJob>) await GetAll();
            
            if (backupJobs.Count == 0)
                return;

            List<BackupJob> backupJobsToExecute = backupJobs
                .Where((item, index) => backupJobsIndex.Contains(index + 1))
                .ToList();

            foreach (var backupJob in backupJobsToExecute)
            {
                BackupJobExecution backupjobExecution = new BackupJobExecution(backupJob, _stateService, _logService, _configuration);

                await backupjobExecution.Execute();
            }   
        }

        public async Task ExecuteBackupJob(BackupJob backupJob)
        {
            BackupJobExecution backupjobExecution = new BackupJobExecution(backupJob, _stateService, _logService, _configuration);

            await backupjobExecution.Execute();
        }
    }

}
