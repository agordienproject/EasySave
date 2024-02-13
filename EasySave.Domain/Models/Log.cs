using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EasySave.Domain.Models
{
    public class Log : NamedEntity
    {
        public string SourceFile { get; set;}
        public string TargetFile { get; set;}
        public long FileSize {get; set;}
        public double FileTransferTime {get; set;}
        public string TimeStamp {get; set;}

        public Log(string backupName, string sourceFile, string targetFile, long fileSize, double fileTransferTime, string timeStamp) : base(backupName)
        {
            SourceFile = sourceFile;
            TargetFile = targetFile;
            FileSize = fileSize;
            FileTransferTime = fileTransferTime;
            TimeStamp = timeStamp;
        }
    }
}
