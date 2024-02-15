using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySave.Domain.Enums;


namespace EasySave.Domain.Models
{
    public class BackupJob : NamedEntity
    {
        public string SourceDirectory { get; set; }
        public string TargetDirectory { get; set; }
        public BackupType BackupType { get; set; }

        public BackupJob(string backupName, string sourceDirectory, string targetDirectory, BackupType backupType) : base(backupName)
        {
            SourceDirectory = sourceDirectory;
            TargetDirectory = targetDirectory;
            BackupType = backupType;
        }

        public BackupJob() : base("")
        {
            SourceDirectory = "";
            TargetDirectory = "";
            BackupType = BackupType.Complete;
        }
        
    }
}
