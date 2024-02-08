using EasySave.Enums;
using EasySave.Models;

namespace EasySave.Services.Interfaces
{
    public interface IStateManager
    {
        Task CreateState(State state);
        Task DeleteState(string backupJobName);
        Task ReadStates();
        Task UpdateState(string BackupName, BackupState BackupState = BackupState.None, int TotalFilesNumber = 0, long TotalFilesSize = 0, int NbFilesLeftToDo = 0, long FilesSizeLeftToDo = 0, string SourceTransferingFilePath = "", string TargetTransferingFilePath = "");
        Task WriteStates();
    }
}