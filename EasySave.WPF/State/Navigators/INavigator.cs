using EasySave.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace EasySave.WPF.State.Navigators
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
