using ConsoleTables;
using EasySave.Enums;
using EasySave.Models;
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
            await _stateManager.UpdateState(backupJob.BackupName, BackupState : BackupState.Active);

            await CopyFiles(backupJob.BackupName, backupJob.SourceDirectory, backupJob.TargetDirectory, backupJob.BackupType);

            await _stateManager.UpdateState(
                backupJob.BackupName, 
                BackupState: BackupState.Inactive, 
                TotalFilesNumber : 0,
                TotalFilesSize : 0,
                NbFilesLeftToDo: 0,
                FilesSizeLeftToDo: 0,
                SourceTransferingFilePath : "",
                TargetTransferingFilePath: "");
        }

        public async Task CopyFiles(string backupJobName, string sourceDir, string targetDir, BackupType backupType)
        {
            if (!Directory.Exists(sourceDir))
            {
                ConsoleView.NoSourceDirMessage(sourceDir);
                return;
            }

            int totalFilesNumber = 0;
            long totalFilesSize = 0;

            int nbFilesLeftToDo = totalFilesNumber;
            long filesSizeLeftToDo = totalFilesSize;

            GetDirectoryInfos(sourceDir, ref totalFilesNumber, ref totalFilesSize);

            await _stateManager.UpdateState(
                backupJobName, 
                TotalFilesNumber: totalFilesNumber, 
                TotalFilesSize: totalFilesSize, 
                NbFilesLeftToDo : nbFilesLeftToDo, 
                FilesSizeLeftToDo : filesSizeLeftToDo);

            
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            foreach (string filePath in Directory.GetFiles(sourceDir))
            {
                FileInfo fileInfo = new FileInfo(filePath);

                string targetPath = Path.Combine(targetDir, fileInfo.Name);

                await _stateManager.UpdateState(backupJobName, NbFilesLeftToDo: nbFilesLeftToDo, FilesSizeLeftToDo: filesSizeLeftToDo, SourceTransferingFilePath: filePath, TargetTransferingFilePath: targetPath);

                await CopyFile(backupJobName, filePath, targetDir, backupType);

                nbFilesLeftToDo--;

                filesSizeLeftToDo -= fileInfo.Length;

            }

            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                string subDirName = Path.GetFileName(subDir);
                string targetSubDir = Path.Combine(targetDir, subDirName);
                await CopyFiles(backupJobName, subDir, targetSubDir, backupType);
            }
        }

        static void GetDirectoryInfos(string directory, ref int totalFilesNumber, ref long totalFilesSize)
        {
            try
            {
                string[] fichiers = Directory.GetFiles(directory);
                string[] sousDossiers = Directory.GetDirectories(directory);

                foreach (var fichier in fichiers)
                {
                    totalFilesNumber++;
                    totalFilesSize += new FileInfo(fichier).Length;
                }

                foreach (var sousDossier in sousDossiers)
                {
                    GetDirectoryInfos(sousDossier, ref totalFilesNumber, ref totalFilesSize);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($" {directory}: {e.Message}");
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
                    DateTime.Now.ToString()
                ));

                Console.WriteLine($"Copie du fichier : {Path.GetFileName(sourceFilePath)}");
            }
        }

        private static bool ShouldCopyFile(string sourceFilePath, string targetFilePath)
        {
            FileInfo sourceFileInfo = new(sourceFilePath);
            FileInfo destFileInfo = new(targetFilePath);
            string sourceFileHash = CalculateMD5(sourceFilePath);

            // Vérifier si le fichier existe déjà dans le répertoire cible
            if (File.Exists(targetFilePath))
            {
                string destFileHash = CalculateMD5(targetFilePath);

                if (sourceFileHash != destFileHash)
                {
                    // Le fichier source est plus récent, donc le copier
                    return true;
                }
                else
                {
                    // Le fichier source n'est pas plus récent, passer au prochain fichier
                    Console.WriteLine($"Le fichier {destFileInfo} existe déjà dans le répertoire cible et est plus récent ou égal. Ne pas copier.");
                    return false;
                }
            }
            else
            {

                Console.WriteLine($"Copie du nouveau fichier");
                return true;
                // Le fichier n'existe pas dans le répertoire cible, donc le copier
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
