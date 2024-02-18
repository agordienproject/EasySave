using EasySave.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EasySave.Models
{
    public class Log : INamedEntity
    {
        public string BackupName { get; set; }
        public string SourceFile { get; set;}
        public string TargetFile { get; set;}
        public long FileSize {get; set;}
        public double FileTransferTime {get; set;}
        public string TimeStamp {get; set;}

        public Log()
        {

        }

        public Log(string backupName, string sourceFile, string targetFile, long fileSize, double fileTransferTime, string timeStamp)
        {
            this.BackupName = backupName;
            this.SourceFile = sourceFile;
            this.TargetFile = targetFile;
            this.FileSize = fileSize;
            this.FileTransferTime = fileTransferTime;
            this.TimeStamp = timeStamp;
        }
    }
}
