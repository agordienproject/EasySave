using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Text.Json;
using EasySave.Domain.Models; // Ajoutez cette ligne pour inclure l'espace de noms contenant ConfigData

namespace EasySave.WPF.Views
{
    public partial class AppSettingsView
    {
        private AppSettings configData;

        public AppSettingsView()
        {
            InitializeComponent();
            LoadConfig();
            //DataContext = configData;
        }

        private void LoadConfig()
        {
            try
            {
                string jsonFilePath = "appsettings.json";
                string json = File.ReadAllText(jsonFilePath);
                var config = JsonSerializer.Deserialize<AppSettings>(json);

                // Mettre à jour les valeurs dans les contrôles de l'interface utilisateur
                BackupJobsFolderPathTextBox.Text = config.DataFilesLocation.BackupJobsFolderPath;
                BackupJobsJsonFileNameTextBox.Text = config.DataFilesLocation.BackupJobsJsonFileName;
                StatesFolderPathTextBox.Text = config.DataFilesLocation.StatesFolderPath;
                StatesJsonFileNameTextBox.Text = config.DataFilesLocation.StatesJsonFileName;
                LogsFolderPathTextBox.Text = config.DataFilesLocation.LogsFolderPath;

                // Vérifier la valeur de la langue actuelle et cocher la case correspondante
                if (config.Localization.CurrentCulture == "en-EN")
                {
                    EnglishCheckBox.IsChecked = true;
                }
                else if (config.Localization.CurrentCulture == "fr-FR")
                {
                    FrenchCheckBox.IsChecked = true;
                }

                // Affecter la configuration chargée à configData
                configData = config;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite lors du chargement du fichier de configuration : " + ex.Message);
                //configData = new AppSettings();
            }
        }

        private void LanguageCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Décocher l'autre case
            if (sender == EnglishCheckBox)
            {
                FrenchCheckBox.IsChecked = false;
            }
            else if (sender == FrenchCheckBox)
            {
                EnglishCheckBox.IsChecked = false;
            }
        }

        private void SaveConfig()
        {
            try
            {
                string jsonFilePath = "appsettings.json"; // Chemin vers votre fichier JSON
                string json = JsonSerializer.Serialize(configData, Formatting.Indented);
                File.WriteAllText(jsonFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite lors de la sauvegarde du fichier de configuration : " + ex.Message);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (EnglishCheckBox.IsChecked == true)
            {
                configData.Localization.CurrentCulture = "en-EN";
            }
            else if (FrenchCheckBox.IsChecked == true)
            {
                configData.Localization.CurrentCulture = "fr-FR";
            }

            // Mettre à jour les valeurs dans configData avec celles des contrôles de l'interface utilisateur
            configData.DataFilesLocation.BackupJobsFolderPath = BackupJobsFolderPathTextBox.Text;
            configData.DataFilesLocation.BackupJobsJsonFileName = BackupJobsJsonFileNameTextBox.Text;
            configData.DataFilesLocation.StatesFolderPath = StatesFolderPathTextBox.Text;
            configData.DataFilesLocation.StatesJsonFileName = StatesJsonFileNameTextBox.Text;
            configData.DataFilesLocation.LogsFolderPath = LogsFolderPathTextBox.Text;

            // Sauvegarder la configuration mise à jour dans le fichier JSON
            SaveConfig();
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
}
