using EasySave.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Domain.Models
{
    public class BackupJobInfo : INamedEntity
    {
        public string BackupName { get; set; }
        public string SourceDirectory { get; set; }
        public string TargetDirectory { get; set; }
        public BackupType BackupType { get; set; }
        public BackupState BackupState { get; set; }
        public string? BackupTime { get; set; }
        public int TotalFilesNumber { get; set; }
        public long TotalFilesSize { get; set; }
        public int NbFilesLeftToDo { get; set; }
        public long FilesSizeLeftToDo { get; set; }
        public string? SourceTransferingFilePath { get; set; }
        public string? TargetTransferingFilePath { get; set; }

        public BackupJobInfo
        (   string backupName,
            string sourceDirectory, 
            string targetDirectory, 
            BackupType backupType, 
            BackupState backupState, 
            string? backupTime, 
            int totalFilesNumber, 
            long totalFilesSize, 
            int nbFilesLeftToDo, 
            long filesSizeLeftToDo, 
            string? sourceTransferingFilePath, 
            string? targetTransferingFilePath)
        {
            BackupName = backupName;
            SourceDirectory = sourceDirectory;
            TargetDirectory = targetDirectory;
            BackupType = backupType;
            BackupState = backupState;
            BackupTime = backupTime;
            TotalFilesNumber = totalFilesNumber;
            TotalFilesSize = totalFilesSize;
            NbFilesLeftToDo = nbFilesLeftToDo;
            FilesSizeLeftToDo = filesSizeLeftToDo;
            SourceTransferingFilePath = sourceTransferingFilePath;
            TargetTransferingFilePath = targetTransferingFilePath;
        }
        
        public BackupJobInfo()
        {
            BackupName = "";
            SourceDirectory = "";
            TargetDirectory = "";
            BackupType = BackupType.Complete;
            BackupState = BackupState.Inactive;
            BackupTime = null;
            TotalFilesNumber = 0;
            TotalFilesSize = 0;
            NbFilesLeftToDo = 0;
            FilesSizeLeftToDo = 0;
            SourceTransferingFilePath = "";
            TargetTransferingFilePath = "";
        }
    }
}
