using ConsoleTables;
using EasySave.Enums;
using EasySave.Models;
using EasySave.Services.Factories;
using EasySave.Services.Interfaces;
using EasySave.Views;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
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

        public BackupManager(IFileServiceFactory fileServiceFactory,ILogManager logManager, IStateManager stateManager)
        {
            _jsonFileManager = fileServiceFactory.CreateFileService("json", AppSettingsJson.GetBackupJobsFilePath());
            _logManager = logManager;
            _stateManager = stateManager;
        }

        public void ReadBackups()
        {
            _backupJobs = _jsonFileManager.Read<BackupJob>();
        }

        public void WriteBackups()
        {
            _jsonFileManager.Write(_backupJobs);
        }

        public void CreateBackupJob(BackupJob backupJob)
        {
            ReadBackups();

            if (_backupJobs.Count >= BACKUPJOBS_LIMIT)
            {
                ConsoleView.TooManyBackup();
                return;
            }

            if (_backupJobs.Any(backup => backup.BackupName == backupJob.BackupName))
            {
                ConsoleView.SameNameBackup();
                return;
            }

            _backupJobs.Add(backupJob);

            WriteBackups();
        }

        public void DeleteBackupJob(string backupJobName)
        {
            ReadBackups();

            BackupJob backupJobToDelete = _backupJobs.Find(backupJob => backupJob.BackupName == backupJobName);

            if (backupJobToDelete == null)
            {
                ConsoleView.NoBackupJobFound();
                return;
            }

            _backupJobs.Remove(backupJobToDelete);
            _stateManager.DeleteState(backupJobName);
            ConsoleView.SuccessDelete(backupJobName);

            WriteBackups();
        }

        public void DisplayBackupJobs()
        {
            ReadBackups();

            ConsoleView.DisplayBackupJobsTable(_backupJobs);
        }

        public void ExecuteBackupJobs(List<int> backupJobsIndex)
        {
            ReadBackups();

            if (_backupJobs.Count == 0)
            {
                ConsoleView.NoBackupJobToExecute();
                return;
            }

            List<BackupJob> backupJobsToExecute = _backupJobs
                .Where((item, index) => backupJobsIndex.Contains(index + 1))
                .ToList();

            foreach (var backupJob in backupJobsToExecute)
            {
                ConsoleView.StartingBackupJobExecutionMessage(backupJob.BackupName);
                ExecuteBackupJob(backupJob);
                ConsoleView.EndBackupJobExecutionMessage();
            }
        }

        public void ExecuteBackupJob(BackupJob backupJob)
        {
            _stateManager.UpdateState(backupJob.BackupName, backupState: BackupState.Active);

            int totalFilesNumber = 0;
            long totalFilesSize = 0;

            GetDirectoryInfos(backupJob.SourceDirectory, ref totalFilesNumber, ref totalFilesSize);

            int nbFilesLeftToDo = totalFilesNumber;
            long filesSizeLeftToDo = totalFilesSize;

            _stateManager.UpdateState(
                backupJob.BackupName,
                totalFilesNumber: totalFilesNumber,
                totalFilesSize: totalFilesSize,
                nbFilesLeftToDo: totalFilesNumber,
                filesSizeLeftToDo: totalFilesSize);

            CopyFiles(backupJob.BackupName, backupJob.SourceDirectory, backupJob.TargetDirectory, backupJob.BackupType, ref nbFilesLeftToDo, ref filesSizeLeftToDo);

            _stateManager.UpdateState(
                backupJob.BackupName,
                backupState: BackupState.Inactive,
                totalFilesNumber: 0,
                totalFilesSize: 0,
                nbFilesLeftToDo: 0,
                filesSizeLeftToDo: 0,
                sourceTransferingFilePath: "",
                targetTransferingFilePath: "");
        }

        public void CopyFiles(string backupJobName, string sourceDir, string targetDir, BackupType backupType, ref int nbFilesLeftToDo, ref long filesSizeLeftToDo)
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
                FileInfo fileInfo = new FileInfo(filePath);

                string targetPath = Path.Combine(targetDir, fileInfo.Name);

                _stateManager.UpdateState(
                    backupJobName,
                    nbFilesLeftToDo: nbFilesLeftToDo,
                    filesSizeLeftToDo: filesSizeLeftToDo,
                    sourceTransferingFilePath: filePath,
                    targetTransferingFilePath: targetPath);

                CopyFile(backupJobName, filePath, targetDir, backupType);

                nbFilesLeftToDo--;
                filesSizeLeftToDo -= fileInfo.Length;
            }

            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                string subDirName = Path.GetFileName(subDir);
                string targetSubDir = Path.Combine(targetDir, subDirName);
                CopyFiles(backupJobName, subDir, targetSubDir, backupType, ref nbFilesLeftToDo, ref filesSizeLeftToDo);
            }
        }

        static void GetDirectoryInfos(string directory, ref int totalFilesNumber, ref long totalFilesSize)
        {
            try
            {
                string[] files = Directory.GetFiles(directory);
                string[] subDirs = Directory.GetDirectories(directory);

                foreach (var fichier in files)
                {
                    totalFilesNumber++;
                    totalFilesSize += new FileInfo(fichier).Length;
                }

                foreach (var subDir in subDirs)
                {
                    GetDirectoryInfos(subDir, ref totalFilesNumber, ref totalFilesSize);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($" {directory}: {e.Message}");
            }
        }

        private void CopyFile(string backupJobName, string sourceFilePath, string targetDir, BackupType backupType)
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

                _logManager.CreateLog(new Log(
                    backupJobName,
                    sourceFilePath,
                    targetFilePath,
                    sourceFileInfo.Length,
                    transferTime,
                    DateTime.Now.ToString()
                ));

                ConsoleView.CopyFile(sourceFilePath);
            }
        }

        private static bool ShouldCopyFile(string sourceFilePath, string targetFilePath)
        {
            FileInfo destFileInfo = new(targetFilePath);
            string sourceFileHash = CalculateMD5(sourceFilePath);

            // Check if the file already exists in the target directory
            if (File.Exists(targetFilePath))
            {
                string destFileHash = CalculateMD5(targetFilePath);

                if (sourceFileHash != destFileHash)
                {
                    // The source file is more recent, so we copy it
                    return true;
                }
                else
                {
                    // The source file is no newer than the one already there, so we move on to the next file
                    ConsoleView.DontCopyFile(destFileInfo);
                    return false;
                }
            }
            else
            {
                // The file doesn't exist in the target directory, so we copy it
                ConsoleView.CopyNewFile();
                return true;
            }

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
