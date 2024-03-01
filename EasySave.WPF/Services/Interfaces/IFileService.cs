namespace EasySave.Services.Interfaces
{
    public interface IFileService
    {
        List<T>? Read<T>();
        void Write<T>(List<T> list);
    }
}
