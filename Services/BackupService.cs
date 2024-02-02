//using EasySave.Enums;
//using EasySave.Models;
//using Microsoft.Extensions.Configuration;
//using System.Configuration;
//using System.Text.Json;

//namespace EasySave.Services
//{
//    public static class BackupService
//    {
//        public static async Task CreateBackup(string name, string sourcePath, string destinationPath, BackupType backupType)
//        {
//            List<BackupJob> backupList = await ReadBackups();

//            if (backupList.Count < 5)
//            {
//                if (!backupList.Any(backup => backup.BackupName == name))
//                {
//                    backupList.Add(new BackupJob(name, sourcePath, destinationPath, backupType));
//                } else
//                {
//                    Console.WriteLine("Two backups can't have the same name.");
//                }

//            } else
//            {
//                Console.WriteLine("Too many backups already exist.");
//            }

//            WriteBackups(backupList);
//        }

//        public static async Task DeleteBackup(string name)
//        {
//            List<BackupJob> backupList = await ReadBackups();

//            BackupJob backupToDelete = backupList.Find(backup => backup.BackupName == name);

//            if (backupToDelete != null) 
//            {
//                backupList.Remove(backupToDelete);
//                Console.WriteLine($"{backupToDelete.BackupName} successfuly deleted");
//            } else
//            {
//                Console.WriteLine($"No backup named : {name} was found.");
//            }

//            WriteBackups(backupList);
//        }

//        public static async Task ShowBackups()
//        {
//            List<BackupJob> backupList = await ReadBackups();
//            int i = 0;
//            foreach (var backup in backupList)
//            {
//                Console.WriteLine($"---------------{i}-----------------");
//                Console.WriteLine($"Name : {backup.BackupName}");
//                Console.WriteLine($"Source directory : {backup.SourceDirectory}");
//                Console.WriteLine($"Destination directory : {backup.TargetDirectory}");
//                Console.WriteLine($"Backup type : {backup.BackupType}");
//                Console.WriteLine("----------------------------------");
//                i++;
//            }
//        }

//        public static async Task SaveBackup()
//        {
//            List<BackupJob> backups = await ReadBackups();

//            foreach (var backup in backups)
//            {
//                if (backup.BackupType == BackupType.Complete)
//                {
//                    Console.WriteLine($"Complete backup of : {backup.BackupName}");
//                    CopyFilesRecursively(backup.SourceDirectory, backup.TargetDirectory);
//                } else if (backup.BackupType == BackupType.Differential)
//                {
//                    Console.WriteLine($"Diferential backup of : {backup.BackupName}");
//                    CopyFilesRecursively(backup.SourceDirectory, backup.TargetDirectory);
//                }
//            }
//        }

//        private static async Task<List<BackupJob>> ReadBackups()
//        {
//            string backupsFilePath = @".\Backups\backups.json";

//            using FileStream openStream = File.OpenRead(backupsFilePath);

//            List<BackupJob> backupList = new List<BackupJob>();

//            try
//            {
//                backupList = await JsonSerializer.DeserializeAsync<List<BackupJob?>>(openStream);
//            }
//            catch (JsonException e)
//            {
//                Console.WriteLine(e.Message);
//            }

//            return backupList;
//        }

//        private static async Task WriteBackups(List<BackupJob> backupList)
//        {
//            string backupsFilePath = @".\Backups\backups.json";

//            var options = new JsonSerializerOptions { WriteIndented = true, };

//            using FileStream openStream = File.Open(backupsFilePath, FileMode.Truncate);
//            await JsonSerializer.SerializeAsync(openStream, backupList, options);
//        }

//        private static void CopyFilesRecursively(string sourceDir, string targetDir)
//        {
//            // Vérifier si le répertoire source existe
//            if (!Directory.Exists(sourceDir))
//            {
//                Console.WriteLine($"Le répertoire source '{sourceDir}' n'existe pas.");
//                return;
//            }

//            // Créer le répertoire cible s'il n'existe pas
//            if (!Directory.Exists(targetDir))
//            {
//                Directory.CreateDirectory(targetDir);
//            }

//            // Copier les fichiers du répertoire source vers le répertoire cible
//            foreach (string filePath in Directory.GetFiles(sourceDir))
//            {
//                string fileName = Path.GetFileName(filePath);
//                string destFilePath = Path.Combine(targetDir, fileName);
//                File.Copy(filePath, destFilePath, true);
//                Console.WriteLine($"Copie du fichier : {fileName}");
//            }

//            // Copier les fichiers des sous-répertoires
//            foreach (string subDir in Directory.GetDirectories(sourceDir))
//            {
//                string subDirName = Path.GetFileName(subDir);
//                string targetSubDir = Path.Combine(targetDir, subDirName);
//                CopyFilesRecursively(subDir, targetSubDir);
//            }
//        }
//    }
//}
