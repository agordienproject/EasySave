using EasySave.State.Navigators;

namespace EasySave.ViewModels.Factories
{
    public interface IEasySaveViewModelFactory
    {
        ViewModelBase CreateViewModel(ViewType viewType);
    }
}
