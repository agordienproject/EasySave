using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace EasySave.Converters
{
    public class BytesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double bytes)
            {
                double size;

                if (bytes < Math.Pow(2, 30)) // Si moins d'un Go
                {
                    size = bytes / Math.Pow(2, 20); // Convertir en Mo
                    return $"{size:F2} Mo";
                }
                else
                {
                    size = bytes / Math.Pow(2, 30); // Convertir en Go
                    return $"{size:F2} Go";
                }
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
