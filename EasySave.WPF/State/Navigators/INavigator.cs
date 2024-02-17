using EasySave.ViewModels;

namespace EasySave.State.Navigators
{
    public enum ViewType
    {
        BackupJobs,
        AppSettings
    }

    public interface INavigator
    {
        ViewModelBase CurrentViewModel { get; set; }
        event Action StateChanged;
    }
}
