namespace EasySave.Models
{
    public class Log : INamedEntity
    {
        public string? BackupName { get; set; }
        public string? SourceFile { get; set;}
        public string? TargetFile { get; set;}
        public long FileSize {get; set;}
        public double FileTransferTime {get; set;}
        public double FileEncryptionTime { get; set;}
        public string? TimeStamp {get; set;}

        public Log()
        {

        }

        public Log(string? backupName, string? sourceFile, string? targetFile, long fileSize, double fileTransferTime, double fileEncryptionTime, string? timeStamp)
        {
            BackupName = backupName;
            SourceFile = sourceFile;
            TargetFile = targetFile;
            FileSize = fileSize;
            FileTransferTime = fileTransferTime;
            FileEncryptionTime = fileEncryptionTime;
            TimeStamp = timeStamp;
        }
    }
}
