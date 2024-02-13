using EasySave.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.DataAccess.Services.Factories
{
    public class FileServiceFactory : IFileServiceFactory
    {
        public IFileService CreateFileService(string type)
        {
            switch (type)
            {
                case "json":
                    return new JsonFileService();
                case "xml":
                    return new XMLFileService();
                default:
                    throw new ArgumentException("Invalid type", "type");
            }
        }
    }
}
