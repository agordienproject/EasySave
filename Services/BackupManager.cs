using EasySave.Enums;
using EasySave.Models;
using EasySave.Services.Interfaces;
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
            _jsonFileManager = new JsonFileManager(GetBackupJobsFilePath());
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

        public async Task ExecuteBackupJobs(List<int> backupJobsIndex)
        {
            await ReadBackups();

            Console.WriteLine("before");
            if (_backupJobs.Count == 0)
            {
                Console.WriteLine("No backup job to execute !");
                return;
            }
            Console.WriteLine("after");

            List<BackupJob> backupJobsToExecute = _backupJobs.Where((item, index) => backupJobsIndex.Contains(index)).ToList();
            
            foreach (var backupJob in backupJobsToExecute)
            {
                await ExecuteBackupJob(backupJob);
            }
        }

        public async Task ExecuteBackupJob(BackupJob backupJob)
        {
            if (backupJob.BackupType == BackupType.Complete)
            {
                //Console.WriteLine($"Complete backup of : {backupJob.BackupName}");
                await CopyFilesRecursively(backupJob.BackupName, backupJob.SourceDirectory, backupJob.TargetDirectory);
            }
            else if (backupJob.BackupType == BackupType.Differential)
            {
                //Console.WriteLine($"Differential backup of : {backupJob.BackupName}");
                await CopyFilesRecursively(backupJob.BackupName, backupJob.SourceDirectory, backupJob.TargetDirectory);
            }
        }

        public async Task CopyFilesRecursively(string backupJobName, string sourceDir, string targetDir)
        {
            // Vérifier si le répertoire source existe
            if (!Directory.Exists(sourceDir))
            {
                Console.WriteLine($"Le répertoire source '{sourceDir}' n'existe pas.");
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

                DateTime before = DateTime.Now;
                File.Copy(filePath, destFilePath, true);
                DateTime after = DateTime.Now;
                double transferTime = after.Subtract(before).TotalSeconds;
                await _logManager.CreateLog(new Log(
                    backupJobName,
                    filePath,
                    destFilePath,
                    fileInfo.Length,
                    transferTime,
                    DateTime.Now
                    ));
                Console.WriteLine($"Copie du fichier : {fileName}");
            }

            // Copier les fichiers des sous-répertoires
            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                string subDirName = Path.GetFileName(subDir);
                string targetSubDir = Path.Combine(targetDir, subDirName);
                await CopyFilesRecursively(backupJobName, subDir, targetSubDir);
            }
        }

        private string GetBackupJobsFilePath()
        {
            string folderPath = @".\Data\BackupJobs\";
            string filePath = @".\Data\BackupJobs\backupjobs.json";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            if (!File.Exists(filePath))
            {
                using (FileStream fs = File.Create(filePath))
                {
                }
            }

            return filePath;
        }
    }
}
