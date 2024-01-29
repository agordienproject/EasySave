using EasySave.Enums;
using EasySave.Models;
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

        var addCommand = new Command("add", "Add a new backup work.")
            {
                backupNameOption,
                sourceDirectoryPathOption,
                destinationDirectoryPathOption,
                backupTypeOption
            };
        rootCommand.AddCommand(addCommand);

        var deleteCommand = new Command("delete", "Delete the specified backup.")
            {
                backupNameOption,
            };
        rootCommand.AddCommand(deleteCommand);

        #endregion

        #region Handlers
        addCommand.SetHandler(CreateBackup,
            backupNameOption, sourceDirectoryPathOption, destinationDirectoryPathOption, backupTypeOption);

        deleteCommand.SetHandler(DeleteBackup, backupNameOption);
        
        #endregion
        
        return await rootCommand.InvokeAsync(args);
    }

    static async Task CreateBackup(string name, string sourcePath, string destinationPath, BackupType backupType)
    {
        string backupsFilePath = "C:\\Users\\funny\\source\\repos\\lhPierre\\EasySave\\backups.json";

        using FileStream openStream = File.Open(backupsFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

        List<Backup?> backupList = new List<Backup?>();
        try
        {
            backupList = await JsonSerializer.DeserializeAsync<List<Backup?>>(openStream);
        }
        catch (JsonException e)
        {
            Console.WriteLine(e.InnerException);
            Console.WriteLine(e.Message);    
        }
        
        backupList.Add(new Backup(name, sourcePath, destinationPath, backupType));
        
        var options = new JsonSerializerOptions { WriteIndented = true, };
        await JsonSerializer.SerializeAsync(openStream, backupList, options);
    }

    static async Task DeleteBackup(string name)
    {
        Console.WriteLine($"Deleting backup... {name}");
    }

    static async Task SaveBackup()
    {

    }

    static async Task ShowBackups()
    {

    }
}