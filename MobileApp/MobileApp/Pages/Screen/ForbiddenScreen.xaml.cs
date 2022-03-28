using MobileApp.ViewModels;
using Xamarin.Forms;

namespace MobileApp.Pages.Screen
{
    public partial class ForbiddenScreen : ContentPage
    {
        private readonly BaseViewModel viewModel;

        private readonly string _email;

        public ForbiddenScreen(string email)
        {
            InitializeComponent();
            _email = email;
            viewModel = new ForbiddenScreenViewModel();
            this.BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            viewModel.InitializeViewModel(_email);
        }
    }
}
