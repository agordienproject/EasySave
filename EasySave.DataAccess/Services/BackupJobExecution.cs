using EasySave.Domain.Enums;
using EasySave.Domain.Models;
using EasySave.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.DataAccess.Services
{
    public class BackupJobExecution
    {
        private readonly IStateService _stateService;
        private readonly ILogService _logService;

        private BackupJob _backupJob {  get; set; }

        private State _backupState;
        public State BackupState
        {
            get
            {
                return _backupState;
            }
            set
            {
                _backupState = value;
                UpdateState(value);
            }
        }
        
        public BackupJobExecution(BackupJob backupJob,IStateService stateService, ILogService logService) 
        {
            _stateService = stateService;
            _logService = logService;

            _backupJob = backupJob;
            BackupState = new State(backupJob.BackupName);
        }

        public async Task UpdateState(State state)
        {
            await _stateService.Update(state);
        }

        public async Task Execute()
        {
            await InitState();
            
            await GetDirectoryInfos(_backupJob.SourceDirectory);

            BackupState.NbFilesLeftToDo = BackupState.TotalFilesNumber;
            BackupState.FilesSizeLeftToDo = BackupState.TotalFilesSize;

            await CopyFiles(_backupJob.BackupName, _backupJob.SourceDirectory, _backupJob.TargetDirectory, _backupJob.BackupType);

            await ClearState();
        }

        private async Task InitState()
        {
            BackupState.BackupState = Domain.Enums.BackupState.Active;
        }

        private async Task ClearState()
        {
            BackupState.BackupState = Domain.Enums.BackupState.Inactive;
            BackupState.BackupTime = DateTime.Now.ToString();
            BackupState.TotalFilesNumber = 0;
            BackupState.TotalFilesSize = (long)0;
            BackupState.NbFilesLeftToDo = 0;
            BackupState.FilesSizeLeftToDo = (long)0;
            BackupState.SourceTransferingFilePath = "";
            BackupState.TargetTransferingFilePath = "";
        }

        private async Task GetDirectoryInfos(string directory)
        {
            try
            {
                string[] files = Directory.GetFiles(directory);
                string[] subDirs = Directory.GetDirectories(directory);

                foreach (var fichier in files)
                {
                    BackupState.TotalFilesNumber++;
                    BackupState.TotalFilesSize += new FileInfo(fichier).Length;
                }

                foreach (var subDir in subDirs)
                {
                    await GetDirectoryInfos(subDir);
                }
            }
            catch (Exception e)
            {
                
            }
        }
        
        private async Task CopyFiles(string backupJobName, string sourceDir, string targetDir, BackupType backupType)
        {
            if (!Directory.Exists(sourceDir))
            {
                return;
            }

            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            foreach (string filePath in Directory.GetFiles(sourceDir))
            {
                FileInfo fileInfo = new FileInfo(filePath);

                string targetPath = Path.Combine(targetDir, fileInfo.Name);

                BackupState.SourceTransferingFilePath = filePath;
                BackupState.TargetTransferingFilePath = targetPath;

                await CopyFile(backupJobName, filePath, targetDir, backupType);

                BackupState.NbFilesLeftToDo--;
                BackupState.FilesSizeLeftToDo -= fileInfo.Length;
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
            string sourceFileName = Path.GetFileName(sourceFilePath);
            string targetFilePath = Path.Combine(targetDir, sourceFileName);

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

                await _logService.Create(new Log(
                    backupJobName,
                    sourceFilePath,
                    targetFilePath,
                    sourceFileInfo.Length,
                    transferTime,
                    DateTime.Now.ToString()
                ));

            }
        }

        private static bool ShouldCopyFile(string sourceFilePath, string targetFilePath)
        {
            bool shouldCopy = false;

            // Check if the file already exists in the target directory
            if (!File.Exists(targetFilePath))
            {
                shouldCopy = true;
                return shouldCopy;
            }

            string sourceFileHash = CalculateMD5(sourceFilePath);
            string destFileHash = CalculateMD5(targetFilePath);

            if (sourceFileHash != destFileHash)
            {
                // The source file is more recent, so we copy it
                shouldCopy = true;
                return shouldCopy;
            }

            return shouldCopy;
        }

        private static string CalculateMD5(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    byte[] hashBytes = md5.ComputeHash(stream);
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}
