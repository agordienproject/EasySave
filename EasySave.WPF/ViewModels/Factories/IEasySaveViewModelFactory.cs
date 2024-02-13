using EasySave.WPF.State.Navigators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.WPF.ViewModels.Factories
{
    public interface IEasySaveViewModelFactory
    {
        ViewModelBase CreateViewModel(ViewType viewType);
    }
}
