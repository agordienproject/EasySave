﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Domain.Models
{
    public interface INamedEntity
    {
        string? BackupName { get; set; }
    }
}
