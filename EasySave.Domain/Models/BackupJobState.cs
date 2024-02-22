using EasySave.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Domain.Models
{
    public class BackupJobState
    {
        public BackupState BackupState { get; set; }
        public string? BackupTime { get; set; }
        public int TotalFilesNumber { get; set; }
        public long TotalFilesSize { get; set; }
        public int NbFilesLeftToDo { get; set; }
        public long FilesSizeLeftToDo { get; set; }
        public string? SourceTransferingFilePath { get; set; }
        public string? TargetTransferingFilePath { get; set; }

        public BackupJobState(BackupState backupState, string? backupTime, int totalFilesNumber, long totalFilesSize, int nbFilesLeftToDo, long filesSizeLeftToDo, string? sourceTransferingFilePath, string? targetTransferingFilePath)
        {
            BackupState = backupState;
            BackupTime = backupTime;
            TotalFilesNumber = totalFilesNumber;
            TotalFilesSize = totalFilesSize;
            NbFilesLeftToDo = nbFilesLeftToDo;
            FilesSizeLeftToDo = filesSizeLeftToDo;
            SourceTransferingFilePath = sourceTransferingFilePath;
            TargetTransferingFilePath = targetTransferingFilePath;
        }
    }
}
