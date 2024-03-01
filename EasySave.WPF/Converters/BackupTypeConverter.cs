using EasySave.Domain.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace EasySave.Converters
{
    public class BackupTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string selectedType))
                return null;

            switch (selectedType)
            {
                case "Full":
                    return BackupType.Full;
                case "Differential":
                    return BackupType.Differential;
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is BackupType selectedType))
                return null;

            switch (selectedType)
            {
                case BackupType.Full:
                    return "Full";
                case BackupType.Differential:
                    return "Differential";
                default:
                    return null;
            }
        }
    }
}
