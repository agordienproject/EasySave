using EasySave.Enums;
using EasySave.Models;
using EasySave.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EasySave.Services
{
    public class StateManager : IStateManager
    {
        private readonly IFileManager _jsonFileManager;

        private List<State>? _states;

        public StateManager()
        {
            _jsonFileManager = new JsonFileManager(AppSettingsJson.GetStatesFilePath());
        }

        public void ReadStates()
        {
            try
            {
                _states = _jsonFileManager.Read<State>();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        public void WriteStates()
        {
            _jsonFileManager.Write(_states);
        }

        public void CreateState(State state)
        {
            ReadStates();

            _states.Add(state);

            WriteStates();
        }

        public void DeleteState(string backupJobName)
        {
            ReadStates();

            State stateToDelete = _states.FirstOrDefault(x => x.BackupName == backupJobName);
            _states.Remove(stateToDelete);

            WriteStates();
        }

        public void UpdateState
        (
            string backupName,
            BackupState backupState = BackupState.None,
            int? totalFilesNumber = null,
            long? totalFilesSize = null,
            int? nbFilesLeftToDo = null,
            long? filesSizeLeftToDo = null,
            string sourceTransferingFilePath = "none",
            string targetTransferingFilePath = "none"
        )
        {
            ReadStates();

            int stateToUpdateIndex = _states.FindIndex(state => state.BackupName == backupName);

            if (stateToUpdateIndex == -1)
            {
                return;
            }

            State stateToUpdate = _states[stateToUpdateIndex];
            stateToUpdate.BackupTime = DateTime.Now.ToString();

            if (backupState != Enums.BackupState.None)
            {
                stateToUpdate.BackupState = backupState;
            }

            if (totalFilesNumber != null)
            {
                stateToUpdate.TotalFilesNumber = (int)totalFilesNumber;
            }

            if (totalFilesSize != null)
            {
                stateToUpdate.TotalFilesSize = (long)totalFilesSize;
            }

            if (nbFilesLeftToDo != null)
            {
                stateToUpdate.NbFilesLeftToDo = (int)nbFilesLeftToDo;
            }

            if (filesSizeLeftToDo != null)
            {
                stateToUpdate.FilesSizeLeftToDo = (long)filesSizeLeftToDo;
            }

            if (sourceTransferingFilePath != "none")
            {
                stateToUpdate.SourceTransferingFilePath = sourceTransferingFilePath;
            }

            if (targetTransferingFilePath != "none")
            {
                stateToUpdate.TargetTransferingFilePath = targetTransferingFilePath;
            }

            _states[stateToUpdateIndex] = stateToUpdate;

            WriteStates();
        }

    }
}
