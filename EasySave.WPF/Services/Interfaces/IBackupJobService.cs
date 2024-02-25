using EasySave.Domain.Models;
using EasySave.Models;

namespace EasySave.Services.Interfaces
{
    public interface IBackupJobService
    {
        List<BackupJobInfo> GetAll();
        BackupJobInfo? Get(string name);
        BackupJobInfo Create(BackupJobInfo entity);
        BackupJobInfo? Update(BackupJobInfo entity);
        bool Delete(Guid name);
    }
}
