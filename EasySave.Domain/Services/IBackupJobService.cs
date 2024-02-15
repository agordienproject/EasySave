using EasySave.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Domain.Services
{
    public interface IBackupJobService : IDataService<BackupJob>
    {
        Task ExecuteBackupJobs(List<int> backupJobsIndex);
        Task ExecuteBackupJob(BackupJob backupJob);
    }
}
