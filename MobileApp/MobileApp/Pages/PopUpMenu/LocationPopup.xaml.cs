using MobileApp.ViewModels;
using Rg.Plugins.Popup.Pages;

namespace MobileApp.Pages.PopUpMenu
{
    public partial class LocationPopup : PopupPage
    {
        private readonly BaseViewModel _viewModel;

        public LocationPopup()
        {
            _viewModel = new LocationPopupViewModel();
            this.BindingContext = _viewModel;

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            _viewModel.InitializeViewModel();
        }
    }
}
