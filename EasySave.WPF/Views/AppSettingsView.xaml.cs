using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Text.Json;
using EasySave.Domain.Models; // Ajoutez cette ligne pour inclure l'espace de noms contenant ConfigData
using System.Globalization;
using System.Windows.Data;
using EasySave.WPF.ViewModels;
using EasySave.DataAccess.Services;
using EasySave.Domain.Services;
using Microsoft.Extensions.Configuration;

namespace EasySave.WPF.Views
{
    public partial class AppSettingsView : UserControl
    {
        private AppSettingsViewModel _viewModel;
        private Dictionary<string, Action<string>> RadioButtonActions;

        public AppSettingsView()
        {
            InitializeComponent();
            IAppSettingsService appSettingsService = new AppSettingsService(); // Remplacez AppSettingsService par la classe réelle
            _viewModel = new AppSettingsViewModel(appSettingsService);

            InitializeRadioButtonActions();
        }

        private void InitializeRadioButtonActions()
        {
            RadioButtonActions = new Dictionary<string, Action<string>>
            {
                { "French", newValue => _viewModel.AppSettings.Localization.CurrentCulture = newValue },
                { "English", newValue => _viewModel.AppSettings.Localization.CurrentCulture = newValue },
                { "BackupJobsFileType-json", newValue => _viewModel.AppSettings.DataFilesTypes.BackupJobsFileType = newValue },
                { "BackupJobsFileType-xml", newValue => _viewModel.AppSettings.DataFilesTypes.BackupJobsFileType = newValue },
                { "StatesFileType-json", newValue => _viewModel.AppSettings.DataFilesTypes.StatesFileType = newValue },
                { "StatesFileType-xml", newValue => _viewModel.AppSettings.DataFilesTypes.StatesFileType = newValue },
                { "LogsFileType-json", newValue => _viewModel.AppSettings.DataFilesTypes.LogsFileType = newValue },
                { "LogsFileType-xml", newValue => _viewModel.AppSettings.DataFilesTypes.LogsFileType = newValue }
            };
        }


        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            HandleRadioButtonClick(sender, RadioButtonActions);
        }

        private void HandleRadioButtonClick(object sender, Dictionary<string, Action<string>> actions)
        {
            var radioButton = sender as RadioButton;
            if (radioButton != null && radioButton.IsChecked == true)
            {
                string content = radioButton.Content.ToString();
                if (actions.TryGetValue(content, out Action<string> action))
                {
                    action(content);
                }
            }
        }
        private void SaveAppSettings_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SaveAppSettingsCommand.Execute(null);

        }

        private void BrowseBackupJobsFolder_Click(object sender, RoutedEventArgs e)
        {
            string selectedFolderPath = BrowseFolder();
            if (!string.IsNullOrEmpty(selectedFolderPath))
            {
                BackupJobsFolderPathTextBox.Text = selectedFolderPath;
            }
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

    }

    public class LanguageToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            string targetCulture = parameter.ToString();
            string currentCulture = value.ToString();

            return currentCulture.Equals(targetCulture, StringComparison.OrdinalIgnoreCase);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked && isChecked)
            {
                return parameter; // Retourner la culture associée au bouton radio
            }

            // Retourner Binding.DoNothing si le bouton n'est pas sélectionné
            return Binding.DoNothing;
        }
    }


}
