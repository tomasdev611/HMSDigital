using System.Windows.Input;
using MobileApp.Interface;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class LocationPopupViewModel : BaseViewModel
    {
        private readonly ISettingsHelper _settingsHelper;

        public LocationPopupViewModel() : base()
        {
            _settingsHelper = DependencyService.Get<ISettingsHelper>();
        }

        public override void InitializeViewModel(object data = null)
        {

        }

        public ICommand _openSettingsCommand;

        public ICommand OpenSettingsCommand
        {
            get
            {
                return _openSettingsCommand ?? (_openSettingsCommand = new Command(OpenSetting));
            }
        }

        private void OpenSetting()
        {
            _settingsHelper.OpenLocationSetting();
            _navigationService.PopPopupAsync();
        }
    }
}
