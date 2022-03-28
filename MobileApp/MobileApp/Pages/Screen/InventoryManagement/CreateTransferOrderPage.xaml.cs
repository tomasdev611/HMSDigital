using System;
using System.Collections.Generic;
using MobileApp.Models;
using MobileApp.ViewModels.InventoryManagement;
using Xamarin.Forms;

namespace MobileApp.Pages.Screen.InventoryManagement
{
    public partial class CreateTransferOrderPage : ContentPage
    {
        private CreateTransferOrderViewModel _vm;
        private TransferOrder _transferOrder;

        public CreateTransferOrderPage()
        {
            InitializeComponent();
            _vm = new CreateTransferOrderViewModel();
            this.BindingContext = _vm;
        }

        public CreateTransferOrderPage(TransferOrder transferOrder)
        {
            _vm = new CreateTransferOrderViewModel();
            _transferOrder = transferOrder;
            InitializeComponent();
            this.BindingContext = _vm;
        }

        protected override void OnAppearing()
        {
            _vm.InitializeViewModel(_transferOrder);
        }
    }
}