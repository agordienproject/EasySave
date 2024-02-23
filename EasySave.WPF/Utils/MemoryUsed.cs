using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.WPF.Utils
{
    class MemoryUsed
    {
        public static bool IsMaxMemoryReached(long MaxMemory)
        {
            string PROCESSNAME = "EasySave";

            Process[] processes = Process.GetProcessesByName(PROCESSNAME);

            if (processes.Length > 0)
            {
                Process process = processes[0];

                long ProcessMemory = process.WorkingSet64 / 1048576;

                if(ProcessMemory > MaxMemory)
                {
                    return true;
                }
            }

            return false;
            
        }
    }
}