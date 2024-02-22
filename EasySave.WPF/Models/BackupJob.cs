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

        public void Execute()
        {
            if (!Directory.Exists(SourceDirectory))
            {
                return;
            }

            PropertyChanged += (sender, e) => {_backupJobService.Update(this);};

            InitState();

            List<FileInfo> files = GetDirectoryInfos();

            CopyFiles(files);

            ClearState();

            PropertyChanged -= (sender, e) => _backupJobService.Update(this);
        }

        private void InitState()
        {
            BackupState = BackupState.Active;
            BackupTime = DateTime.Now.ToString();
            TotalFilesNumber = 0;
            TotalFilesSize = (long)0;
            NbFilesLeftToDo = 0;
            FilesSizeLeftToDo = (long)0;
            SourceTransferingFilePath = "";
            TargetTransferingFilePath = "";
        }

        private void ClearState()
        {
            BackupState = BackupState.Finished;
            BackupTime = DateTime.Now.ToString();
            TotalFilesNumber = 0;
            TotalFilesSize = (long)0;
            NbFilesLeftToDo = 0;
            FilesSizeLeftToDo = (long)0;
            SourceTransferingFilePath = "";
            TargetTransferingFilePath = "";
        }

        private List<FileInfo> GetDirectoryInfos()
        {
            List<FileInfo> filesInfo = new List<FileInfo>();
            string[] files = Directory.GetFiles(SourceDirectory, "*.*", SearchOption.AllDirectories);

            TotalFilesNumber = files.Length;

            foreach (var fichier in files)
            {
                FileInfo fileInfo = new FileInfo(fichier);
                TotalFilesSize += fileInfo.Length;
                filesInfo.Add(fileInfo);
            }

            NbFilesLeftToDo = TotalFilesNumber;
            FilesSizeLeftToDo = TotalFilesSize;

            return filesInfo;
        }

        private void CopyFiles(List<FileInfo> filesInfo)
        {
            if (!Directory.Exists(TargetDirectory))
            {
                Directory.CreateDirectory(TargetDirectory);
            }

            foreach (FileInfo fileInfo in filesInfo)
            {
                SourceTransferingFilePath = fileInfo.FullName;

                string targetPath = TargetDirectory + SourceTransferingFilePath.Substring(SourceDirectory.Length);
                TargetTransferingFilePath = targetPath;

                if (ShouldBeCopied(SourceTransferingFilePath, TargetTransferingFilePath))
                {
                    double transferTime = 0;
                    double encryptionTime = 0;

                    if (ShouldBeEncrypted(SourceTransferingFilePath))
                    {
                        CopyFileEncrypted(SourceTransferingFilePath, TargetTransferingFilePath, ref transferTime, ref encryptionTime);
                    } 
                    else
                    {
                        CopyFile(SourceTransferingFilePath, TargetTransferingFilePath, ref transferTime);
                    }

                    _logService.Create(new Log(
                        BackupName,
                        SourceTransferingFilePath,
                        TargetTransferingFilePath,
                        fileInfo.Length,
                        transferTime,
                        encryptionTime,
                        DateTime.Now.ToString()
                    ));
                }

                NbFilesLeftToDo--;
                FilesSizeLeftToDo -= fileInfo.Length;
            }

        }

        private void CopyFile(string SourceTransferingFilePath, string TargetTransferingFilePath, ref double transferTime)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(TargetTransferingFilePath));

                DateTime before = DateTime.Now;
                File.Copy(SourceTransferingFilePath, TargetTransferingFilePath, true);
                DateTime after = DateTime.Now;
                transferTime = (after - before).TotalSeconds;
            }
            catch (Exception)
            {
                transferTime = -1;
            }
        }

        private void CopyFileEncrypted(string SourceTransferingFilePath, string TargetTransferingFilePath, ref double transferTime, ref double encryptionTime)
        {
            try
            {
                DateTime before = DateTime.Now;

                var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
                var appRoot = Path.GetDirectoryName(location);
                string cryptoSoftPath = Path.Combine(appRoot, "CryptoSoft", "CryptoSoft.exe");
                string cryptoSoftArg = $"-s \"{SourceTransferingFilePath}\" -d \"{TargetTransferingFilePath}\"";

                Process process = new Process();
                process.StartInfo.FileName = cryptoSoftPath;
                process.StartInfo.Arguments = cryptoSoftArg;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.EnableRaisingEvents = true;

                process.Start();

                process.WaitForExit();

                double exitCode = process.ExitCode;

                if (exitCode >= 0)
                {
                    encryptionTime = (exitCode / 1000);
                }
                else if (exitCode == -1)
                {
                    encryptionTime = exitCode;
                }

                DateTime after = DateTime.Now;
                transferTime = (after - before).TotalSeconds;
            }
            catch (Exception)
            {
                transferTime = -1;
            }
        }

        private static bool ShouldBeCopied(string sourceFilePath, string targetFilePath)
        {
            if (!File.Exists(targetFilePath))
            {
                return true;
            }

            FileInfo sourceFile = new FileInfo(sourceFilePath);
            FileInfo targetFile = new FileInfo(targetFilePath);

            if (sourceFile.LastWriteTime > targetFile.LastWriteTime)
                return true;

            return false;
        }

        private static bool ShouldBeEncrypted(string filePath)
        {
            string fileExtension = Path.GetExtension(filePath);
            fileExtension = fileExtension.TrimStart('.'); // Enlève le "." au début de l'extension

            List<string> authorizedExtensions = Properties.Settings.Default.EncryptedExtensions.Cast<string>().ToList();

            if (authorizedExtensions.Contains(fileExtension))
                return true;

            return false;
        }
    }
}
