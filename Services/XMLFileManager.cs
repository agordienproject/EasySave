using EasySave.Services.Interfaces;
using System.Xml.Serialization;

namespace EasySave.Services
{
    public class XMLFileManager : IFileManager
    {
        private readonly string _filePath;

        public XMLFileManager(string filePath)
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
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            using (StreamReader reader = new StreamReader(_filePath))
            {
                return (List<T>)serializer.Deserialize(reader);
            }
        }

        public void Write<T>(List<T> list)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            using (StreamWriter writer = new StreamWriter(_filePath))
            {
                serializer.Serialize(writer, list);
            }
        }

    }
}