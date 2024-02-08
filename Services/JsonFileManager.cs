using System.Text.Json;
using EasySave.Services.Interfaces;

namespace EasySave.Services
{
    public class JsonFileManager : IFileManager
    {
        private readonly string _filePath;

        public JsonFileManager(string filePath)
        {
            _filePath = filePath;
            InitJsonFile(filePath);
        }

        private void InitJsonFile(string filePath)
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
                //Console.WriteLine(e.Message);
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
