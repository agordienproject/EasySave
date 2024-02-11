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
        Task<bool> InitDataFile(string filePath);
    }
}
