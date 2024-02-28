using System.Diagnostics;
using System.Security.Cryptography;
using EasySave.Domain.Enums;
using System.IO;
using EasySave.Services.Interfaces;
using EasySave.Services;
using EasySave.Services.Factories;
using System.Threading;
using EasySave.Domain.Models;
using EasySave.WPF.Utils;
using System.Windows;
using System.Collections.Generic;

namespace EasySave.Models
{
    public class BackupJob : BackupJobInfo, INamedEntity
    {
        private readonly IBackupJobService _backupJobService;
        private readonly ILogService _logService;

        public bool IsRunning { get { return BackupState == BackupState.Active; } }
        public bool IsFinished { get { return BackupState == BackupState.Finished; } }
        public bool IsPaused { get { return BackupState == BackupState.Paused; } }

        private readonly object _alreadyExecutinglock = new object();
        private readonly object _maxSizeFileLock = new object();

        private readonly ManualResetEvent _pauseEvent = new ManualResetEvent(true);
        private static bool isMaxSizeFileLockTaken = false;

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
            BackupState = BackupState.Paused;
            if (Barrier.ParticipantCount > 0)
                Barrier.RemoveParticipant();

            _pauseEvent.Reset(); // Resets the event, blocking threads
        }

        public void Resume()
        {
            BackupState = BackupState.Active;
            Barrier.AddParticipant();
            _pauseEvent.Set(); // Sets the event, allowing threads to proceed
        }

        public void Stop()
        {
            TokenSource.Cancel();
        }

        public void Execute()
        {
            lock (_alreadyExecutinglock)
            {
                if (!Directory.Exists(SourceDirectory))
                {
                    return;
                }
                else if (MemoryUsed.IsRamReached == true)
                {
                    MessageBox.Show("Vous ne pouvez pas lancer de backupjob");
                    return;
                }
                TokenSource = new();
                PropertyChanged += (sender, e) => { _backupJobService.Update(this); };

                InitState();

                // Set the _priorityFiles & _nonPriorityFiles lists 
                SetDirectoryInfos();

                // PRIORITAIRES
                Barrier.AddParticipant();
                if (_priorityFiles.Count > 0)
                    CopyFiles(_priorityFiles, this);
                
                Barrier.SignalAndWait();

                // NON PRIORITAIRES
                Barrier.RemoveParticipant();
                if (_nonPriorityFiles.Count > 0)
                    CopyFiles(_nonPriorityFiles, this);

                ClearState();
                TokenSource.Dispose();
                PropertyChanged -= (sender, e) => _backupJobService.Update(this);
            }
        }

        private void InitState()
        {
            BackupState = BackupState.Active;
            BackupTime = DateTime.Now.ToString();
            TotalFilesNumber = 0;
            FilesSizeLeftToDo = (long)0;
            NbFilesLeftToDo = 0;
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
            string[] files = Directory.GetFiles(SourceDirectory, ".", SearchOption.AllDirectories);
            DirectoryInfo directoryInfo = new DirectoryInfo(SourceDirectory);

            TotalFilesNumber = files.Length;
            TotalFilesSize = new DirectoryInfo(SourceDirectory).GetFiles(".", SearchOption.AllDirectories).Sum(file => file.Length);

            foreach (var fichier in files)
            {
                FileInfo fileInfo = new FileInfo(fichier);
                //TotalFilesSize += fileInfo.Length;

                if (Properties.Settings.Default.PrioritizedExtensions.Contains(fileInfo.Extension.TrimStart('.').ToLower()))
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

        private void CopyFiles(List<FileInfo> filesInfo, BackupJobInfo backupJobInfo)
        {
            if (!Directory.Exists(TargetDirectory))
            {
                Directory.CreateDirectory(TargetDirectory);
            }
            List<FileInfo> lastBigFiles= new List<FileInfo>();

            foreach (FileInfo fileInfo in filesInfo)
            {
                if (TokenSource.IsCancellationRequested)
                    return;
                _pauseEvent.WaitOne();

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

                    if (fileInfo.Length >= Properties.Settings.Default.MaxKoToTransfert * 1024)
                    {
                        bool isLocked = isMaxSizeFileLockTaken;

                        if (isLocked)
                        {
                            // Le verrou est déjà pris, ajoutez le fichier à la liste
                            lastBigFiles.Add(fileInfo);
                            continue;
                        }

                        // Essayez d'acquérir le verrou
                        lock (_maxSizeFileLock)
                        {
                            // Mettez à jour l'état du verrou
                            isMaxSizeFileLockTaken = true;

                            // Copiez le fichier
                            if (ShouldBeEncrypted(SourceTransferingFilePath))
                            {
                                CopyFileEncrypted(SourceTransferingFilePath, TargetTransferingFilePath, ref transferTime, ref encryptionTime);
                            }
                            else
                            {
                                CopyFile(SourceTransferingFilePath, TargetTransferingFilePath, ref transferTime);
                            }

                            // Remettre à jour l'état du verrou après avoir terminé
                            isMaxSizeFileLockTaken = false;
                        }
                    }
                    else
                    {
                        if (ShouldBeEncrypted(SourceTransferingFilePath))
                        {
                            CopyFileEncrypted(SourceTransferingFilePath, TargetTransferingFilePath, ref transferTime, ref encryptionTime);
                        }
                        else
                        {
                            CopyFile(SourceTransferingFilePath, TargetTransferingFilePath, ref transferTime);
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
            }

            if (lastBigFiles.Count > 0)
            {
                CopyFiles(lastBigFiles, backupJobInfo);
            }
        }


        private void CopyFile(string sourceFilePath, string targetFilePath, ref double transferTime)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(targetFilePath));

                const int bufferSize = 1024 * 1024 * 20; // 1 MB buffer size
                byte[] buffer = new byte[bufferSize];
                long totalBytesCopied = 0;
                long fileSize = new FileInfo(sourceFilePath).Length;

                using (FileStream sourceStream = File.OpenRead(sourceFilePath))
                using (FileStream targetStream = File.Create(targetFilePath))
                {
                    DateTime startTime = DateTime.Now;

                    int bytesRead;
                    while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        if (TokenSource.IsCancellationRequested)
                            return;

                        _pauseEvent.WaitOne();

                        targetStream.Write(buffer, 0, bytesRead);
                        FilesSizeLeftToDo -= bytesRead;

                        // Report progress

                        // Optionally, you can introduce a small delay to not overwhelm the system
                    }

                    DateTime endTime = DateTime.Now;
                    transferTime = (endTime - startTime).TotalSeconds;
                }
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

                while (!process.WaitForExit(100)) // Attendre 100 millisecondes maximum
                {
                    if (BackupState == BackupState.Paused) // Vérifier si la pause est activée
                    {
                        process.Kill(); // Arrêter le processus
                        _pauseEvent.WaitOne(); // Attendre jusqu'à ce que la pause soit désactivée

                        // Vérifier si la tâche a été annulée
                        if (TokenSource.IsCancellationRequested)
                            return;
                            
                        process.Start();
                        
                    }
                }

                // Le processus est terminé
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
            fileExtension = fileExtension.ToLower();
            List<string> authorizedExtensions = Properties.Settings.Default.EncryptedExtensions.Cast<string>().ToList();

            if (authorizedExtensions.Contains(fileExtension))
                return true;

            return false;
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
