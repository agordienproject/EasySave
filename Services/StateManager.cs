using EasySave.Models;
using EasySave.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EasySave.Services
{
    public class StateManager
    {
        private readonly IConfiguration _configuration;
        private readonly IJsonFileManager _jsonFileManager;

        private List<State>? _states;

        public StateManager(IConfiguration configuration)
        {
            _configuration = configuration;
            _jsonFileManager = new JsonFileManager(GetStateFilePath());
        }

        public void ReadStates()
        {
            _states = _jsonFileManager.Read<State>();
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

        public void UpdateState(State updatedState)
        {
            ReadStates();

            int stateToUpdateIndex = _states.FindIndex(state => state.BackupName == updatedState.BackupName);

            if (stateToUpdateIndex != -1)
            {
                _states[stateToUpdateIndex] = updatedState;
            }

            WriteStates();
        }

        private string GetStateFilePath()
        {
            string filePath = _configuration.GetValue<string>("StatesJsonPath");

            if (!File.Exists(filePath))
            {
                using (FileStream fs = File.Create(filePath))
                {
                }
            }

            return filePath;
        }
    }
}
