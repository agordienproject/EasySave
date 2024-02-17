using EasySave.Commands;
using EasySave.State.Navigators;
using EasySave.ViewModels.Factories;
using System.Windows.Input;

namespace EasySave.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IEasySaveViewModelFactory _viewModelFactory;
        private readonly INavigator _navigator;

        public ViewModelBase CurrentViewModel => _navigator.CurrentViewModel;

        public ICommand UpdateCurrentViewModelCommand { get; }

        public MainViewModel(IEasySaveViewModelFactory viewModelFactory, INavigator navigator)
        {
            _viewModelFactory = viewModelFactory;
            _navigator = navigator;

            _navigator.StateChanged += Navigator_StateChanged;

            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(_viewModelFactory, navigator);
            UpdateCurrentViewModelCommand.Execute(ViewType.BackupJobs);
        }

        private void Navigator_StateChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        public override void Dispose()
        {
            _navigator.StateChanged -= Navigator_StateChanged;

            base.Dispose();
        }
    }
}
