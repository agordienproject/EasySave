using EasySave.Enums;
using EasySave.Models;
using EasySave.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EasySave.Services
{
    public class BackupManager
    {
        private readonly IConfiguration _configuration;
        private readonly IJsonFileManager _jsonFileManager;

        private List<BackupJob>? _backupJobs;

        private const int BACKUPJOBS_LIMIT = 5;

        public BackupManager(IConfiguration configuration) 
        {
            _configuration = configuration;
            _jsonFileManager = new JsonFileManager(_configuration.GetValue<string>("BackupJobsJsonPath"));
        }

        public List<BackupJob> GetBackupJobs()
        {
            ReadBackups();
            return _backupJobs;
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
                Console.WriteLine("Too many backups already exist.");
                return;
            }
            
            if (_backupJobs.Any(backup => backup.BackupName == backupJob.BackupName))
            {
                Console.WriteLine("Two backups can't have the same name.");
                return;
            }
            
            _backupJobs.Add(backupJob);

            WriteBackups();
        }

        public void DeleteBackupJob(BackupJob backupJob)
        {
            _backupJobs.Remove(backupJob);
            
            Console.WriteLine($"{backupJob.BackupName} successfuly deleted");

            WriteBackups();
        }

        public async Task ExecuteBackupJob(BackupJob backupJob)
        {
            if (backupJob.BackupType == BackupType.Complete)
            {
                Console.WriteLine($"Complete backup of : {backupJob.BackupName}");
                await CopyFilesRecursively(backupJob.SourceDirectory, backupJob.TargetDirectory);
            }
            else if (backupJob.BackupType == BackupType.Differential)
            {
                Console.WriteLine($"Diferential backup of : {backupJob.BackupName}");
                await CopyFilesRecursively(backupJob.SourceDirectory, backupJob.TargetDirectory);
            }
        }

        public async Task ExecuteBackupJobs(List<BackupJob> backupJobsToExecute)
        {
            foreach (var backupJobToExecute in backupJobsToExecute)
            {
                await ExecuteBackupJob(backupJobToExecute);
            }
        }

        public void DisplayBackupJobs()
        {
            ReadBackups();
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

        public static async Task CopyFilesRecursively(string sourceDir, string targetDir)
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
                string fileName = Path.GetFileName(filePath);
                string destFilePath = Path.Combine(targetDir, fileName);
                File.Copy(filePath, destFilePath, true);
                Console.WriteLine($"Copie du fichier : {fileName}");
            }

            // Copier les fichiers des sous-répertoires
            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                string subDirName = Path.GetFileName(subDir);
                string targetSubDir = Path.Combine(targetDir, subDirName);
                await CopyFilesRecursively(subDir, targetSubDir);
            }
        }
    }
}
