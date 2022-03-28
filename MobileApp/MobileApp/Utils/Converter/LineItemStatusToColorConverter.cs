using System;
using System.Globalization;
using MobileApp.Models;
using Xamarin.Forms;

namespace MobileApp.Utils.Converter
{
    public class LineItemStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Color.Transparent;
            }
            var status = value.ToString().ToUpper();
            if (status == "Receive")
            {
                return Color.FromHex("#F7E1B5"); //(Color)Application.Current.Resources["Yellow800Strong"];
            }
            else
            {
                return Color.Transparent;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
