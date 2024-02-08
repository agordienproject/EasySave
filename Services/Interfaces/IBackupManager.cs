using EasySave.Enums;
using EasySave.Models;

namespace EasySave.Services.Interfaces
{
    public interface IBackupManager
    {
        void CopyFiles(string backupJobName, string sourceDir, string targetDir, BackupType backupType, ref int nbFilesLeftToDo, ref long filesSizeLeftToDo);
        void CreateBackupJob(BackupJob backupJob);
        void DeleteBackupJob(string backupJobName);
        void DisplayBackupJobs();
        void ExecuteBackupJob(BackupJob backupJob);
        void ExecuteBackupJobs(List<int> backupJobsIndex);
        void ReadBackups();
        void WriteBackups();
    }
}