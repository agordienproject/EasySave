using EasySave.Services.Interfaces;

namespace EasySave.Services.Factories
{
    public interface IFileServiceFactory
    {
        IFileManager CreateFileService(string type, string filePath);
    }
}