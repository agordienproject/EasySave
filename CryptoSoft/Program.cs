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
                Directory.CreateDirectory(Path.GetDirectoryName(destinationFilePath));

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