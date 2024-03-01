using Microsoft.Extensions.Configuration;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Resources;

namespace CryptoSoft
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var appRoot = Path.GetDirectoryName(location);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(appRoot)
                .AddJsonFile($"appsettings.json", false, true)
                .Build();

            #region OPTIONS
            var sourceFilePathOption = new Option<string>(
                aliases: ["--source", "-s"],
                description: "")
            {
                IsRequired = true
            };

            var destinationFilePathOption = new Option<string>(
                aliases: ["--destination", "-d"],
                description: "")
            {
                IsRequired = true
            };
            #endregion

            #region COMMANDS
            var rootCommand = new RootCommand("")
            {
                sourceFilePathOption,
                destinationFilePathOption
            };

            #endregion

            #region HANDLERS
            rootCommand.SetHandler((sourceFilePath, destinationFilePath) =>
            {
                var key = configuration["cipherKey"];

                DateTime DateBefore = DateTime.Now;
                bool success = XORCipher(sourceFilePath, destinationFilePath, key);
                
                if (!success)
                {
                    Environment.Exit(-1);
                }
                
                DateTime DateAfter = DateTime.Now;

                double transferTime = (DateAfter - DateBefore).TotalSeconds;
                int exitCode = (int)(transferTime * 1000); // Conversion du temps en millisecondes à un entier
                
                Environment.Exit(exitCode);

            }, sourceFilePathOption, destinationFilePathOption);
            #endregion

            return await rootCommand.InvokeAsync(args);
        }

        private static bool XORCipher(string sourceFilePath, string destinationFilePath, string key)
        {
            try
            {
                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);
                byte[] buffer = new byte[1024 * 1024 * 20]; // Taille du buffer (20 Mo)

                long position = 0; // Variable pour stocker la position actuelle dans le fichier de destination

                // Vérifier si le fichier de destination existe déjà
                if (File.Exists(destinationFilePath))
                {
                    // Lire la position actuelle à partir du fichier temporaire s'il existe
                    string tempFilePath = destinationFilePath + ".temp";
                    if (File.Exists(tempFilePath))
                    {
                        using (StreamReader reader = new StreamReader(tempFilePath))
                        {
                            string positionString = reader.ReadLine();
                            if (positionString != null)
                            {
                                position = long.Parse(positionString);
                            }
                        }
                    }
                }

                using (FileStream sourceStream = File.OpenRead(sourceFilePath))
                using (FileStream destinationStream = new FileStream(destinationFilePath, FileMode.Append, FileAccess.Write))
                {
                    sourceStream.Seek(position, SeekOrigin.Begin); // Positionner le curseur de lecture à la dernière position enregistrée
                    int bytesRead;

                    while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        // XOR
                        for (int i = 0; i < bytesRead; i++)
                        {
                            buffer[i] ^= keyBytes[i % keyBytes.Length];
                        }

                        // Écrire dans le fichier de destination
                        destinationStream.Write(buffer, 0, bytesRead);

                        // Enregistrer la nouvelle position dans le fichier temporaire
                        using (StreamWriter writer = new StreamWriter(destinationFilePath + ".temp", false))
                        {
                            writer.WriteLine(destinationStream.Position);
                        }
                    }
                }

                // Supprimer le fichier temporaire une fois le chiffrement terminé
                File.Delete(destinationFilePath + ".temp");

                return true; // Succès
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chiffrement : {ex.Message}");
                return false; // Échec
            }
        }


    }

}