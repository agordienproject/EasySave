using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.WPF.Utils
{
    class BusinessAppChecker
    {
        public static bool IsBusinessAppRunning(string ProcessName)
        {
            Process[] processes = Process.GetProcesses();

            foreach (Process p in processes)
            {
                if (p.ProcessName.ToLower().Contains(ProcessName))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
