using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConsoleDeportee.Controls
{
    /// <summary>
    /// Logique d'interaction pour BackupJobsDataGrid.xaml
    /// </summary>
    public partial class BackupJobsDataGrid : UserControl
    {
        public BackupJobsDataGrid()
        {
            InitializeComponent();
        }

        private void MessageBoxDeleteBackup(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("Backupjob is deleted", "Message");

        }

        private void MessageBoxLaunchBackup(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("Backupjob is launched", "Message");

        }
    }
}
