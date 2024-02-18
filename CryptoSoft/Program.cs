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
                double transferTime;

                var key = configuration["cipherKey"];
                DateTime DateBefore = DateTime.Now;
                var success = XORCipher(sourceFilePath, destinationFilePath, key);
                XORCipher(sourceFilePath, destinationFilePath, key);
                DateTime DateAfter = DateTime.Now;
                transferTime = (DateAfter - DateBefore).TotalSeconds;
                Console.WriteLine(transferTime);
                int exitCode = (int)(transferTime * 1000); // Conversion du temps en millisecondes à un entier
                Console.WriteLine(exitCode);


                if (success)
                {
                    Console.WriteLine("Test réussi");
                    Environment.ExitCode = 1; // Succès
                }
                else
                {
                    Console.WriteLine("Test échoué");
                    Environment.ExitCode = -1; // Échec
                }

            }, sourceFilePathOption, destinationFilePathOption);
            #endregion

            return await rootCommand.InvokeAsync(args);
        }

        private static bool XORCipher(string sourceFilePath, string destinationFilePath, string key)
        {
            try
            {
                byte[] sourceBytes = File.ReadAllBytes(sourceFilePath);
                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);

                // XOR
                for (int i = 0; i < sourceBytes.Length; i++)
                {
                    sourceBytes[i] ^= keyBytes[i % keyBytes.Length];
                }

                File.WriteAllBytes(destinationFilePath, sourceBytes);
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