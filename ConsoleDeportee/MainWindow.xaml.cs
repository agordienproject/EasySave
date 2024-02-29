using ConsoleDeportee.Services;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace ConsoleDeportee
{
    public partial class MainWindow : Window
    {
        public MainWindow(object dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Récupération des valeurs des champs Adresse IP et Port
                string ipAddress = txtIPAddress.Text;
                int port = int.Parse(txtPort.Text);

                // Tentative de connexion au serveur avec l'adresse IP et le port spécifiés
                bool isConnected = TCPClientManager.ConnectToServer(ipAddress, port);

                // Vérification de la réussite de la connexion
                if (isConnected)
                {
                    // La connexion a réussi, faire quelque chose si nécessaire
                }
                else
                {
                    // La connexion a échoué, afficher un message à l'utilisateur
                    MessageBox.Show("La connexion au serveur a échoué. Veuillez vérifier l'adresse IP et réessayer.", "Erreur de connexion", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Le port doit être un nombre entier.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite lors de la connexion au serveur : " + ex.Message);
            }
        }
    }
}
