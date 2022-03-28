using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlashMessage : ContentView
    {

        public static readonly BindableProperty FlashMessageTextProperty = BindableProperty.Create(nameof(FlashMessageText), typeof(string), typeof(FlashMessage), string.Empty);

        public string FlashMessageText
        {
            get
            {
                return (string)GetValue(FlashMessageTextProperty);
            }
            set
            {
                SetValue(FlashMessageTextProperty, value);
            }
        }

        public static readonly BindableProperty FlashMessageVisibleProperty = BindableProperty.Create(nameof(FlashMessageVisible), typeof(bool), typeof(FlashMessage), false, BindingMode.TwoWay);

        public bool FlashMessageVisible
        {
            get
            {
                return (bool)GetValue(FlashMessageVisibleProperty);
            }
            set
            {
                SetValue(FlashMessageVisibleProperty, value);
            }
        }

        public FlashMessage()
        {
            InitializeComponent();
            this.BindingContext = this;
        }

        private void Dismiss(object sender, EventArgs e)
        {
            FlashMessageVisible = false;
        }
    }
}