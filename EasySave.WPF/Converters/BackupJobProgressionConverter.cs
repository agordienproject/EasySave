using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EasySave.Converters
{
    public class BackupJobProgressionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is double totalFilesSize && values[1] is double filesSizeLeftToDo)
            {
                if (totalFilesSize > 0)
                {
                    double progressionPercentage = ((totalFilesSize - filesSizeLeftToDo) / totalFilesSize) * 100;
                    return $"{progressionPercentage:F2}%";
                }
                else
                {
                    return "N/A";
                }
            }

            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
