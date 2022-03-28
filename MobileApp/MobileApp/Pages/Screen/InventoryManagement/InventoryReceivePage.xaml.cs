using System;
using System.Collections.Generic;
using MobileApp.Models;
using MobileApp.ViewModels.InventoryManagement;
using Xamarin.Forms;

namespace MobileApp.Pages.Screen.InventoryManagement
{
    public partial class InventoryReceivePage : ContentPage
    {
        #region Private Properties

        private InventoryReceiveViewModel _vm;
        private TransferOrder _transferOrder;

        #endregion

        public InventoryReceivePage()
        {
            InitializeComponent();
            _vm = new InventoryReceiveViewModel();
            this.BindingContext = _vm;
        }

        public InventoryReceivePage(TransferOrder transferOrder)
        {
            _vm = new InventoryReceiveViewModel();
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
