using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

/// <summary>
/// XAML logic can be done via "Converters", which are code snippets that are passed in to the XAML. They're like plugins that can do some work for you.
/// https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.data.binding.converter?view=winrt-19041
/// Converters are also given parameters, which are passed in to the code snippet.
/// 
/// The namespace WPFTools is a shared class library that can be added to other XAML projects, allowing the use of these converters anywhere in a modular fashion. This project
/// should remain independent of specific applications.
/// </summary>
namespace WPFTools
{
    // Converts a given number based on the provided double parameter value.
    // Usage: "{Binding Source={<Number to Convert>}, Converter={RatioConverter}, ConverterParameter='<Ratio to convert to, using 1 as the base (so .95 reduces the input to 95%)>' }" 
    //  The Converter={RatioConverter} arg is this method being given to the XAML. The ConverterParameter is passed in as the 'object parameter' in the method header. The Number to 
    //  Convert is passed as the 'object value' when the converter is invoked on it by the XAML design code.
    public class RatioConverter : MarkupExtension, IValueConverter
    {
        // Code based on: 
        // https://stackoverflow.com/questions/8121906/resize-wpf-window-and-contents-depening-on-screen-resolution
        // Increases or Decreases the "value" to a ratio of itself, dictated by the double value in "parameter"
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
        // "In XAML applications, markup extensions are a method/technique to gain a value that is neither a specific XAML object nor a primitive type. 
        //   Markup extensions can be defined by opening and closing curly braces and inside that curly braces, the scope of the markup extension is defined.
        //   Data binding and static resources are markup extensions.There are some predefined XAML markup extensions in System.xaml which can be used."
        // https://www.tutorialspoint.com/xaml/xaml_markup_extensions.htm
        // I think this just has to be here to provide a copy of the method up there as the {RatioConverter} arg but honestly I'm not sure; it seems like
        //   this would be done under the hood if so
        RatioConverter _instance;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new RatioConverter());
        }
    }
}
