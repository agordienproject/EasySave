using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Domain.Models
{
    public class NamedEntity
    {
        public string? BackupName { get; set; }
        
        public NamedEntity(string backupName)
        {
            BackupName = backupName;
        }

    }
}
