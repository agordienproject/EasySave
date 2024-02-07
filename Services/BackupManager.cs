﻿using ConsoleTables;
using EasySave.Enums;
using EasySave.Models;
using EasySave.Services.Interfaces;
using EasySave.Views;
using Microsoft.Extensions.Configuration;
using System.Xml.Linq;

namespace EasySave.Services
{
    public class BackupManager
    {
        private readonly IConfiguration _configuration;
        private readonly IFileManager _jsonFileManager;

        private readonly LogManager _logManager;
        private readonly StateManager _stateManager;

        private List<BackupJob>? _backupJobs;

        private const int BACKUPJOBS_LIMIT = 5;

        public BackupManager(IConfiguration configuration) 
        {
            _configuration = configuration;
            _jsonFileManager = new JsonFileManager(AppSettingsJson.GetBackupJobsFilePath());
            _logManager = new LogManager(configuration);
            _stateManager = new StateManager(configuration);
        }

        public async Task ReadBackups()
        {
            _backupJobs = await _jsonFileManager.Read<BackupJob>();
        }

        public async Task WriteBackups()
        {
            await _jsonFileManager.Write(_backupJobs);
        }

        public async Task CreateBackupJob(BackupJob backupJob)
        {
            await ReadBackups();

            if (_backupJobs.Count >= BACKUPJOBS_LIMIT)
            {
                Console.WriteLine("Too many backups already exist.");
                return;
            }
            
            if (_backupJobs.Any(backup => backup.BackupName == backupJob.BackupName))
            {
                Console.WriteLine("Two backups can't have the same name.");
                return;
            }
            
            _backupJobs.Add(backupJob);
            await _stateManager.CreateState(new State(backupJob.BackupName));

            await WriteBackups();
        }

        public async Task DeleteBackupJob(string backupJobName)
        {
            await ReadBackups();

            BackupJob backupJobToDelete = _backupJobs.Find(backupJob => backupJob.BackupName == backupJobName);

            if (backupJobToDelete == null) 
            {
                Console.WriteLine($"No backupJob named : {backupJobName} was found !");
                return;
            }
            
            _backupJobs.Remove(backupJobToDelete);
            await _stateManager.DeleteState(backupJobName);
            Console.WriteLine($"{backupJobName} successfuly deleted");

            await WriteBackups();
        }

        public async Task DisplayBackupJobs()
        {
            await ReadBackups();

            ConsoleView.DisplayBackupJobsTable(_backupJobs);
        }

        public async Task ExecuteBackupJobs(List<int> backupJobsIndex)
        {
            await ReadBackups();

            if (_backupJobs.Count == 0)
            {
                Console.WriteLine("No backup job to execute !");
                return;
            }

            List<BackupJob> backupJobsToExecute = _backupJobs.Where((item, index) => backupJobsIndex.Contains(index + 1)).ToList();
            foreach (var backupJob in backupJobsToExecute)
            {
                await ExecuteBackupJob(backupJob);
            }

        }

        public async Task ExecuteBackupJob(BackupJob backupJob)
        {
            if (backupJob.BackupType == BackupType.Complete)
            {
                Console.WriteLine($"La sauvegarde est complète");
                //Console.WriteLine($"Complete backup of : {backupJob.BackupName}");
                await CopyFilesRecursively(backupJob.BackupName, backupJob.SourceDirectory, backupJob.TargetDirectory);
            }
            else if (backupJob.BackupType == BackupType.Differential)
            {
                Console.WriteLine($"La sauvegarde est différentielle");
                //Console.WriteLine($"Differential backup of : {backupJob.BackupName}");
                await CopyFilesDifferential(backupJob.BackupName, backupJob.SourceDirectory, backupJob.TargetDirectory);
            }
        }

        public async Task CopyFilesRecursively(string backupJobName, string sourceDir, string targetDir)
        {

            // Vérifier si le répertoire source existe
            if (!Directory.Exists(sourceDir))
            {
                ConsoleView.NoSourceDirMessage(sourceDir);
                return;
            }

            // Créer le répertoire cible s'il n'existe pas
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            // Copier les fichiers du répertoire source vers le répertoire cible
            foreach (string filePath in Directory.GetFiles(sourceDir))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                string fileName = Path.GetFileName(filePath);
                string destFilePath = Path.Combine(targetDir, fileName);

                CopyFileAndUpdateLog(backupJobName, filePath, destFilePath, fileInfo);

                Console.WriteLine($"Copie du fichier : {fileName} dans {destFilePath}");
            }

            // Copier les fichiers des sous-répertoires
            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                string subDirName = Path.GetFileName(subDir);
                string targetSubDir = Path.Combine(targetDir, subDirName);
                await CopyFilesRecursively(backupJobName, subDir, targetSubDir);
            }
        }
        public async Task CopyFilesDifferential(string backupJobName, string sourceDir, string targetDir)
        {
            Console.WriteLine($"Entrée dans la fonction CopyFilesDifferential");
            // Vérifier si le répertoire source existe
            if (!Directory.Exists(sourceDir))
            {
                ConsoleView.NoSourceDirMessage(sourceDir);
                return;
            }

            // Créer le répertoire cible s'il n'existe pas
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            // Copier les fichiers du répertoire source vers le répertoire cible
            foreach (string filePath in Directory.GetFiles(sourceDir))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                string fileName = Path.GetFileName(filePath);
                string destFilePath = Path.Combine(targetDir, fileName);

                // Vérifier si le fichier existe déjà dans le répertoire cible
                if (File.Exists(destFilePath))
                {
                    FileInfo destFileInfo = new FileInfo(destFilePath);
                    if (fileInfo.LastWriteTime > destFileInfo.LastWriteTime)
                    {
                        // Le fichier source est plus récent, donc le copier
                        CopyFileAndUpdateLog(backupJobName, filePath, destFilePath, fileInfo);
                    }
                    else
                    {
                        // Le fichier source n'est pas plus récent, passer au prochain fichier
                        Console.WriteLine($"Le fichier {fileName} existe déjà dans le répertoire cible et est plus récent ou égal. Ne pas copier.");
                    }
                }
                else
                {
                    Console.WriteLine($"Copie du nouveau fichier");

                    // Le fichier n'existe pas dans le répertoire cible, donc le copier
                    CopyFileAndUpdateLog(backupJobName, filePath, destFilePath, fileInfo);
                }
            }

            // Copier les fichiers des sous-répertoires
            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                string subDirName = Path.GetFileName(subDir);
                string targetSubDir = Path.Combine(targetDir, subDirName);
                await CopyFilesDifferential(backupJobName, subDir, targetSubDir);
            }
        }
        private void CopyFileAndUpdateLog(string backupJobName, string sourceFilePath, string destFilePath, FileInfo fileInfo)
        {
            Console.WriteLine($"Entrée dans la fonction CopyFileAndUpdateLog");

            DateTime before = DateTime.Now;
            File.Copy(sourceFilePath, destFilePath, true);
            DateTime after = DateTime.Now;
            double transferTime = after.Subtract(before).TotalSeconds;
            _logManager.CreateLog(new Log(
                backupJobName,
                sourceFilePath,
                destFilePath,
                fileInfo.Length,
                transferTime,
                DateTime.Now
            ));
            Console.WriteLine($"Copie du fichier : {Path.GetFileName(sourceFilePath)}");
        }

    }
}