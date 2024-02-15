using EasySave.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EasySave.Domain.Models
{
    public class State : NamedEntity
    {
        public BackupState BackupState { get; set; }
        public string? BackupTime { get; set; }
        public int TotalFilesNumber { get; set; }
        public long TotalFilesSize { get; set; }
        public int NbFilesLeftToDo { get; set; }
        public long FilesSizeLeftToDo { get; set; }
        public string? SourceTransferingFilePath { get; set; }
        public string? TargetTransferingFilePath { get; set; }


        [JsonConstructor]
        public State(string backupName, BackupState backupState, string? backupTime, int totalFilesNumber, long totalFilesSize, int nbFilesLeftToDo, long filesSizeLeftToDo, string? sourceTransferingFilePath, string? targetTransferingFilePath) : base(backupName)
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

        public State(string backupName) : base(backupName)
        {
            BackupState = Enums.BackupState.Inactive;
            BackupTime = DateTime.Now.ToString();
            TotalFilesNumber = 0;
            TotalFilesSize = (long)0;
            NbFilesLeftToDo = 0;
            FilesSizeLeftToDo = (long)0;
            SourceTransferingFilePath = "";
            TargetTransferingFilePath = "";
                      
        }
        
    }
}
