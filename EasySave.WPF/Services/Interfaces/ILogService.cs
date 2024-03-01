using EasySave.Models;

namespace EasySave.Services.Interfaces
{
    public interface ILogService
    {
        List<Log> GetAll();
        Log Create(Log entity);
    }
}
