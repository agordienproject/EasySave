using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasySave.Domain.Enums;
using EasySave.Services.Interfaces;
using EasySave.Domain.Models;
using System.Diagnostics;

namespace EasySave.Models
{
    public class BackupJob : BackupJobInfo, INamedEntity
    {
        private readonly IBackupJobService _backupJobService;
        private readonly ILogService _logService;
        private readonly object _pauseLock = new object();

        public bool IsRunning { get { return BackupState == BackupState.Active; } }
        public bool IsFinished { get { return BackupState == BackupState.Finished; } }
        public bool IsPaused { get { return BackupState == BackupState.Paused; } }

        private readonly object _alreadyExecutinglock = new object();
        private readonly object _maxSizeFileLock = new object();

        private readonly ManualResetEvent _pauseEvent = new ManualResetEvent(true);

        public CancellationTokenSource TokenSource;

        private List<FileInfo> _priorityFiles = new();
        private List<FileInfo> _nonPriorityFiles = new();

        public static Barrier Barrier = new(0);

        public BackupJob(IBackupJobService backupJobService, ILogService logService, BackupJobInfo backupJobInfo)
        {
            BackupJobId = backupJobInfo.BackupJobId;
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

            PropertyChanged += (sender, e) => _backupJobService.Update(this);
            ClearState();
            PropertyChanged -= (sender, e) => _backupJobService.Update(this);
        }

        public void Pause()
        {
            lock (_pauseLock)
            {
                if (!IsPaused)
                {
                    BackupState = BackupState.Paused;
                    _pauseEvent.Reset(); // Mettre en pause la copie en bloquant les threads
                }
            }
        }

        public void Resume()
        {
            lock (_pauseLock)
            {
                if (IsPaused)
                {
                    BackupState = BackupState.Active;
                    _pauseEvent.Set(); // Reprendre la copie en autorisant les threads à continuer
                }
            }
        }

        public void Stop()
        {
            TokenSource.Cancel();
        }

        public async Task ExecuteAsync()
        {
            lock (_alreadyExecutinglock)
            {
                if (!Directory.Exists(SourceDirectory))
                {
                    return;
                }

                TokenSource = new();
                Barrier.AddParticipant();
                PropertyChanged += (sender, e) => { _backupJobService.Update(this); };

                InitState();

                // Set the _priorityFiles & _nonPriorityFiles lists 
                SetDirectoryInfos();

                // PRIORITAIRES
                await CopyFilesAsync(_priorityFiles);

                Barrier.SignalAndWait();

                // NON PRIORITAIRES
                await CopyFilesAsync(_nonPriorityFiles);

                ClearState();

                TokenSource.Dispose();
                Barrier.RemoveParticipant();
                PropertyChanged -= (sender, e) => _backupJobService.Update(this);
            }
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

            _priorityFiles.Clear();
            _nonPriorityFiles.Clear();
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

            _priorityFiles.Clear();
            _nonPriorityFiles.Clear();
        }

        private void SetDirectoryInfos()
        {
            string[] files = Directory.GetFiles(SourceDirectory, "*.*", SearchOption.AllDirectories);

            TotalFilesNumber = files.Length;

            foreach (var fichier in files)
            {
                FileInfo fileInfo = new FileInfo(fichier);
                TotalFilesSize += fileInfo.Length;

                if (Properties.Settings.Default.PrioritizedExtensions.Contains(fileInfo.Extension.TrimStart('.')))
                {
                    _priorityFiles.Add(fileInfo);
                }
                else
                {
                    _nonPriorityFiles.Add(fileInfo);
                }
            }

            NbFilesLeftToDo = TotalFilesNumber;
            FilesSizeLeftToDo = TotalFilesSize;

        }

        private async Task CopyFilesAsync(List<FileInfo> filesInfo)
        {
            if (!Directory.Exists(TargetDirectory))
            {
                Directory.CreateDirectory(TargetDirectory);
            }

            foreach (FileInfo fileInfo in filesInfo)
            {
                if (TokenSource.IsCancellationRequested)
                    return;

                lock (_pauseLock)
                {
                    _pauseEvent.WaitOne(); // Attendre si la copie est en pause
                }

                SourceTransferingFilePath = fileInfo.FullName;

                string targetPath = TargetDirectory + SourceTransferingFilePath.Substring(SourceDirectory.Length);
                TargetTransferingFilePath = targetPath;

                // Differential
                bool shouldCopy = true;
                if (BackupType == BackupType.Differential)
                    shouldCopy = ShouldBeCopied(SourceTransferingFilePath, TargetTransferingFilePath);

                if (shouldCopy)
                {
                    double transferTime = 0;
                    double encryptionTime = 0;

                    if (fileInfo.Length >= Properties.Settings.Default.MaxKoToTransfert)
                    {
                        lock (_maxSizeFileLock)
                        {
                            if (ShouldBeEncrypted(SourceTransferingFilePath))
                            {
                                await CopyFileEncryptedAsync(SourceTransferingFilePath, TargetTransferingFilePath, transferTime, encryptionTime);
                            }
                            else
                            {
                                await CopyFileAsync(SourceTransferingFilePath, TargetTransferingFilePath, transferTime);
                            }
                        }
                    }
                    else
                    {
                        if (ShouldBeEncrypted(SourceTransferingFilePath))
                        {
                            await CopyFileEncryptedAsync(SourceTransferingFilePath, TargetTransferingFilePath, transferTime, encryptionTime);
                        }
                        else
                        {
                            await CopyFileAsync(SourceTransferingFilePath, TargetTransferingFilePath, transferTime);
                        }
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

        private async Task CopyFileAsync(string sourceFilePath, string targetFilePath, double transferTime)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(targetFilePath));

                DateTime before = DateTime.Now;
                using (var sourceStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read))
                using (var targetStream = new FileStream(targetFilePath, FileMode.Create, FileAccess.Write))
                {
                    await sourceStream.CopyToAsync(targetStream, 81920);
                }
                DateTime after = DateTime.Now;
                transferTime = (after - before).TotalSeconds;
            }
            catch (Exception)
            {
                transferTime = -1;
            }
        }

        private async Task CopyFileEncryptedAsync(string sourceFilePath, string targetFilePath, double transferTime, double encryptionTime)
        {
            try
            {
                DateTime before = DateTime.Now;

                var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
                var appRoot = Path.GetDirectoryName(location);
                string cryptoSoftPath = Path.Combine(appRoot, "CryptoSoft", "CryptoSoft.exe");
                string cryptoSoftArg = $"-s \"{sourceFilePath}\" -d \"{targetFilePath}\"";

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

            return sourceFile.LastWriteTime > targetFile.LastWriteTime;
        }

        private static bool ShouldBeEncrypted(string filePath)
        {
            string fileExtension = Path.GetExtension(filePath);
            fileExtension = fileExtension.TrimStart('.'); // Enlève le "." au début de l'extension

            return Properties.Settings.Default.EncryptedExtensions.Cast<string>().ToList().Contains(fileExtension);
        }

        public void UpdateBackupJobInfos(BackupJobInfo backupJobInfo)
        {
            BackupJobId = backupJobInfo.BackupJobId;
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
        }
    }
}
