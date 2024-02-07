using EasySave.Models;
using EasySave.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EasySave.Services
{
    public class StateManager
    {
        private readonly IConfiguration _configuration;
        private readonly IFileManager _jsonFileManager;

        private List<State>? _states;

        public StateManager(IConfiguration configuration)
        {
            _configuration = configuration;
            _jsonFileManager = new JsonFileManager(GetStateFilePath());
        }

        public async Task ReadStates()
        {
            _states = await _jsonFileManager.Read<State>();
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

        public async Task UpdateState(State updatedState)
        {
            await ReadStates();

            int stateToUpdateIndex = _states.FindIndex(state => state.BackupName == updatedState.BackupName);

            if (stateToUpdateIndex != -1)
            {
                _states[stateToUpdateIndex] = updatedState;
            }

            await WriteStates();
        }

        private string GetStateFilePath()
        {
            string folderPath = @".\Data\State\";
            string filePath = @".\Data\State\states.json";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

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
