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

        public async Task ReadStates()
        {
            try
            {

            _states = await _jsonFileManager.Read<State>();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        public async Task WriteStates()
        {
            await _jsonFileManager.Write(_states);
        }

        public async Task CreateState(State state)
        {
            await ReadStates();

            _states.Add(state);

            await WriteStates();
        }

        public async Task DeleteState(string backupJobName)
        {
            await ReadStates();

            State stateToDelete = _states.FirstOrDefault(x => x.BackupName == backupJobName);
            _states.Remove(stateToDelete);

            await WriteStates();
        }

        public async Task UpdateState
            (
            string BackupName,
            BackupState BackupState = BackupState.None,
            int TotalFilesNumber = 0,
            long TotalFilesSize = 0,
            int NbFilesLeftToDo = 0,
            long FilesSizeLeftToDo = 0,
            string SourceTransferingFilePath = "",
            string TargetTransferingFilePath = ""
            )

        {
            await ReadStates();

            int stateToUpdateIndex = _states.FindIndex(state => state.BackupName == BackupName);

            State stateToUpdate = _states[stateToUpdateIndex];

            stateToUpdate.BackupTime = DateTime.Now.ToString();

            if (stateToUpdateIndex != -1)
            {
                stateToUpdate = _states[stateToUpdateIndex];
            }

            if (BackupState != Enums.BackupState.None)
            {
                stateToUpdate.BackupState = BackupState;
            }

            if (TotalFilesNumber != 0)
            {
                stateToUpdate.TotalFilesNumber = TotalFilesNumber;
            }

            if (TotalFilesSize != 0)
            {
                stateToUpdate.TotalFilesSize = TotalFilesSize;
            }

            if (NbFilesLeftToDo != 0)
            {
                stateToUpdate.NbFilesLeftToDo = NbFilesLeftToDo;
            }

            if (FilesSizeLeftToDo != 0)
            {
                stateToUpdate.FilesSizeLeftToDo = FilesSizeLeftToDo;
            }

            if (SourceTransferingFilePath != "")
            {
                stateToUpdate.SourceTransferingFilePath = SourceTransferingFilePath;
            }

            if (TargetTransferingFilePath != "")
            {
                stateToUpdate.TargetTransferingFilePath = TargetTransferingFilePath;
            }

            _states[stateToUpdateIndex] = stateToUpdate;

            await WriteStates();
        }

    }
}
