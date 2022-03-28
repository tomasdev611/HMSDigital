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
    public partial class InventoryItemCard : ContentView
    {
        public InventoryItemCard()
        {
            InitializeComponent();
        }
    }
}