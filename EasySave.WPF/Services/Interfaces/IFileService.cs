namespace EasySave.Services.Interfaces
{
    public interface IFileService
    {
        Task<List<T>?> Read<T>();
        Task Write<T>(List<T> list);
    }
}
