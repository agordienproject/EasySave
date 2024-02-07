﻿using EasySave.Controllers;
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
            Resources.Language.Culture = new CultureInfo(AppSettingsJson.GetAppSettings()["CurrentCulture"]);
            
            InitJsonFile(AppSettingsJson.GetBackupJobsFilePath());
            InitJsonFile(AppSettingsJson.GetLogsFilePath());
            InitJsonFile(AppSettingsJson.GetStatesFilePath());

            await InitCommandLine(args);
        }

        public async Task<int> InitCommandLine(string[] args)
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

            //if (args.Length == 0)
            //{
                while (args.Length == 0)
                {
                    //Console.Clear();
                    await rootCommand.InvokeAsync(["--help"]);
                    Console.WriteLine("Saisissez une commande :");
                    args = Console.ReadLine().Split(' ');  // Lire la commande depuis la console et la diviser en arguments
                }
            //}

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

        private void InitJsonFile(string filePath)
        {
            string directoryPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (!File.Exists(filePath))
            {
                using (FileStream fs = File.Create(filePath))
                {
                }
            }
        }
    }
}