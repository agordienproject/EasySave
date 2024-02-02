﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EasySave.Models
{
    public class Log
    {
        public string Name { get; set; }
        public string SourceFile { get; set;}
        public string TargetFile { get; set;}
        public int FileSize {get; set;}
        public float FileTransferTime {get; set;}
        public DateTime TimeStamp {get; set;}

        public Log(string name, string sourceFile, string targetFile, int fileSize, float fileTransferTime, DateTime timeStamp)
        {
            this.Name = name;
            this.SourceFile = sourceFile;
            this.TargetFile = targetFile;
            this.FileSize = fileSize;
            this.FileTransferTime = fileTransferTime;
            this.TimeStamp = timeStamp;

        }
    }
}
