using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace WPFTools
{
    // Converts a given number based on the provided double parameter value.
    public class RatioConverter : MarkupExtension, IValueConverter
    {
        // Code based on: 
        // https://stackoverflow.com/questions/8121906/resize-wpf-window-and-contents-depening-on-screen-resolution
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double size = System.Convert.ToDouble(value) * System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
            return size.ToString("G0", CultureInfo.InvariantCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double size = System.Convert.ToDouble(value) / System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
            return size.ToString("G0", CultureInfo.InvariantCulture);
        }

        // "Markup extensions return objects to callers, based on the input of string attribute values or markup elements in XAML"
        // https://docs.microsoft.com/en-us/dotnet/api/system.windows.markup.markupextension?view=netcore-3.1
        RatioConverter _instance;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new RatioConverter());
        }
    }
}
