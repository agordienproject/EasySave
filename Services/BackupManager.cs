﻿using ConsoleTables;
using EasySave.Enums;
using EasySave.Models;
using EasySave.Services.Interfaces;
using EasySave.Views;
using Microsoft.Extensions.Configuration;
using System.Xml.Linq;

namespace EasySave.Services
{
    public class BackupManager : IBackupManager
    {
        private readonly IFileManager _jsonFileManager;

        private readonly ILogManager _logManager;
        private readonly IStateManager _stateManager;

        private List<BackupJob>? _backupJobs;

        private const int BACKUPJOBS_LIMIT = 5;

        public BackupManager(ILogManager logManager, IStateManager stateManager)
        {
            _jsonFileManager = new JsonFileManager(AppSettingsJson.GetBackupJobsFilePath());
            _logManager = logManager;
            _stateManager = stateManager;
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

            List<BackupJob> backupJobsToExecute = _backupJobs
                .Where((item, index) => backupJobsIndex.Contains(index + 1))
                .ToList();

            foreach (var backupJob in backupJobsToExecute)
            {
                await ExecuteBackupJob(backupJob);
            }
        }

        public async Task ExecuteBackupJob(BackupJob backupJob)
        {
            await CopyFiles(backupJob.BackupName, backupJob.SourceDirectory, backupJob.TargetDirectory, backupJob.BackupType);
        }

        public async Task CopyFiles(string backupJobName, string sourceDir, string targetDir, BackupType backupType)
        {
            if (!Directory.Exists(sourceDir))
            {
                ConsoleView.NoSourceDirMessage(sourceDir);
                return;
            }

            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            foreach (string filePath in Directory.GetFiles(sourceDir))
            {
                await CopyFile(backupJobName, filePath, targetDir, backupType);
            }

            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                string subDirName = Path.GetFileName(subDir);
                string targetSubDir = Path.Combine(targetDir, subDirName);
                await CopyFiles(backupJobName, subDir, targetSubDir, backupType);
            }
        }

        private async Task CopyFile(string backupJobName, string sourceFilePath, string targetDir, BackupType backupType)
        {
            string fileName = Path.GetFileName(sourceFilePath);
            string targetFilePath = Path.Combine(targetDir, fileName);

            bool shouldCopy = true;

            if (backupType == BackupType.Differential)
            {
                shouldCopy = ShouldCopyFile(sourceFilePath, targetFilePath);
            }

            bool targetFileExist = File.Exists(targetFilePath);

            if ((targetFileExist && shouldCopy) || !targetFileExist)
            {
                FileInfo sourceFileInfo = new(sourceFilePath);

                double transferTime;

                try
                {
                    DateTime before = DateTime.Now;
                    File.Copy(sourceFilePath, targetFilePath, true);
                    DateTime after = DateTime.Now;
                    transferTime = after.Subtract(before).TotalSeconds;
                }
                catch (Exception)
                {
                    transferTime = -1;
                }

                await _logManager.CreateLog(new Log(
                    backupJobName,
                    sourceFilePath,
                    targetFilePath,
                    sourceFileInfo.Length,
                    transferTime,
                    DateTime.Now
                ));

                Console.WriteLine($"Copie du fichier : {Path.GetFileName(sourceFilePath)}");
            }
        }

        private static bool ShouldCopyFile(string sourceFilePath, string targetFilePath)
        {
            FileInfo sourceFileInfo = new(sourceFilePath);
            FileInfo destFileInfo = new(targetFilePath);
            return sourceFileInfo.LastWriteTime > destFileInfo.LastWriteTime;
        }
    }
}
