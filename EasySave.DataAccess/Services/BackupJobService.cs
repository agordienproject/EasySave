using EasySave.DataAccess.Services;
using EasySave.DataAccess.Services.Factories;
using EasySave.Domain.Enums;
using EasySave.Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasySave.Domain.Services
{
    public class BackupJobService : DataService<BackupJob>, IBackupJobService
    {
        private readonly ILogService _logService;

        public BackupJobService(IConfiguration configuration, IFileServiceFactory fileServiceFactory, ILogService logService)
        {
            string type = configuration["DataFilesTypes:BackupJobsFileType"];
            string filePath = Path.Combine(configuration["DataFilesLocation:BackupJobsFolderPath"], configuration["DataFilesLocation:BackupJobsJsonFileName"]);
            base._fileService = fileServiceFactory.CreateFileService(type, filePath);

            _logService = logService;
        }

        //public static async Task ExecuteBackupJob(BackupJob backupJob)
        //{
        //    await InitState(backupJob);

        //    await GetDirectoryInfos(backupJob.SourceDirectory);

        //    backupJob.NbFilesLeftToDo = backupJob.TotalFilesNumber;
        //    backupJob.FilesSizeLeftToDo = backupJob.TotalFilesSize;

        //    await CopyFiles(backupJob.BackupName, backupJob.SourceDirectory, backupJob.TargetDirectory, backupJob.BackupType);

        //    await ClearState(backupJob);
        //}

        //private static async Task InitState(BackupJob backupJob)
        //{
        //    backupJob.BackupState = Domain.Enums.BackupState.Active;
        //}

        //private static async Task ClearState(BackupJob backupJob)
        //{
        //    backupJob.BackupState = Domain.Enums.BackupState.Inactive;
        //    backupJob.BackupTime = DateTime.Now.ToString();
        //    backupJob.TotalFilesNumber = 0;
        //    backupJob.TotalFilesSize = (long)0;
        //    backupJob.NbFilesLeftToDo = 0;
        //    backupJob.FilesSizeLeftToDo = (long)0;
        //    backupJob.SourceTransferingFilePath = "";
        //    backupJob.TargetTransferingFilePath = "";
        //}

        //private static async Task GetDirectoryInfos(BackupJob backupJob)
        //{
        //    try
        //    {
        //        string[] files = Directory.GetFiles(backupJob.SourceDirectory);
        //        string[] subDirs = Directory.GetDirectories(backupJob.SourceDirectory);

        //        foreach (var fichier in files)
        //        {
        //            backupJob.TotalFilesNumber++;
        //            backupJob.TotalFilesSize += new FileInfo(fichier).Length;
        //        }

        //        foreach (var subDir in subDirs)
        //        {
        //            await GetDirectoryInfos(subDir);
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //}

        //private async Task CopyFiles(string backupJobName, string sourceDir, string targetDir, BackupType backupType)
        //{
        //    if (!Directory.Exists(sourceDir))
        //    {
        //        return;
        //    }

        //    if (!Directory.Exists(targetDir))
        //    {
        //        Directory.CreateDirectory(targetDir);
        //    }

        //    foreach (string filePath in Directory.GetFiles(sourceDir))
        //    {
        //        FileInfo fileInfo = new FileInfo(filePath);

        //        string targetPath = Path.Combine(targetDir, fileInfo.Name);

        //        SourceTransferingFilePath = filePath;
        //        TargetTransferingFilePath = targetPath;

        //        await CopyFile(backupJobName, filePath, targetDir, backupType);

        //        NbFilesLeftToDo--;
        //        FilesSizeLeftToDo -= fileInfo.Length;
        //    }

        //    foreach (string subDir in Directory.GetDirectories(sourceDir))
        //    {
        //        string subDirName = Path.GetFileName(subDir);
        //        string targetSubDir = Path.Combine(targetDir, subDirName);
        //        await CopyFiles(backupJobName, subDir, targetSubDir, backupType);
        //    }
        //}

        //private async Task CopyFile(string backupJobName, string sourceFilePath, string targetDir, BackupType backupType)
        //{
        //    string sourceFileName = Path.GetFileName(sourceFilePath);
        //    string targetFilePath = Path.Combine(targetDir, sourceFileName);

        //    bool shouldCopy = true;

        //    if (backupType == BackupType.Differential)
        //    {
        //        shouldCopy = ShouldCopyFile(sourceFilePath, targetFilePath);
        //    }

        //    bool targetFileExist = File.Exists(targetFilePath);

        //    if ((targetFileExist && shouldCopy) || !targetFileExist)
        //    {
        //        FileInfo sourceFileInfo = new(sourceFilePath);

        //        double transferTime;

        //        try
        //        {
        //            DateTime before = DateTime.Now;

        //            // Récupérer l'extension du fichier actuellement traité
        //            string fileExtension = Path.GetExtension(sourceFilePath);
        //            fileExtension = fileExtension.TrimStart('.'); // Enlève le "." au début de l'extension

        //            // Vérifier si l'extension est présente dans la liste des extensions autorisées
        //            IConfigurationSection authorizedExtensionsSection = _configuration.GetSection("FileExtensions:AuthorizedExtensions");
        //            List<string> authorizedExtensions = authorizedExtensionsSection.Get<List<string>>();

        //            if (authorizedExtensions.Contains(fileExtension))
        //            {
        //                string cryptoSoftPath = Path.Combine("..", "..", "..", "..", "CryptoSoft", "bin", "Debug", "net8.0", "CryptoSoft.exe");
        //                //string cryptoSoftPath = "C:\\Users\\Alexis\\Documents\\#CESI\\2023-2024\\PROJETS\\BLOC-PROXYS\\LIVRABLE-II\\V2.0\\CryptoSoft\\bin\\Debug\\net8.0\\CryptoSoft.exe";
        //                string cryptoSoftArg = String.Concat("-s ", sourceFilePath, " -d ", targetFilePath);
        //                Process.Start(cryptoSoftPath, cryptoSoftArg);

        //            }
        //            else
        //            {
        //                File.Copy(sourceFilePath, targetFilePath, true);
        //            }
        //            DateTime after = DateTime.Now;
        //            transferTime = after.Subtract(before).TotalSeconds;
        //        }
        //        catch (Exception)
        //        {
        //            transferTime = -1;
        //        }

        //        //await _logService.Create(new Log(
        //        //    backupJobName,
        //        //    sourceFilePath,
        //        //    targetFilePath,
        //        //    sourceFileInfo.Length,
        //        //    transferTime,
        //        //    DateTime.Now.ToString()
        //        //));

        //    }
        //}

        //private static bool ShouldCopyFile(string sourceFilePath, string targetFilePath)
        //{
        //    bool shouldCopy = false;

        //    // Check if the file already exists in the target directory
        //    if (!File.Exists(targetFilePath))
        //    {
        //        return true;
        //    }



        //    return shouldCopy;
        //}

        //private static string CalculateMD5(string filePath)
        //{
        //    using (var md5 = MD5.Create())
        //    {
        //        using (var stream = File.OpenRead(filePath))
        //        {
        //            byte[] hashBytes = md5.ComputeHash(stream);
        //            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        //        }
        //    }
        //}
    }

}
