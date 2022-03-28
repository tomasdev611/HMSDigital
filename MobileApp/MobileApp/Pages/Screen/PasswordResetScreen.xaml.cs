using MobileApp.ViewModels;
using Xamarin.Forms;

namespace MobileApp.Pages.Screen
{
    public partial class PasswordResetScreen : ContentPage
    {
        private readonly BaseViewModel viewModel;

        private readonly string _email;

        public PasswordResetScreen(string email)
        {
            InitializeComponent();

            _email = email;
            viewModel = new PasswordResetScreenViewModel();
            this.BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            PagesUtils.PageNavigationFollowUp(nameof(PasswordResetScreen));
            viewModel.InitializeViewModel(_email);
        }
    }
}
