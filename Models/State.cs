using EasySave.Enums;
using EasySave.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EasySave.Models
{
    public class State : INamedEntity
    {
        public string BackupName { get; set; }
        public BackupState BackupState { get; set; }
        public DateTime BackupTime { get; set; }
        public int TotalFilesNumber { get; set; }
        public int TotalFilesSize { get; set; }
        public int NbFilesLeftToDo { get; set; }
        public int FilesSizeLeftToDo { get; set; }
        public string? SourceTransferingFilePath { get; set; }
        public string? TargetTransferingFilePath { get; set; }

        [JsonConstructor]
        public State(string backupName, DateTime backupTime, BackupState backupState, int totalFilesNumber, int totalFilesSize, int nbFilesLeftToDo, int filesSizeLeftToDo, string sourceTransferingFilePath, string targetTransferingFilePath)
        {
            BackupName = backupName;
            BackupState = backupState;
            BackupTime = backupTime;
            TotalFilesNumber = totalFilesNumber;
            TotalFilesSize = totalFilesSize;
            NbFilesLeftToDo = nbFilesLeftToDo;
            FilesSizeLeftToDo = filesSizeLeftToDo;
            SourceTransferingFilePath = sourceTransferingFilePath;
            TargetTransferingFilePath = targetTransferingFilePath;
        }

        public State(string backupName)
        {
            BackupName = backupName;
            BackupState = BackupState.Inactive;
        }
        
    }
}
