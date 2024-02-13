using Microsoft.Extensions.Configuration;
using System.CommandLine;
using System.CommandLine.Invocation;
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
                XORCipher(sourceFilePath, destinationFilePath, key);
            }, sourceFilePathOption, destinationFilePathOption);
            #endregion

            return await rootCommand.InvokeAsync(args);
        }

        private static void XORCipher(string sourceFilePath, string destinationFilePath, string key)
        {
            byte[] sourceBytes = File.ReadAllBytes(sourceFilePath);
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);

            // XOR
            for (int i = 0; i < sourceBytes.Length; i++)
            {
                sourceBytes[i] ^= keyBytes[i % keyBytes.Length];
            }

            File.WriteAllBytes(destinationFilePath, sourceBytes);
        }
    }

}
