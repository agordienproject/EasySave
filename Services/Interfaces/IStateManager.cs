using EasySave.Models;

namespace EasySave.Services.Interfaces
{
    public interface IStateManager
    {
        Task CreateState(State state);
        Task DeleteState(string backupJobName);
        Task ReadStates();
        Task UpdateState(State updatedState);
        Task WriteStates();
    }
}