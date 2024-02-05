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
        private readonly IJsonFileManager _jsonFileManager;
        
        private readonly LogManager _logManager;
        private readonly StateManager _stateManager;

        private List<BackupJob>? _backupJobs;

        private const int BACKUPJOBS_LIMIT = 5;

        public BackupManager(IConfiguration configuration) 
        {
            _configuration = configuration;
            _jsonFileManager = new JsonFileManager(_configuration.GetValue<string>("BackupJobsJsonPath"));
            _logManager = new LogManager(configuration);
            _stateManager = new StateManager(configuration);
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
            _stateManager.CreateState(new State(backupJob.BackupName));

            WriteBackups();
        }

        public void DeleteBackupJob(string backupJobName)
        {
            ReadBackups();

            BackupJob backupJobToDelete = _backupJobs.Find(backupJob => backupJob.BackupName == backupJobName);

            if (backupJobToDelete == null) 
            {
                Console.WriteLine($"No backupJob named : {backupJobName} was found !");
                return;
            }
            
            _backupJobs.Remove(backupJobToDelete);
            _stateManager.DeleteState(backupJobName);
            Console.WriteLine($"{backupJobName} successfuly deleted");

            WriteBackups();
        }

        public async Task ExecuteBackupJob(BackupJob backupJob)
        {
            if (backupJob.BackupType == BackupType.Complete)
            {
                Console.WriteLine($"Complete backup of : {backupJob.BackupName}");
                await CopyFilesRecursively(backupJob.BackupName, backupJob.SourceDirectory, backupJob.TargetDirectory);
            }
            else if (backupJob.BackupType == BackupType.Differential)
            {
                Console.WriteLine($"Differential backup of : {backupJob.BackupName}");
                await CopyFilesRecursively(backupJob.BackupName, backupJob.SourceDirectory, backupJob.TargetDirectory);
            }
        }

        public async Task ExecuteBackupJobs(List<int> backupJobsIndex)
        {
            ReadBackups();

            foreach (var index in backupJobsIndex)
            {
                if (_backupJobs.ElementAt(index - 1) != null)
                {
                    await ExecuteBackupJob(_backupJobs.ElementAt(index - 1));
                }
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
                _logManager.CreateLog(new Log(
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
    }
}
