using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace EasySave.Views
{
    /// <summary>
    /// Logique d'interaction pour BackupJobCreationView.xaml
    /// </summary>
    public partial class BackupJobCreationView : UserControl
    {
        public BackupJobCreationView()
        {
            InitializeComponent();
        }

        private void Button_Click_Source_Dir(object sender, RoutedEventArgs e)
        {
            OpenFolder(SourceDirButton);   
        }

        private void Button_Click_Destination_Dir(object sender, RoutedEventArgs e)
        {
            OpenFolder(DestinationDirButton);
        }

        private void OpenFolder( TextBox valueBox)
        {
            OpenFolderDialog openfolderDialog = new OpenFolderDialog();
            if (openfolderDialog.ShowDialog() == true)
            {
                string filePath = openfolderDialog.FolderName;
                valueBox.Text = filePath;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                // Mettez à jour le texte de la TextBox avec le contenu de l'élément de menu sélectionné
                TextBoxTypeBackup.Text = menuItem.Header.ToString();
            }
        }
    }
}
