﻿using EasySave.Models;
using EasySave.Services.Interfaces;
using EasySave.ViewModels;
using EasySave.WPF.Utils;
using System.Reflection.Metadata;

namespace EasySave.Commands.BackupJobs
{
    public class ExecuteBackupJobsCommand : AsyncCommandBase
    {
        private readonly BackupJobsListingViewModel _backupJobsViewModel;
        private readonly IBackupJobService _backupJobService;

        public ExecuteBackupJobsCommand(BackupJobsListingViewModel backupJobsViewModel, IBackupJobService backupJobService)
        {
            _backupJobsViewModel = backupJobsViewModel;
            _backupJobService = backupJobService;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            if (BusinessAppChecker.IsBusinessAppRunning(Properties.Settings.Default.BusinessAppName))
                return;

            if (_backupJobsViewModel.BackupJobs.Count == 0)
            {
                return;
            }

            if (_backupJobsViewModel.StartRange == 0 && _backupJobsViewModel.EndRange == 0)
            {
                foreach (var backupJob in _backupJobsViewModel.BackupJobs)
                {
                    ExecuteBackupJob(backupJob);
                }
                return;
            }

            List<int> backupJobsIndex = new List<int>();

            int minIndex = Math.Min(_backupJobsViewModel.StartRange, _backupJobsViewModel.EndRange);
            int maxIndex = Math.Max(_backupJobsViewModel.StartRange, _backupJobsViewModel.EndRange);

            for (int i = minIndex; i <= maxIndex; i++)
            {
                backupJobsIndex.Add(i);
            }

            List<BackupJob> backupJobsToExecute = _backupJobsViewModel.BackupJobs
                .Where((item, index) => backupJobsIndex.Contains(index + 1))
                .ToList();

            foreach (var backupJob in backupJobsToExecute)
            {
                ExecuteBackupJob(backupJob);
            }

        }

        private void ExecuteBackupJob(BackupJob backupJob)
        {
            if (BusinessAppChecker.IsBusinessAppRunning(Properties.Settings.Default.BusinessAppName))
                return;

            Thread thread = new Thread(backupJob.Execute);
            thread.IsBackground = true;
            thread.Start();
        }

    }
}