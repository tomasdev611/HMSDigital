using System;
using Xamarin.Forms;

namespace MobileApp.Models
{
    public class HamburgerMenuItem : BindableObject
    {
        private string _title;
        private Type _viewType;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }
        public Type ViewType
        {
            get
            {
                return _viewType;
            }
            set
            {
                _viewType = value;
                OnPropertyChanged();
            }
        }
    }
}
