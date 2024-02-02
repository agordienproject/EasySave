using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Models
{
    public class State
    {
        public string BackupName { get; set; }
        public DateTime BackupTime { get; set; }
        public string BackupState { get; set; }
        public int TotalFilesNumber { get; set; }
        public int TotalFilesSize { get; set; }

        public State(string backupName, DateTime backupTime, string backupState, int totalFilesNumber, int totalFilesSize)
        {
            this.BackupName = backupName;
            this.BackupTime = backupTime;
            this.BackupState = backupState;
            this.TotalFilesNumber = totalFilesNumber;
            this.TotalFilesSize = totalFilesSize;
        }
    }
}
