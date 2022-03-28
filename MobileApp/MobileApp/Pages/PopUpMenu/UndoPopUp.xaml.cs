using MobileApp.DataBaseAttributes;
using MobileApp.ViewModels;
using Rg.Plugins.Popup.Pages;

namespace MobileApp.Pages.PopUpMenu
{
    public partial class UndoPopUp : PopupPage
    {
        private readonly BaseViewModel _viewModel;

        private readonly ScanItem _scannedItem;

        public UndoPopUp(ScanItem scannedItem)
        {
            _scannedItem = scannedItem;
            _viewModel = new UndoScanPopupViewModel();
            InitializeComponent();
            this.BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            _viewModel.InitializeViewModel(_scannedItem);
        }
    }
}
