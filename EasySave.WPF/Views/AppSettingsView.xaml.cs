using EasySave.Services;
using EasySave.Services.Interfaces;
using EasySave.ViewModels;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace EasySave.Views
{
    public partial class AppSettingsView : UserControl
    {
        public AppSettingsView()
        {
            InitializeComponent();
        }

        private void BrowseStatesFolder_Click(object sender, RoutedEventArgs e)
        {
            string selectedFolderPath = BrowseFolder();
            if (!string.IsNullOrEmpty(selectedFolderPath))
            {
                StatesFolderPathTextBox.Text = selectedFolderPath;
            }
        }

        private void BrowseLogsFolder_Click(object sender, RoutedEventArgs e)
        {
            string selectedFolderPath = BrowseFolder();
            if (!string.IsNullOrEmpty(selectedFolderPath))
            {
                LogsFolderPathTextBox.Text = selectedFolderPath;
            }
        }

        private string BrowseFolder()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "Select folder";
            dialog.Filter = "Folders|*.none";

            if (dialog.ShowDialog() == true)
            {
                return Path.GetDirectoryName(dialog.FileName);
            }

            return null;
        }

        private void RemoveEncryptedFileExtension_Click(object sender, RoutedEventArgs e)
        {
            listboxencryptedextensions.Items.Remove(listboxencryptedextensions.SelectedIndex);

            listboxencryptedextensions.Items.Refresh();
        }
        private void RemovePrioritizedFileExtension_Click(object sender, RoutedEventArgs e)
        {
            listboxprioritizedextensions.Items.Remove(listboxprioritizedextensions.SelectedIndex);

            listboxprioritizedextensions.Items.Refresh();
        }

    }

}
