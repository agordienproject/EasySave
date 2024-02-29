using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ConsoleDeportee.Converters
{
    public class PathToDirectoryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            string fullPath = value.ToString();
            string shortPath = GetShortPath(fullPath);

            return shortPath;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string GetShortPath(string fullPath)
        {
            return ".../" + Path.GetFileName(fullPath);
        }
    }
}
