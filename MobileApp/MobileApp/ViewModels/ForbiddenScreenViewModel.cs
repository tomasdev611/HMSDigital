using System.Windows.Input;
using MobileApp.Pages.Screen;
using MobileApp.Service;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class ForbiddenScreenViewModel : BaseViewModel
    {
        private readonly StorageService _storageService;

        public ForbiddenScreenViewModel() : base()
        {
            _storageService = new StorageService();

        }

        public override void InitializeViewModel(object data = null)
        {
            Email = (string)data;
        }

        private string _email;

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private ICommand _logoutCommand;

        public ICommand LogoutCommand
        {
            get
            {
                return _logoutCommand ?? (_logoutCommand = new Command(Logout));
            }
        }

        private async void Logout()
        {
            _storageService.ClearStorage();

            await _navigationService.NavigateToAsync(typeof(LoginScreen));
        }
    }
}
