using MobileApp.ViewModels;
using Xamarin.Forms;

namespace MobileApp.Pages.Screen
{
    public partial class Dashboard : ContentPage
    {
        private BaseViewModel _viewModel;
        private bool _showFlashMessage;
        public Dashboard()
        {
            InitializeComponent();
            BindContext();
        }

        public Dashboard(bool showFlashMessage)
        {
            _showFlashMessage = showFlashMessage;
            InitializeComponent();
            BindContext();
            flashMessage.FlashMessageVisible = _showFlashMessage;
        }

        private void BindContext()
        {
            _viewModel = new DashboardViewModel();
            this.BindingContext = _viewModel;
        }


        protected override void OnAppearing()
        {
            _viewModel.InitializeViewModel(_showFlashMessage);
            PagesUtils.PageNavigationFollowUp(nameof(Dashboard));
        }

        protected override void OnDisappearing()
        {
            _viewModel.DestroyViewModel();
        }
    }
}

