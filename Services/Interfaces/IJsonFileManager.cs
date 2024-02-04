namespace EasySave.Services.Interfaces
{
    public interface IJsonFileManager
    {
        List<T>? Read<T>();
        void Write<T>(List<T> list);
    }
}