using EasySave.Enums;
using EasySave.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasySave.Services
{
    public static class BackupService
    {

        public static async Task CreateBackup(string name, string sourcePath, string destinationPath, BackupType backupType)
        {
            List<Backup> backupList = await ReadBackups();

            if (backupList.Count < 5)
            {
                if (!backupList.Any(backup => backup.Name == name))
                {
                    backupList.Add(new Backup(name, sourcePath, destinationPath, backupType));
                } else
                {
                    Console.WriteLine("Two backups can't have the same name.");
                }

            } else
            {
                Console.WriteLine("Too many backups already exist.");
            }

            WriteBackups(backupList);
        }

        public static async Task DeleteBackup(string name)
        {
            List<Backup> backupList = await ReadBackups();

            Backup backupToDelete = backupList.Find(backup => backup.Name == name);

            if (backupToDelete != null) 
            {
                backupList.Remove(backupToDelete);
                Console.WriteLine($"{backupToDelete.Name} successfuly deleted");
            } else
            {
                Console.WriteLine($"No backup named : {name} was found.");
            }

            WriteBackups(backupList);
        }

        public static async Task ShowBackups()
        {
            List<Backup> backupList = await ReadBackups();
            foreach (var backup in backupList)
            {
                Console.WriteLine(backup.Name);
                Console.WriteLine(backup.SourceDirectory);
                Console.WriteLine(backup.DestinationDirectory);
                Console.WriteLine(backup.Type);
            }
        }

        public static async Task SaveBackup()
        {
            List<Backup> backups = await ReadBackups();

            foreach (var backup in backups)
            {
                if (backup.Type == BackupType.Complete)
                {
                    Console.WriteLine($"Complete backup of : {backup.Name}");
                    CopyFilesRecursively(backup.SourceDirectory, backup.DestinationDirectory);
                } else if (backup.Type == BackupType.Differential)
                {
                    Console.WriteLine($"Diferential backup of : {backup.Name}");
                    CopyFilesRecursively(backup.SourceDirectory, backup.DestinationDirectory);
                }
            }
        }

        private static async Task<List<Backup>> ReadBackups()
        {
            string backupsFilePath = @".\backups.json";

            using FileStream openStream = File.OpenRead(backupsFilePath);

            List<Backup> backupList = new List<Backup>();

            try
            {
                backupList = await JsonSerializer.DeserializeAsync<List<Backup?>>(openStream);
            }
            catch (JsonException e)
            {
                Console.WriteLine(e.Message);
            }

            return backupList;
        }

        private static async Task WriteBackups(List<Backup> backupList)
        {
            string backupsFilePath = @".\backups.json";

            var options = new JsonSerializerOptions { WriteIndented = true, };

            using FileStream openStream = File.Open(backupsFilePath, FileMode.Truncate);
            await JsonSerializer.SerializeAsync(openStream, backupList, options);
        }

        private static void CopyFilesRecursively(string sourceDir, string targetDir)
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
                CopyFilesRecursively(subDir, targetSubDir);
            }
        }
    }
}
