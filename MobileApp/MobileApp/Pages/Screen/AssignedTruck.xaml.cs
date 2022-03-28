using MobileApp.ViewModels;
using Xamarin.Forms;

namespace MobileApp.Pages.Screen
{
    public partial class AssignedTruck : ContentPage
    {
        private readonly BaseViewModel _viewModel;

        private bool _isOrderCompleted;

        public AssignedTruck(bool isOrderCompleted)
        {
            InitializeComponent();
            _viewModel = new AssignedTruckViewModel();
            this.BindingContext = _viewModel;
            _isOrderCompleted = isOrderCompleted;
        }

        protected override void OnAppearing()
        {
            PagesUtils.PageNavigationFollowUp(nameof(AssignedTruck));
            _viewModel.InitializeViewModel(_isOrderCompleted);
        }
    }
}
