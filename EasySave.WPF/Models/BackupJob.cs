using System.Diagnostics;
using System.Security.Cryptography;
using EasySave.Enums;
using System.IO;
using EasySave.Services.Interfaces;
using EasySave.Services;
using EasySave.Services.Factories;

namespace EasySave.Models
{
    public class BackupJob : BackupJobInfo, INamedEntity
    {
        private readonly IBackupJobService _backupJobService;
        private readonly ILogService _logService;

        public BackupJob(IBackupJobService backupJobService, ILogService logService, BackupJobInfo backupJobInfo)
        {
            BackupName = backupJobInfo.BackupName;
            SourceDirectory = backupJobInfo.SourceDirectory;
            TargetDirectory = backupJobInfo.TargetDirectory;
            BackupType = backupJobInfo.BackupType;
            BackupState = backupJobInfo.BackupState;
            BackupTime = backupJobInfo.BackupTime;
            TotalFilesNumber = backupJobInfo.TotalFilesNumber;
            TotalFilesSize = backupJobInfo.TotalFilesSize;
            NbFilesLeftToDo = backupJobInfo.NbFilesLeftToDo;
            FilesSizeLeftToDo = backupJobInfo.FilesSizeLeftToDo;
            SourceTransferingFilePath = backupJobInfo.SourceTransferingFilePath;
            TargetTransferingFilePath = backupJobInfo.TargetTransferingFilePath;

            _backupJobService = backupJobService;
            _logService = logService;
         
        }

        public async Task Execute()
        {
            PropertyChanged += (sender, e) => _backupJobService.Update(this);

            await InitState();

            await GetDirectoryInfos(SourceDirectory);

            NbFilesLeftToDo = TotalFilesNumber;
            FilesSizeLeftToDo = TotalFilesSize;

            await CopyFiles(BackupName, SourceDirectory, TargetDirectory, BackupType);

            await ClearState();

            PropertyChanged -= (sender, e) => _backupJobService.Update(this);
        }

        private async Task InitState()
        {
            BackupState = BackupState.Active;
        }

        private async Task ClearState()
        {
            BackupState = BackupState.Inactive;
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
                    List<string> authorizedExtensions = Properties.Settings.Default.AuthorizedExtensions.Cast<string>().ToList();

                    if (authorizedExtensions.Contains(fileExtension))
                    {
                        string cryptoSoftPath = Path.Combine("..", "..", "..", "..", "CryptoSoft", "bin", "Debug", "net8.0", "CryptoSoft.exe");
                        //string cryptoSoftPath = "C:\\Users\\Alexis\\Documents\\#CESI\\2023-2024\\PROJETS\\BLOC-PROXYS\\LIVRABLE-II\\V2.0\\CryptoSoft\\bin\\Debug\\net8.0\\CryptoSoft.exe";
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

                await _logService.Create(new Log(
                    backupJobName,
                    sourceFilePath,
                    targetFilePath,
                    sourceFileInfo.Length,
                    transferTime,
                    DateTime.Now.ToString()
                ));

            }
        }

        private static bool ShouldCopyFile(string sourceFilePath, string targetFilePath)
        {
            bool shouldCopy = false;

            // Check if the file already exists in the target directory
            if (!File.Exists(targetFilePath))
            {
                return true;
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
