using EasySave.Enums;
using EasySave.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EasySave.Services
{
    public class BackupManager
    {
        private readonly FileManager _fileManager;
        private List<BackupJob> _backupJobs {  get; set; }

        public BackupManager() 
        {
            _fileManager = new FileManager(@".\Backups\backups.json");
        }

        public async Task<List<BackupJob>> GetBackupJobs()
        {
            await ReadBackups();
            return _backupJobs;
        }

        public async Task CreateBackupJob(BackupJob backupJob)
        {
            await ReadBackups();

            if (_backupJobs.Count < 5)
            {
                if (!_backupJobs.Any(backup => backup.BackupName == backupJob.BackupName))
                {
                    _backupJobs.Add(backupJob);
                }
                else
                {
                    Console.WriteLine("Two backups can't have the same name.");
                }
            }
            else
            {
                Console.WriteLine("Too many backups already exist.");
            }

            await WriteBackups();
        }

        public async Task DeleteBackupJob(BackupJob backupJobToDelete)
        {
            await ReadBackups();

            if (backupJobToDelete != null)
            {
                bool deleteStatus = _backupJobs.Remove(backupJobToDelete);
                Console.WriteLine($"{backupJobToDelete.BackupName} successfuly deleted");
            }

            await WriteBackups();
        }

        public async Task ExecuteBackupJob(BackupJob backupJob)
        {
            if (backupJob.BackupType == BackupType.Complete)
            {
                Console.WriteLine($"Complete backup of : {backupJob.BackupName}");
                await FileManager.CopyFilesRecursively(backupJob.SourceDirectory, backupJob.TargetDirectory);
            }
            else if (backupJob.BackupType == BackupType.Differential)
            {
                Console.WriteLine($"Diferential backup of : {backupJob.BackupName}");
                await FileManager.CopyFilesRecursively(backupJob.SourceDirectory, backupJob.TargetDirectory);
            }
        }

        public async Task ExecuteBackupJobs(List<BackupJob> backupJobsToExecute)
        {
            foreach (var backupJobToExecute in backupJobsToExecute)
            {
                await ExecuteBackupJob(backupJobToExecute);
            }
        }

        public async Task DisplayBackupJobs()
        {
            await ReadBackups();
            int i = 0;
            foreach (var backupJob in _backupJobs)
            {
                Console.WriteLine($"---------------{i}-----------------");
                Console.WriteLine($"Name : {backupJob.BackupName}");
                Console.WriteLine($"Source directory : {backupJob.SourceDirectory}");
                Console.WriteLine($"Destination directory : {backupJob.TargetDirectory}");
                Console.WriteLine($"Backup type : {backupJob.BackupType}");
                Console.WriteLine("----------------------------------");
                i++;
            }
        }

        public async Task ReadBackups()
        {
            _backupJobs = await _fileManager.Read<BackupJob>();
        }

        public async Task WriteBackups()
        {
            await _fileManager.Write(_backupJobs);
        }

    }
}
