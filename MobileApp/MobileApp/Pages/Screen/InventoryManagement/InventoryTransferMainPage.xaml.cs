using MobileApp.Models;
using MobileApp.ViewModels.InventoryManagement;
using MobileApp.ViewModels.PurchaseOrder;
using Xamarin.Forms;

namespace MobileApp.Pages.Screen.InventoryManagement
{
    public partial class InventoryTransferMainPage : ContentPage
    {
        #region Private Properties

        private InventoryTransferMainViewModel _vm;

        #endregion

        public InventoryTransferMainPage()
        {
            InitializeComponent();
            _vm = new InventoryTransferMainViewModel();
            this.BindingContext = _vm;
        }

        protected override void OnAppearing()
        {
            _vm.InitializeViewModel();
        }
    }
}