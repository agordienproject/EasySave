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
            using FileStream openStream = File.OpenRead(_filePath);

            List<T>? list = new();

            try
            {
                list = JsonSerializer.Deserialize<List<T>?>(openStream);
            }
            catch (JsonException e)
            {

            }

            return list;
        }

        public void Write<T>(List<T> list)
        {
            var options = new JsonSerializerOptions { WriteIndented = true, };
            using FileStream openStream = File.Open(_filePath, FileMode.Truncate);
            JsonSerializer.Serialize(openStream, list, options);
        }
    }
}
