using EasySave.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Domain.Models
{
    public class BackupJobProgress
    {
        public Guid BackupJobId { get; set; }
        public BackupState BackupState { get; set; }
        public double FilesSizeLeftToDo { get; set; }
    }
}
