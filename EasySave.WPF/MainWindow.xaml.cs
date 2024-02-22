using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace EasySave
{
    public partial class MainWindow : Window
    {
        public MainWindow(object dataContext)
        {
            InitializeComponent();

            DataContext = dataContext;
        }

    }
}
