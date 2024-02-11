using EasySave.Enums;
using EasySave.Models;

namespace EasySave.Services.Interfaces
{
    public interface IStateManager
    {
        void CreateState(State state);
        void DeleteState(string backupJobName);
        void ReadStates();
        void UpdateState(string backupName, BackupState backupState = BackupState.None, int? totalFilesNumber = null, long? totalFilesSize = null, int? nbFilesLeftToDo = null, long? filesSizeLeftToDo = null, string sourceTransferingFilePath = "none", string targetTransferingFilePath = "none");
        void WriteStates();
    }
}