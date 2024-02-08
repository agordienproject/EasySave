using EasySave.Controllers;
using EasySave.Enums;
using EasySave.Utils;
using Microsoft.Extensions.Configuration;
using System.Collections.Specialized;
using System.CommandLine;
using System.Globalization;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;

namespace EasySave
{
    public class App
    {
        private readonly IConfiguration _configuration;
        private readonly IBackupController _backupController;

        public App(IConfiguration configuration, IBackupController backupController) 
        {
            _configuration = configuration;
            _backupController = backupController;
        }

        public async Task Run(string[] args)
        {
            // Set app culture from appsettings.json
            Resources.Language.Culture = new CultureInfo(_configuration["CurrentCulture"]);

            // Traitement des guillemets
            args = ParseFilePath(args);

            RootCommand rootCommand = InitCommandLine();

            if (args.Length == 0)
            {
                await rootCommand.InvokeAsync(new string[] { "--help" });
                while (true)
                {
                    Console.WriteLine($"{Resources.Language.ChooseCommand}");
                    args = Console.ReadLine().Split(' ');
                    await rootCommand.InvokeAsync(args);
                }
            }

            await rootCommand.InvokeAsync(args);
        }

        private string[] ParseFilePath(string[] args)
        {
            // Handling quotes for each argument
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = args[i].Trim('"');
            }
            return args;
        }


        public RootCommand InitCommandLine()
        {
            #region Options
            var backupNameOption = new Option<string>(
                aliases: ["--name", "-n"],
                description: Resources.Language.NameOptionDescription)
                {
                    IsRequired = true
                };

            var sourceDirectoryPathOption = new Option<string>(
                aliases: ["--source", "-s"],
                description: Resources.Language.SourceDirectoryPathOptionDescription)
                {
                    IsRequired = true
                };

            var destinationDirectoryPathOption = new Option<string>(
                aliases: ["--destination", "-d"],
                description: Resources.Language.DestinationDirectoryPathOptionDescription)
                {
                    IsRequired = true
                };

            var backupTypeOption = new Option<BackupType>(
                aliases: ["--type", "-t"],
                description: Resources.Language.BackupTypeOptionDescription,
                getDefaultValue: () => BackupType.Complete);

            var backupIndexesOption = new Option<List<int>>(
                aliases: ["--range", "-r"],
                description: Resources.Language.BackupIndexesOptionDescription,
                isDefault: true,
                parseArgument: result =>
                {
                    return CommandLineParseUtils.ParseBackupJobIndex(result.ToString());
                });
            var languageOption = new Option<string>(
            aliases: new[] { "--language", "-l" },
            description: Resources.Language.LanguageCommandDescription,
            isDefault: true,
            parseArgument: result =>
            {
                var language = RecupLanguage(result.ToString());
                if (language == null)
                {
                    // Quitter l'application si la langue n'a pas pu être récupérée
                    Environment.Exit(1); // Utilisez le code d'erreur que vous préférez
                }
                return language;
            });
            #endregion

            #region Commands
            var rootCommand = new RootCommand("EasySave app");

            var createCommand = new Command("create", Resources.Language.CreateCommandDescription)
            {
                backupNameOption,
                sourceDirectoryPathOption,
                destinationDirectoryPathOption,
                backupTypeOption
            };
            rootCommand.AddCommand(createCommand);

            var deleteCommand = new Command("delete", Resources.Language.DeleteCommandDescription)
            {
                backupNameOption,
            };
            rootCommand.AddCommand(deleteCommand);

            var showCommand = new Command("show", Resources.Language.ShowCommandDescription);
            rootCommand.AddCommand(showCommand);

            var executeCommand = new Command("execute", Resources.Language.ExecuteCommandDescription) 
            { 
                backupIndexesOption,
            };
            rootCommand.AddCommand(executeCommand);
            var languageCommand = new Command("language", Resources.Language.LanguageCommandDescription)
            {
                languageOption,
            };
            rootCommand.AddCommand(languageCommand);
            #endregion

            #region Handlers
            createCommand.SetHandler(_backupController.CreateBackupJob,
                backupNameOption, sourceDirectoryPathOption, destinationDirectoryPathOption, backupTypeOption);

            deleteCommand.SetHandler(_backupController.DeleteBackupJob, backupNameOption);

            showCommand.SetHandler(_backupController.ShowBackupJobs);

            executeCommand.SetHandler(_backupController.ExecuteBackupJobs, backupIndexesOption);
            
            languageCommand.SetHandler(AppSettingsJson.SetCurrentCulture, languageOption);

            #endregion

            return rootCommand;
        }
        public static string RecupLanguage(string input)
        {
            Regex regex = new Regex(@"<([^>]*)>");
            Match match = regex.Match(input);

            if (match.Success)
            {
                string language = match.Groups[1].Value;

                // Vérifie si la langue est dans le dictionnaire
                Dictionary<string, string> languageDictionary = new Dictionary<string, string>
                {
                    { "fr", "fr-FR" },
                    { "en", "en-EN" }
                };

                if (languageDictionary.ContainsKey(language))
                {
                    Console.WriteLine($" {Resources.Language.ChoosenLanguageCommand} : {languageDictionary[language]}");
                    return languageDictionary[language];
                }
                else
                {
                    Console.WriteLine($" {Resources.Language.ErrorLanguage1}");
                    return null;
                }
            }
            else
            {
                Console.WriteLine($" {Resources.Language.ErrorLanguage2}");
                return null;
            }
        }
    }
}
