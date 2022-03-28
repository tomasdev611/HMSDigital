using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Utils.Converter
{
    public class StringToPhoneNumberConverter : IMarkupExtension, IValueConverter
    {
        private static StringToPhoneNumberConverter _converter = null;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string s = value.ToString();
            if (string.IsNullOrEmpty(s) || s.Length < 10)
                return value;

            return string.Format("({0}) {1}-{2}", s.Substring(0, 3), s.Substring(3, 3), s.Substring(6, 4)); //"(###) ###-####".
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new StringToPhoneNumberConverter();
            return _converter;
        }
    }
}