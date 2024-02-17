using EasySave.Services.Interfaces;

namespace EasySave.Services.Factories
{
    public class FileServiceFactory : IFileServiceFactory
    {
        public IFileService CreateFileService(string type, string filePath)
        {
            switch (type)
            {
                case "json":
                    return new JsonFileService(filePath);
                case "xml":
                    return new XMLFileService(filePath);
                default:
                    throw new ArgumentException("Invalid type", "type");
            }
        }
    }
}
