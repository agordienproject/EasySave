using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EasySave.Domain.Enums;
using EasySave.Domain.Services;
using Microsoft.Extensions.Configuration;

namespace EasySave.Domain.Models
{
    public class BackupJob : BackupJobInfo, INamedEntity
    {
        private readonly ILogService _logService;
        private readonly IBackupJobService _backupJobService;
        private readonly IConfiguration _configuration;

        public BackupJob() : base()
        {

        }

        public BackupJob(IBackupJobService backupJobService, ILogService logService, IConfiguration configuration) : base()
        {
            _logService = logService;
            _backupJobService = backupJobService;

            PropertyChanged += async (sender, e) => await _backupJobService.Update(this);
            _configuration = configuration;
        }

        public async Task Execute()
        {
            await InitState();

            await GetDirectoryInfos(SourceDirectory);

            NbFilesLeftToDo = TotalFilesNumber;
            FilesSizeLeftToDo = TotalFilesSize;

            await CopyFiles(BackupName, SourceDirectory, TargetDirectory, BackupType);

            await ClearState();
        }

        private async Task InitState()
        {
            BackupState = Domain.Enums.BackupState.Active;
        }

        private async Task ClearState()
        {
            BackupState = Domain.Enums.BackupState.Inactive;
            BackupTime = DateTime.Now.ToString();
            TotalFilesNumber = 0;
            TotalFilesSize = (long)0;
            NbFilesLeftToDo = 0;
            FilesSizeLeftToDo = (long)0;
            SourceTransferingFilePath = "";
            TargetTransferingFilePath = "";
        }

        private async Task GetDirectoryInfos(string directory)
        {
            try
            {
                string[] files = Directory.GetFiles(directory);
                string[] subDirs = Directory.GetDirectories(directory);

                foreach (var fichier in files)
                {
                    TotalFilesNumber++;
                    TotalFilesSize += new FileInfo(fichier).Length;
                }

                foreach (var subDir in subDirs)
                {
                    await GetDirectoryInfos(subDir);
                }
            }
            catch (Exception e)
            {

            }
        }

        private async Task CopyFiles(string backupJobName, string sourceDir, string targetDir, BackupType backupType)
        {
            if (!Directory.Exists(sourceDir))
            {
                return;
            }

            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            foreach (string filePath in Directory.GetFiles(sourceDir))
            {
                FileInfo fileInfo = new FileInfo(filePath);

                string targetPath = Path.Combine(targetDir, fileInfo.Name);

                SourceTransferingFilePath = filePath;
                TargetTransferingFilePath = targetPath;

                await CopyFile(backupJobName, filePath, targetDir, backupType);

                NbFilesLeftToDo--;
                FilesSizeLeftToDo -= fileInfo.Length;
            }

            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                string subDirName = Path.GetFileName(subDir);
                string targetSubDir = Path.Combine(targetDir, subDirName);
                await CopyFiles(backupJobName, subDir, targetSubDir, backupType);
            }
        }

        private async Task CopyFile(string backupJobName, string sourceFilePath, string targetDir, BackupType backupType)
        {
            string sourceFileName = Path.GetFileName(sourceFilePath);
            string targetFilePath = Path.Combine(targetDir, sourceFileName);

            bool shouldCopy = true;

            if (backupType == BackupType.Differential)
            {
                shouldCopy = ShouldCopyFile(sourceFilePath, targetFilePath);
            }

            bool targetFileExist = File.Exists(targetFilePath);

            if ((targetFileExist && shouldCopy) || !targetFileExist)
            {
                FileInfo sourceFileInfo = new(sourceFilePath);

                double transferTime;

                try
                {
                    DateTime before = DateTime.Now;

                    // Récupérer l'extension du fichier actuellement traité
                    string fileExtension = Path.GetExtension(sourceFilePath);
                    fileExtension = fileExtension.TrimStart('.'); // Enlève le "." au début de l'extension

                    // Vérifier si l'extension est présente dans la liste des extensions autorisées
                    IConfigurationSection authorizedExtensionsSection = _configuration.GetSection("FileExtensions:AuthorizedExtensions");
                    List<string> authorizedExtensions = authorizedExtensionsSection.Get<List<string>>();

                    if (authorizedExtensions.Contains(fileExtension))
                    {
                        string old_cryptoSoftPath = Path.Combine("..", "..", "..", "..", "CryptoSoft", "bin", "Debug", "net8.0", "CryptoSoft.exe");
                        string cryptoSoftPath = "C:\\Users\\Alexis\\Documents\\#CESI\\2023-2024\\PROJETS\\BLOC-PROXYS\\LIVRABLE-II\\V2.0\\CryptoSoft\\bin\\Debug\\net8.0\\CryptoSoft.exe";
                        string cryptoSoftArg = String.Concat("-s ", sourceFilePath, " -d ", targetFilePath);
                        Process.Start(cryptoSoftPath, cryptoSoftArg);

                    }
                    else
                    {
                        File.Copy(sourceFilePath, targetFilePath, true);
                    }
                    DateTime after = DateTime.Now;
                    transferTime = after.Subtract(before).TotalSeconds;
                }
                catch (Exception)
                {
                    transferTime = -1;
                }

                //await _logService.Create(new Log(
                //    backupJobName,
                //    sourceFilePath,
                //    targetFilePath,
                //    sourceFileInfo.Length,
                //    transferTime,
                //    DateTime.Now.ToString()
                //));

            }
        }

        private static bool ShouldCopyFile(string sourceFilePath, string targetFilePath)
        {
            bool shouldCopy = false;

            // Check if the file already exists in the target directory
            if (!File.Exists(targetFilePath))
            {
                shouldCopy = true;
                return shouldCopy;
            }

            string sourceFileHash = CalculateMD5(sourceFilePath);
            string destFileHash = CalculateMD5(targetFilePath);

            if (sourceFileHash != destFileHash)
            {
                // The source file is more recent, so we copy it
                shouldCopy = true;
                return shouldCopy;
            }

            return shouldCopy;
        }

        private static string CalculateMD5(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    byte[] hashBytes = md5.ComputeHash(stream);
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}
