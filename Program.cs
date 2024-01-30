using EasySave.Enums;
using EasySave.Models;
using EasySave.Services;
using System.CommandLine;
using System.Text.Json;

namespace EasySave;

class Program
{
    static async Task<int> Main(string[] args)
    {
        #region Options
        var backupNameOption = new Option<string>(
            aliases: ["--name", "-n"],
            description: "The name of the backup");
        backupNameOption.IsRequired = true;

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
        createCommand.SetHandler(BackupService.CreateBackup,
            backupNameOption, sourceDirectoryPathOption, destinationDirectoryPathOption, backupTypeOption);

        deleteCommand.SetHandler(BackupService.DeleteBackup, backupNameOption);

        showCommand.SetHandler(BackupService.ShowBackups);

        saveCommand.SetHandler(BackupService.SaveBackup);
        #endregion

        return await rootCommand.InvokeAsync(args);
    }

}