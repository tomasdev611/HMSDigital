using System;
using System.Globalization;
using MobileApp.Methods;
using MobileApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Utils.Converter
{
    class AddressToStringConverter : IMarkupExtension, IValueConverter
    {
        private static AddressToStringConverter _converter = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Address)
            {
                return CommonUtility.AddressToString((Address)value);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new AddressToStringConverter();
            return _converter;
        }
    }
}
