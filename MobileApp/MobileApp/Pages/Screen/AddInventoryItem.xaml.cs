using MobileApp.Models;
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
    public partial class AddInventoryItem : ContentPage
    {
        private BaseViewModel _vm;
        private Inventory _inventoryItem;
        public AddInventoryItem()
        {
            InitializeComponent();
            _vm = new AddInventoryItemViewModel();
            this.BindingContext = _vm;
        }

        public AddInventoryItem(Inventory inventoryItem)
        {
            InitializeComponent();
            _vm = new AddInventoryItemViewModel();
            this.BindingContext = _vm;
            _inventoryItem = inventoryItem;
        }

        protected override void OnAppearing()
        {
            PagesUtils.PageNavigationFollowUp(nameof(AddInventoryItem));
            if (_inventoryItem == null)
            {
                _vm.InitializeViewModel();
            }
            else
            {
                _vm.InitializeViewModel(_inventoryItem);
            }
        }

        protected override void OnDisappearing()
        {
            _vm.DestroyViewModel();
        }
    }
}