using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Utils.Converter
{
    public class NullToBoolConverter : IMarkupExtension, IValueConverter
    {
        private static NullToBoolConverter _converter = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new NullToBoolConverter();
            return _converter;
        }
    }
}