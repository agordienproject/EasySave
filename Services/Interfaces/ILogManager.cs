using EasySave.Models;

namespace EasySave.Services.Interfaces
{
    public interface ILogManager
    {
        Task CreateLog(Log log);
        Task ReadLogs();
        Task WriteLogs();
    }
}