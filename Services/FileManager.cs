using EasySave.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasySave.Services
{
    public class FileManager
    {
        public string _filePath;

        public FileManager(string filePath) 
        {
            _filePath = filePath;
        }

        public async Task<List<T>> Read<T>()
        {
            using FileStream openStream = File.OpenRead(_filePath);

            List<T> list = new List<T>();

            try
            {
                list = await JsonSerializer.DeserializeAsync<List<T?>>(openStream);
            }
            catch (JsonException e)
            {
                Console.WriteLine(e.Message);
            }

            return list;
        }

        public async Task Write<T>(List<T> list) 
        {
            var options = new JsonSerializerOptions { WriteIndented = true, };
            using FileStream openStream = File.Open(_filePath, FileMode.Truncate);
            await JsonSerializer.SerializeAsync(openStream, list, options);
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
