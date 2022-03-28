using System;
using System.Globalization;
using MobileApp.Models;
using Xamarin.Forms;

namespace MobileApp.Utils.Converter
{
    public class PurchaseOrderStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                return Color.FromHex("DCEAFF");
            }
            var status = value.ToString().ToUpper();
            if(status == PurchaseOrderStatus.OPEN.ToString())
            {
                return Color.FromHex("#DCEAFF"); //(Color)Application.Current.Resources["Yellow800Strong"];
            }
            else if (status == PurchaseOrderStatus.PARTIAL.ToString())
            {
                return Color.FromHex("#F7E1B5"); // (Color)Application.Current.Resources["Green800"];
            }
            else
            {
                return Color.FromHex("#B1DCC7"); //(Color)Application.Current.Resources["BlueLight"];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
