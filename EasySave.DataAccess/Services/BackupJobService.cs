using EasySave.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasySave.Domain.Services
{
    public class BackupJobService : IBackupJobService
    {
        private readonly string _filePath;

        public BackupJobService()
        {
            _filePath = filePath;
            InitDataFile(_filePath).Wait(); // Attendez la fin de l'initialisation avant de continuer.
        }

        public async Task<bool> InitDataFile(string filePath)
        {
            try
            {
                string directoryPath = Path.GetDirectoryName(filePath);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                if (!File.Exists(filePath))
                {
                    using (FileStream fs = File.Create(filePath))
                    {
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Gérer l'exception (journalisation, remontée, etc.)
                Console.WriteLine($"Erreur lors de l'initialisation du fichier : {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<BackupJob>> GetAll()
        {
            try
            {
                using (FileStream openStream = File.OpenRead(_filePath))
                {
                    List<BackupJob>? backupJobs = await JsonSerializer.DeserializeAsync<List<BackupJob>>(openStream);
                    return backupJobs ?? new List<BackupJob>();
                }
            }
            catch (Exception ex)
            {
                // Gérer l'exception (journalisation, remontée, etc.)
                Console.WriteLine($"Erreur lors de la lecture du fichier : {ex.Message}");
                return new List<BackupJob>();
            }
        }

        public async Task<BackupJob> Get(string name)
        {
            try
            {
                IEnumerable<BackupJob> backupJobs = await GetAll();
                return backupJobs.FirstOrDefault(backupJob => backupJob.BackupName == name);
            }
            catch (Exception ex)
            {
                // Gérer l'exception (journalisation, remontée, etc.)
                Console.WriteLine($"Erreur lors de la récupération de la sauvegarde : {ex.Message}");
                return null;
            }
        }

        public async Task<BackupJob> Create(BackupJob entity)
        {
            try
            {
                List<BackupJob> backupJobs = (List<BackupJob>)await GetAll();
                backupJobs.Add(entity);

                using (FileStream createStream = File.Create(_filePath))
                {
                    await JsonSerializer.SerializeAsync(createStream, backupJobs, new JsonSerializerOptions { WriteIndented = true });
                }

                return entity;
            }
            catch (Exception ex)
            {
                // Gérer l'exception (journalisation, remontée, etc.)
                Console.WriteLine($"Erreur lors de la création de la sauvegarde : {ex.Message}");
                return null;
            }
        }

        public async Task<bool> Delete(string name)
        {
            try
            {
                List<BackupJob> backupJobs = (List<BackupJob>)await GetAll();
                BackupJob backupToDelete = backupJobs.FirstOrDefault(backupJob => backupJob.BackupName == name);

                if (backupToDelete != null)
                {
                    backupJobs.Remove(backupToDelete);

                    using (FileStream createStream = File.Create(_filePath))
                    {
                        await JsonSerializer.SerializeAsync(createStream, backupJobs, new JsonSerializerOptions { WriteIndented = true });
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                // Gérer l'exception (journalisation, remontée, etc.)
                Console.WriteLine($"Erreur lors de la suppression de la sauvegarde : {ex.Message}");
                return false;
            }
        }

        public async Task<BackupJob> Update(string name, BackupJob entity)
        {
            try
            {
                List<BackupJob> backupJobs = (List<BackupJob>)await GetAll();
                BackupJob existingBackup = backupJobs.FirstOrDefault(backupJob => backupJob.BackupName == name);

                if (existingBackup != null)
                {
                    int index = backupJobs.IndexOf(existingBackup);
                    backupJobs[index] = entity;

                    using (FileStream createStream = File.Create(_filePath))
                    {
                        await JsonSerializer.SerializeAsync(createStream, backupJobs, new JsonSerializerOptions { WriteIndented = true });
                    }

                    return entity;
                }

                return null;
            }
            catch (Exception ex)
            {
                // Gérer l'exception (journalisation, remontée, etc.)
                Console.WriteLine($"Erreur lors de la mise à jour de la sauvegarde : {ex.Message}");
                return null;
            }
        }
    }

}
