using EasySave.Controllers;
using EasySave.Enums;
using EasySave.Utils;
using EasySave.Views;
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
            args = CommandLineParseUtils.ParseFilePath(args);
            RootCommand rootCommand = InitCommandLine();

            if (args.Length == 0)
            {
                Console.Clear();
                await rootCommand.InvokeAsync(new string[] { "--help" });
                while (true)
                {
                    //ConsoleView.EnterCommand();
                    Console.Write(" > ");
                    string input = Console.ReadLine();
                    MatchCollection matches = Regex.Matches(input, @"[\""].+?[\""]|[^ ]+");
                    args = matches.Select(match => match.Value).ToArray();
                    args = CommandLineParseUtils.ParseFilePath(args);
                    await rootCommand.InvokeAsync(args);
                }
            }

            await rootCommand.InvokeAsync(args);
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
                aliases: [ "--language", "-l" ],
                description: Resources.Language.LanguageOptionDescription
                );
            #endregion

            #region Commands
            var rootCommand = new RootCommand(ConsoleView.Print());

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
        
    }
}
