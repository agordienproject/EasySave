using EasySave.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasySave.Utils
{
    public static class CommandLineParseUtils
    {
        public static List<int> ParseBackupJobIndex(string input)
        {
            List<int> backupNumbers = new List<int>();

            // Check 'X-Y' or 'X;Y' format
            Regex regex = new Regex(@"(\d+)[\-;](\d+)");
            Match match = regex.Match(input);

            if (match.Success)
            {
                int start = int.Parse(match.Groups[1].Value);
                int end = int.Parse(match.Groups[2].Value);

                // Add backup numbers to the list
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
                // Check the unique format 'X'
                regex = new Regex(@"(\d+)");
                match = regex.Match(input);

                if (match.Success)
                {
                    int number = int.Parse(match.Groups[1].Value);
                    backupNumbers.Add(number);
                }
                else
                {
                    // Incorrect format
                    return null;
                }
            }

            return backupNumbers.Distinct().ToList();
        }

        public static string[] ParseFilePath(string[] args)
        {
            // Handling quotes for each argument
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = args[i].Trim('"');
            }
            return args;
        }

        public static string RecupLanguage(string input)
        {
            Regex regex = new Regex(@"<([^>]*)>");
            Match match = regex.Match(input);

            if (match.Success)
            {
                string language = match.Groups[1].Value;

                // Checks if the language is in the dictionary
                Dictionary<string, string> languageDictionary = new Dictionary<string, string>
                {
                    { "fr", "fr-FR" },
                    { "en", "en-EN" }
                };

                if (languageDictionary.ContainsKey(language))
                {
                    ConsoleView.ChoosenLanguageCommand(languageDictionary, language);
                    return languageDictionary[language];
                }
                else
                {
                    ConsoleView.ErrorLanguage1();
                    return null;
                }
            }
            else
            {
                ConsoleView.ErrorLanguage2();
                return null;
            }
        }
    }
}
