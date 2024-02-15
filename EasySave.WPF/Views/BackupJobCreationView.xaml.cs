using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
using Microsoft.Win32;



namespace EasySave.WPF.Views
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
    }
}
