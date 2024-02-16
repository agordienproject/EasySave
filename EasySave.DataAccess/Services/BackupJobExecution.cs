using EasySave.Domain.Enums;
using EasySave.Domain.Models;
using EasySave.Domain.Services;
using EasySave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Diagnostics; // Ajoutez cette ligne


namespace EasySave.DataAccess.Services
{
    public class BackupJobExecution
    {
        private readonly IStateService _stateService;
        private readonly ILogService _logService;
        private readonly IConfiguration _configuration;

        private BackupJob _backupJob {  get; set; }

        private State _backupState { get; set; }


        public BackupJobExecution(BackupJob backupJob,IStateService stateService, ILogService logService, IConfiguration configuration) 
        {
            _stateService = stateService;
            _logService = logService;
            _configuration = configuration;
            _backupJob = backupJob;
            _backupState = new State(backupJob.BackupName);
        }

        public async Task UpdateState(State state)
        {
            await _stateService.Update(state);
        }

        public async Task Execute()
        {
            await InitState();
            
            await GetDirectoryInfos(_backupJob.SourceDirectory);

            _backupState.NbFilesLeftToDo = _backupState.TotalFilesNumber;
            _backupState.FilesSizeLeftToDo = _backupState.TotalFilesSize;
            await UpdateState(_backupState);

            await CopyFiles(_backupJob.BackupName, _backupJob.SourceDirectory, _backupJob.TargetDirectory, _backupJob.BackupType);

            await ClearState();
        }

        private async Task InitState()
        {
            _backupState.BackupState = Domain.Enums.BackupState.Active;
            await UpdateState(_backupState);
        }

        private async Task ClearState()
        {
            _backupState.BackupState = Domain.Enums.BackupState.Inactive;
            _backupState.BackupTime = DateTime.Now.ToString();
            _backupState.TotalFilesNumber = 0;
            _backupState.TotalFilesSize = (long)0;
            _backupState.NbFilesLeftToDo = 0;
            _backupState.FilesSizeLeftToDo = (long)0;
            _backupState.SourceTransferingFilePath = "";
            _backupState.TargetTransferingFilePath = "";
            await UpdateState(_backupState);
        }

        private async Task GetDirectoryInfos(string directory)
        {
            try
            {
                string[] files = Directory.GetFiles(directory);
                string[] subDirs = Directory.GetDirectories(directory);

                foreach (var fichier in files)
                {
                    _backupState.TotalFilesNumber++;
                    _backupState.TotalFilesSize += new FileInfo(fichier).Length;
                    await UpdateState(_backupState);
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

                _backupState.SourceTransferingFilePath = filePath;
                _backupState.TargetTransferingFilePath = targetPath;
                await UpdateState(_backupState);

                await CopyFile(backupJobName, filePath, targetDir, backupType);

                _backupState.NbFilesLeftToDo--;
                _backupState.FilesSizeLeftToDo -= fileInfo.Length;
                await UpdateState(_backupState);
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
