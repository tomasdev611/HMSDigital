using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HorizontalDivider : ContentView
    {
        public static readonly BindableProperty DividerTextProperty = BindableProperty.Create(nameof(DividerText), typeof(string), typeof(HorizontalDivider), string.Empty);

        public HorizontalDivider()
        {
            InitializeComponent();
            this.BindingContext = this;
        }

        public string DividerText
        {
            get
            {
                return (string)GetValue(DividerTextProperty);
            }
            set
            {
                SetValue(DividerTextProperty, value);
            }
        }
    }
}