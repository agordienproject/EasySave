using EasySave.Models;

namespace EasySave.Services.Interfaces
{
    public interface ILogManager
    {
        void CreateLog(Log log);
        void ReadLogs();
        void WriteLogs();
    }
}