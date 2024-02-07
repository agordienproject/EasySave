using EasySave.Controllers;
using EasySave.Enums;
using EasySave.Utils;
using Microsoft.Extensions.Configuration;
using System.CommandLine;
using System.Globalization;
using System.Resources;
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

            RootCommand rootCommand = InitCommandLine();

            if (args.Length == 0)
            {
                await rootCommand.InvokeAsync(["--help"]);
                while (true)
                {
                    Console.WriteLine("Saisissez une commande :");
                    args = Console.ReadLine().Split(' ');
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
            #endregion

            #region Handlers
            createCommand.SetHandler(_backupController.CreateBackupJob,
                backupNameOption, sourceDirectoryPathOption, destinationDirectoryPathOption, backupTypeOption);

            deleteCommand.SetHandler(_backupController.DeleteBackupJob, backupNameOption);

            showCommand.SetHandler(_backupController.ShowBackupJobs);

            executeCommand.SetHandler(_backupController.ExecuteBackupJobs, backupIndexesOption);
            #endregion

            return rootCommand;
        }
    }
}
