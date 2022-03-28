using MobileApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages.Screen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrentInventory : ContentPage
    {
        public CurrentInventory()
        {
            InitializeComponent();
            this.BindingContext = new CurrentInventoryViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            PagesUtils.PageNavigationFollowUp(nameof(CurrentInventory));
        }
    }
}