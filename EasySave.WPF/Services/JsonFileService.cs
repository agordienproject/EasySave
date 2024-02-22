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

        public List<T>? Read<T>()
        {
            if (string.IsNullOrWhiteSpace(_filePath) || !File.Exists(_filePath))
            {
                return null;
            }

            using (FileStream openStream = File.OpenRead(_filePath))
            {
                try
                {
                    return JsonSerializer.Deserialize<List<T>>(openStream);
                }
                catch (JsonException ex)
                {
                    return null;
                }
            }
        }

        public void Write<T>(List<T> list)
        {
            if (string.IsNullOrWhiteSpace(_filePath))
            {
                return;
            }

            var options = new JsonSerializerOptions { WriteIndented = true };

            using (FileStream openStream = File.Open(_filePath, FileMode.Create))
            {
                try
                {
                    JsonSerializer.Serialize(openStream, list, options);
                }
                catch (Exception ex)
                {
                    
                }
            }
        }
    }
}
