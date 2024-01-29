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
        public int TotalFileNumber { get; set; }
        public int TotalFileSize { get; set; }

    }
}
