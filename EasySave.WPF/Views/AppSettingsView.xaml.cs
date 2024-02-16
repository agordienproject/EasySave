using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using EasySave.DataAccess.Services;
using EasySave.Domain.Models;
using EasySave.Domain.Services;
using EasySave.WPF.ViewModels;

namespace EasySave.WPF.Views
{
    public partial class AppSettingsView : UserControl
    {
        private AppSettingsViewModel _viewModel;
        private Dictionary<string, Action<string>> RadioButtonActions;
        public List<string> FileExtensions { get; set; } = new List<string>(); // Déclaration de la liste FileExtensions

        public AppSettingsView()
        {
            InitializeComponent();
            IAppSettingsService appSettingsService = new AppSettingsService();
            _viewModel = new AppSettingsViewModel(appSettingsService);

            InitializeRadioButtonActions();
        }

        // Méthodes existantes

        private void AddFileExtension_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer l'extension de fichier entrée par l'utilisateur
            string newExtension = NewFileExtensionTextBox.Text;

            // Vérifier si l'extension n'est pas déjà dans la liste
            if (!_viewModel.AppSettings.FileExtensions.AuthorizedExtensions.Contains(newExtension))
            {
                // Ajouter la nouvelle extension à la liste dans le modèle de données
                _viewModel.AppSettings.FileExtensions.AuthorizedExtensions.Add(newExtension);

                // Effacer le champ de texte après l'ajout
                NewFileExtensionTextBox.Text = string.Empty;

                listboxextensions.ItemsSource = _viewModel.AppSettings.FileExtensions.AuthorizedExtensions;
            }
        }

        private void RemoveExtension_Click(object sender, RoutedEventArgs e)
        {
            // Vérifiez si un élément est sélectionné dans la liste
            if (listboxextensions.SelectedItem != null)
            {
                // Récupérez l'extension sélectionnée
                string selectedExtension = listboxextensions.SelectedItem.ToString();

                // Supprimez l'extension de la liste dans le modèle de données
                _viewModel.AppSettings.FileExtensions.AuthorizedExtensions.Remove(selectedExtension);

                listboxextensions.ItemsSource = _viewModel.AppSettings.FileExtensions.AuthorizedExtensions;

            }
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
