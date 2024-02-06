namespace EasySave.Services.Interfaces
{
    public interface IFileManager
    {
        Task<List<T>?> Read<T>();
        Task Write<T>(List<T> list);
    }
}