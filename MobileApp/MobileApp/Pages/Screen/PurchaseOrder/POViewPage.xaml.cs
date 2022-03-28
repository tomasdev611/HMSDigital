using System;
using System.Collections.Generic;
using MobileApp.ViewModels;
using MobileApp.ViewModels.PurchaseOrder;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace MobileApp.Pages.Screen.PurchaseOrder
{
    public partial class POViewPage : ContentPage
    {
        #region Private Properties

        private POViewViewModel _vm;

        #endregion

        public POViewPage()
        {
            InitializeComponent();
            _vm = new POViewViewModel();
            this.BindingContext = _vm;
            On<iOS>().SetUseSafeArea(true);
        }

        protected override void OnAppearing()
        {
            _vm.InitializeViewModel();
        }
    }
}