using EasySave.Services.Interfaces;
using System.IO;
using System.Text.Json;

namespace EasySave.Services
{
    public class JsonFileService : IFileService
    {
        private readonly string _filePath;
        
        public JsonFileService(string filePath) 
        {
            _filePath = filePath;   
            InitFile(filePath);
        }

        public static void InitFile(string filePath)
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
        }

        //public async Task<bool> InitDataFile(string filePath)
        //{
        //    try
        //    {
        //        string directoryPath = Path.GetDirectoryName(filePath);

        //        if (!Directory.Exists(directoryPath))
        //        {
        //            Directory.CreateDirectory(directoryPath);
        //        }

        //        if (!File.Exists(filePath))
        //        {
        //            using (FileStream fs = File.Create(filePath))
        //            {
        //            }
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Gérer l'exception (journalisation, remontée, etc.)
        //        Console.WriteLine($"Erreur lors de l'initialisation du fichier : {ex.Message}");
        //        return false;
        //    }
        //}

        public async Task<List<T>?> Read<T>()
        {
            using FileStream openStream = File.OpenRead(_filePath);

            List<T>? list = new();

            try
            {
                list = await JsonSerializer.DeserializeAsync<List<T>?>(openStream);
            }
            catch (JsonException e)
            {

            }

            return list;
        }

        public async Task Write<T>(List<T> list)
        {
            var options = new JsonSerializerOptions { WriteIndented = true, };
            using FileStream openStream = File.Open(_filePath, FileMode.Truncate);
            await JsonSerializer.SerializeAsync(openStream, list, options);
        }
    }
}
