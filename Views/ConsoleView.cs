using ConsoleTables;
using EasySave.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Views
{
    public static class ConsoleView
    {
        public static void DisplayBackupJobsTable(List<BackupJob> backupJobs)
        {
            var table = new ConsoleTable("BackupName", "SourceDirectory", "TargetDirectory", "BackupType");

            foreach (var backupJob in backupJobs)
            {
                table.AddRow(backupJob.BackupName, backupJob.SourceDirectory, backupJob.TargetDirectory, backupJob.BackupType);
            }

            table.Write();
        }

        public static void NoSourceDirMessage(string sourceDir)
        {
            Console.WriteLine($"Le répertoire source n'existe pas : ");
            Console.Write(sourceDir);
        }
    }
}
