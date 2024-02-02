using EasySave.Enums;
using EasySave.Models;
using EasySave.Services;
using System.Collections.ObjectModel;
using System.CommandLine;
using System.Text.Json;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using EasySave.Controllers;
using System.Text.RegularExpressions;

namespace EasySave;

class Program
{
    static async Task Main(string[] args)
    {
        await InitCommandLine(args);

    }

    static async Task<int> InitCommandLine(string[] args)
    {
        Controller controller = new Controller();

        #region Options
        var backupNameOption = new Option<string>(
            aliases: ["--name", "-n"],
            description: "The name of the backup")
        {
            IsRequired = true
        };

        var sourceDirectoryPathOption = new Option<string>(
            aliases: ["--source", "-s"],
            description: "The path of the destination directory.");

        var destinationDirectoryPathOption = new Option<string>(
            aliases: ["--destination", "-d"],
            description: "The path of the destination directory.");

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
                List<int> jobNumbers = new List<int>();

                // Utiliser une regex pour extraire les numéros de travail de sauvegarde
                Regex regex = new Regex(@"(\d+)([;, -]+(\d+))*");
                MatchCollection matches = regex.Matches(result.ToString());

                foreach (Match match in matches)
                {
                    foreach (Capture capture in match.Captures)
                    {
                        if (int.TryParse(capture.Value, out int jobNumber))
                        {
                            jobNumbers.Add(jobNumber);
                        }
                    }
                }

                return jobNumbers;
            });
        #endregion

        #region Commands
        var rootCommand = new RootCommand("EasySave app");

        var createCommand = new Command("create", "Create a new backup work.")
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

        var saveCommand = new Command("save", "Do a backup work");
        rootCommand.AddCommand(saveCommand);
        #endregion

        #region Handlers
        createCommand.SetHandler(controller.CreateBackupJob,
            backupNameOption, sourceDirectoryPathOption, destinationDirectoryPathOption, backupTypeOption);

        deleteCommand.SetHandler(controller.DeleteBackupJob, backupNameOption);

        showCommand.SetHandler(controller.ShowBackupJobs);

        saveCommand.SetHandler(controller.ExecuteBackupJobs, backupIndexesOption);
        #endregion

        return await rootCommand.InvokeAsync(args);
    }
}