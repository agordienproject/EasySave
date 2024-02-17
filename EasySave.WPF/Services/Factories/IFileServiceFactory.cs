using EasySave.Services.Interfaces;

namespace EasySave.Services.Factories
{
    public interface IFileServiceFactory
    {
        public abstract IFileService CreateFileService(string type, string filePath);
    }
}
