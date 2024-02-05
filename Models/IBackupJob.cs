using EasySave.Enums;

namespace EasySave.Models
{
    public interface IBackupJob
    {
        string BackupName { get; set; }
        BackupType BackupType { get; set; }
        string SourceDirectory { get; set; }
        string TargetDirectory { get; set; }
    }
}