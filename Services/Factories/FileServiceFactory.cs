using EasySave.Services;
using EasySave.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Services.Factories
{
    public class FileServiceFactory : IFileServiceFactory
    {
        public IFileManager CreateFileService(string type, string filePath)
        {
            switch (type)
            {
                case "json":
                    return new JsonFileManager(filePath);
                case "xml":
                    return new XMLFileManager(filePath);
                default:
                    throw new ArgumentException("Invalid type", "type");
            }
        }
    }
}