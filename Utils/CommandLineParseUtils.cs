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
