using ConsoleTables;
using EasySave.Models;
using EasySave.Resources;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.Views
{
    public static class ConsoleView
    { 
        public static string Print()
        {
            string text = @"
               ____    __    ___  _  _  ___    __  _  _  ____ 
              ( ___)  /__\  / __)( \/ )/ __)  /__\( \/ )( ___)
               )__)  /(__)\ \__ \ \  / \__ \ /(__)\\  /  )__) 
              (____)(__)(__)(___/ (__) (___/(__)(__)\/  (____)";
            return $"{Resources.Language.Welcome} {text}";
        }

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
            Console.WriteLine(Resources.Language.NoSourceDirMessage);
            Console.Write(sourceDir);
        }
        public static void TooManyBackup()
        {
            Console.WriteLine(Resources.Language.TooManyBackup);
        }
        public static void SameNameBackup()
        {
            Console.WriteLine(Resources.Language.SameNameBackup);
        }
        public static void SuccessDelete(string backupJobName)
        {
            Console.Write(backupJobName);
            Console.WriteLine(Resources.Language.SuccessDelete);
        }
        public static void NoBackupJobToExecute()
        {
            Console.WriteLine(Resources.Language.NoBackupJobToExecute);
        }
        public static void CompleteBackup()
        {
            Console.WriteLine(Resources.Language.CompleteBackup);
        }
        public static void DifferentialBackup()
        {
            Console.WriteLine(Resources.Language.DifferentialBackup);
        }
        public static void StartCopyFilesDifferential()
        {
            Console.WriteLine(Resources.Language.StartCopyFilesDifferential);
        }
        public static void StartCopyFileAndUpdateLog()
        {
            Console.WriteLine(Resources.Language.StartCopyFileAndUpdateLog);
        }
        public static void CopyNewFile()
        {
            Console.WriteLine(Resources.Language.CopyNewFile);
        }
        public static void NoBackupJobFound()
        {
            Console.WriteLine(Resources.Language.NoBackupJobFound);
        }
        public static void CopyFile(string sourceFilePath)
        {
            Console.WriteLine(Resources.Language.CopyFile + Path.GetFileName(sourceFilePath));
        }
        public static void CopyFileInDestinationDirectory(string fileName, string destFilePath)
        {
            Console.WriteLine(Resources.Language.CopyFile + fileName + Resources.Language.In + destFilePath);
        }
        public static void DontCopyFile(FileInfo destFileInfo)
        {
            Console.WriteLine(Resources.Language.TheFile + destFileInfo + Resources.Language.DontCopyFile);
        }
        public static void ErrorSameLanguage()
        {
            Console.WriteLine(Resources.Language.ErrorSameLanguage);
        }
        public static void UpdateLanguage(string cultureName)
        {
            Console.WriteLine(Resources.Language.UpdateLanguage + cultureName);
        }
        public static void ErrorLanguage1()
        {
            Console.WriteLine(Resources.Language.ErrorLanguage1);
        }
        public static void ErrorLanguage2()
        {
            Console.WriteLine(Resources.Language.ErrorLanguage2);
        }
        public static void ChoosenLanguageCommand(Dictionary<string, string> languageDictionary, string language)
        {
            Console.WriteLine(Resources.Language.ChoosenLanguageCommand + languageDictionary[language]);
        }
        public static void EnterCommand()
        {
            Console.WriteLine(Resources.Language.EnterCommand);
        }
    }
}

