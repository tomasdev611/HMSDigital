using MobileApp.Models;
using MobileApp.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;

namespace MobileApp.Pages.PopUpMenu
{
    public partial class ManualScannerPopup : PopupPage
    {
        private readonly BaseViewModel _viewModel;

        private readonly ScannableLoadListGroup _itemToLoad;

        private readonly ScannableOrderList _itemToFulfill;

        public ManualScannerPopup(ScannableLoadListGroup parameters)
        {
            InitializeComponent();
            _itemToLoad = parameters;
            _viewModel = new ManualScannerViewModel();
            this.BindingContext = _viewModel;
        }

        public ManualScannerPopup(ScannableOrderList parameters)
        {
            InitializeComponent();
            _itemToFulfill = parameters;
            _viewModel = new OrderListManulScannerViewModel();
            this.BindingContext = _viewModel;
        }


        protected override void OnAppearing()
        {
            if(_itemToLoad != null)
            {
                _viewModel.InitializeViewModel(_itemToLoad);
            }
            else if(_itemToFulfill != null)
            {
                _viewModel.InitializeViewModel(_itemToFulfill);
            }
        }

        protected override void OnDisappearing()
        {
            _viewModel.DestroyViewModel();
        }
    }
}
