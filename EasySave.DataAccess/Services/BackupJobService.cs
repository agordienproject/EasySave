﻿using EasySave.DataAccess.Services;
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
        private readonly ILogService _logService;

        public BackupJobService(IConfiguration configuration, IFileServiceFactory fileServiceFactory, ILogService logService)
        {
            string type = configuration["DataFilesTypes:BackupJobsFileType"];
            string filePath = Path.Combine(configuration["DataFilesLocation:BackupJobsFolderPath"], configuration["DataFilesLocation:BackupJobsJsonFileName"]);
            base._fileService = fileServiceFactory.CreateFileService(type, filePath);

            _logService = logService;
        } 

    }

}
