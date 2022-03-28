using MobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages.Screen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InventoryAuditView : ContentPage
    {
        private readonly BaseViewModel _vm;
        public InventoryAuditView()
        {
            InitializeComponent();
            _vm = new InventoryAuditViewModel();
            this.BindingContext = _vm;
        }

        protected override void OnAppearing()
        {
            PagesUtils.PageNavigationFollowUp(nameof(InventoryAuditView));
            _vm.InitializeViewModel();
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            _vm.DestroyViewModel();
        }
    }
}