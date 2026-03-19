using Agnus.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Helpers
{
    class TabColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return Colors.Gray;
            var selectedColor = GetAppColor("Primary") ?? Color.FromArgb("#1735bb");
            var defaultColor = GetAppColor("Secondary") ?? Color.FromArgb("#333339");

            return value.ToString() == parameter.ToString() ? selectedColor : defaultColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();

        private Color? GetAppColor(string key)
        {
            if (Application.Current?.Resources.TryGetValue(key, out var colorValue) == true)
            {
                if (colorValue is Color color)
                    return color;
                if (colorValue is SolidColorBrush brush)
                    return brush.Color;
            }
            return null;
        }
    }
}
