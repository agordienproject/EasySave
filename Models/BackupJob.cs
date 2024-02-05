using EasySave.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Models
{
    public class BackupJob
    {
        public string BackupName { get; set; }
        public string SourceDirectory { get; set; }
        public string TargetDirectory { get; set; }
        public BackupType BackupType { get; set; }

        public BackupJob(string backupName, string sourceDirectory, string targetDirectory, BackupType backupType)
        {
            BackupName = backupName;
            SourceDirectory = sourceDirectory;
            TargetDirectory = targetDirectory;
            BackupType = backupType;
        }
        
    }
}
