using System.Windows;
using System.Windows.Controls;

namespace EasySave.Controls
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
