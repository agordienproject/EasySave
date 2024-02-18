using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EasySave.Converters
{
    public class LanguageToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            string targetCulture = parameter.ToString();
            string currentCulture = value.ToString();

            return currentCulture.Equals(targetCulture, StringComparison.OrdinalIgnoreCase);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked && isChecked)
            {
                return parameter; // Retourner la culture associée au bouton radio
            }

            // Retourner Binding.DoNothing si le bouton n'est pas sélectionné
            return Binding.DoNothing;
        }
    }
}
