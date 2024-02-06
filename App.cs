using EasySave.Controllers;
using EasySave.Enums;
using Microsoft.Extensions.Configuration;
using System.CommandLine;
using System.Globalization;
using System.Resources;
using System.Text.RegularExpressions;

namespace EasySave
{
    public class App
    {
        private readonly IBackupController _backupController;
        private readonly IConfiguration _configuration;

        public App(IBackupController backupController, IConfiguration configuration) 
        {
            _backupController = backupController;
            _configuration = configuration;
        }

        public async Task Run(string[] args)
        {
            // Set language from settings
            Resources.Language.Culture = new CultureInfo(_configuration.GetValue<string>("CurrentCulture"));
            
            await InitCommandLine(args);
        }

        public async Task<int> InitCommandLine(string[] args)
        {
            #region Options
            var backupNameOption = new Option<string>(
                aliases: ["--name", "-n"],
                description: "The name of the backup")
                {
                    IsRequired = true
                };

            var sourceDirectoryPathOption = new Option<string>(
                aliases: ["--source", "-s"],
                description: "The path of the destination directory.")
                {
                    IsRequired = true
                };

            var destinationDirectoryPathOption = new Option<string>(
                aliases: ["--destination", "-d"],
                description: "The path of the destination directory.")
                {
                    IsRequired = true
                };

            var backupTypeOption = new Option<BackupType>(
                aliases: ["--type", "-t"],
                description: "The type of backup (Complete or Differential).",
                getDefaultValue: () => BackupType.Complete);

            var backupIndexesOption = new Option<List<int>>(
                aliases: ["--range", "-r"],
                description: "Range of backup to execute.",
                isDefault: true,
                parseArgument: result =>
                {
                    return ParseBackupJobIndex(result.ToString());
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

            var deleteCommand = new Command("delete", "Delete the specified backup.")
            {
                backupNameOption,
            };
            rootCommand.AddCommand(deleteCommand);

            var showCommand = new Command("show", "Show all backups");
            rootCommand.AddCommand(showCommand);

            var executeCommand = new Command("execute", "Execute one or more backup jobs") 
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

            return await rootCommand.InvokeAsync(args);
        }

        public static List<int> ParseBackupJobIndex(string input)
        {
            List<int> backupNumbers = new List<int>();

            // Vérifier le format 'X-Y' ou 'X;Y'
            Regex regex = new Regex(@"(\d+)[\-;](\d+)");
            Match match = regex.Match(input);

            if (match.Success)
            {
                int start = int.Parse(match.Groups[1].Value);
                int end = int.Parse(match.Groups[2].Value);

                // Ajouter les numéros de sauvegarde dans la liste
                if (input.Contains(";"))
                {
                    backupNumbers.Add(start);
                    backupNumbers.Add(end);
                }

                if (input.Contains('-'))
                {
                    for (int i = start; i <= end; i++)
                    {
                        backupNumbers.Add(i);
                    }
                }
            }
            else
            {
                // Vérifier le format unique 'X'
                regex = new Regex(@"(\d+)");
                match = regex.Match(input);

                if (match.Success)
                {
                    int number = int.Parse(match.Groups[1].Value);
                    backupNumbers.Add(number);
                }
                else
                {
                    // Format incorrect
                    return null;
                }
            }

            return backupNumbers.Distinct().ToList();
        }
    }
}
