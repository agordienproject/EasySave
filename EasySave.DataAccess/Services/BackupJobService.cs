using EasySave.DataAccess.Services.Factories;
using EasySave.Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasySave.Domain.Services
{
    public class BackupJobService : IBackupJobService
    {
        private readonly IFileService _fileService;

        public BackupJobService(IConfiguration configuration, IFileServiceFactory fileServiceFactory)
        {
            _fileService = fileServiceFactory.CreateFileService("json");
        }

        public async Task<bool> InitDataFile(string filePath)
        {
            try
            {
                string directoryPath = Path.GetDirectoryName(filePath);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                if (!File.Exists(filePath))
                {
                    using (FileStream fs = File.Create(filePath))
                    {
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Gérer l'exception (journalisation, remontée, etc.)
                Console.WriteLine($"Erreur lors de l'initialisation du fichier : {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<BackupJob>> GetAll()
        {
            List<BackupJob> backupJobs = _fileService.Read<BackupJob>();

            return backupJobs;
        }

        public async Task<BackupJob> Get(string name)
        {
            List<BackupJob> backupJobs = _fileService.Read<BackupJob>();

            BackupJob backupJob = backupJobs.FirstOrDefault(backupJob => backupJob.BackupName == name); ;

            return backupJob;
        }

        public async Task<BackupJob> Create(BackupJob entity)
        {
            List<BackupJob> backupJobs = (List<BackupJob>)await GetAll();

            backupJobs.Add(entity);

            _fileService.Write<BackupJob>(backupJobs);
            
            return entity;
        }

        public async Task<bool> Delete(string name)
        {
            List<BackupJob> backupJobs = (List<BackupJob>)await GetAll();

            BackupJob backupToDelete = backupJobs.FirstOrDefault(backupJob => backupJob.BackupName == name);

            if (backupToDelete != null)
            {
                backupJobs.Remove(backupToDelete);

                _fileService.Write(backupJobs);

                return true;
            }

            return false;
        }

        public async Task<BackupJob> Update(string name, BackupJob entity)
        {
            List<BackupJob> backupJobs = (List<BackupJob>)await GetAll();
            BackupJob existingBackup = backupJobs.FirstOrDefault(backupJob => backupJob.BackupName == name);

            if (existingBackup != null)
            {
                int index = backupJobs.IndexOf(existingBackup);
                backupJobs[index] = entity;

                _fileService.Write(backupJobs);

                return entity;
            }

            return null;
        }
    }

}
