namespace EasySave.Services.Interfaces
{
    public interface IFileManager
    {
        List<T>? Read<T>();
        void Write<T>(List<T> list);
    }
}