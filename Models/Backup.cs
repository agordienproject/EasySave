using EasySave.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Models
{
    public class Backup
    {
        public string Name { get; set; }
        public string SourceDirectory { get; set; }
        public string DestinationDirectory { get; set; }
        public BackupType Type { get; set; }

        public Backup(string name, string sourceDirectory, string destinationDirectory, BackupType type)
        {
            this.Name = name;
            this.SourceDirectory = sourceDirectory;
            this.DestinationDirectory = destinationDirectory;
            this.Type = type;
        }
    }
}
