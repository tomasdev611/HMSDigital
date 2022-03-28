using MobileApp.Methods;
using MobileApp.ViewModels;
using Rg.Plugins.Popup.Pages;

namespace MobileApp.Pages.PopUpMenu
{
    public partial class CallReport : PopupPage
    {
        private string _callingNumber;

        private readonly BaseViewModel _viewModel;

        public CallReport(string number)
        {
            _callingNumber = number;
            _viewModel = new CallReportViewModel();
            this.BindingContext = _viewModel;

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            _viewModel.InitializeViewModel(_callingNumber);
        }
    }
}
