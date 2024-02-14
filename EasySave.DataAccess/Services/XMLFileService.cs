using EasySave.Domain.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace EasySave.DataAccess.Services
{
    public class XMLFileService : IFileService
    {
        private readonly string _filePath;

        public XMLFileService()
        {
            _filePath = "";
            //InitFile(_filePath);
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
