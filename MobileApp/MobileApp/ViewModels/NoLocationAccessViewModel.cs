using MobileApp.Assets.Constants;
using MobileApp.Interface;
using MobileApp.Methods;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class NoLocationAccessViewModel : BaseViewModel
    {
        private ISettingsHelper _settingsHelper;
        public NoLocationAccessViewModel() : base()
        {
            _settingsHelper = DependencyService.Get<ISettingsHelper>();
        }

        private ImageSource _appLogoPath;

        public ImageSource AppLogoPath
        {
            get
            {
                return _appLogoPath ?? (_appLogoPath = ImageSource.FromResource(AppConstants.IMAGE_PATH + "appLogo.png"));
            }
        }

        private ICommand _openAppSettingsCommand;

        public ICommand OpenAppSettingsCommand
        {
            get
            {
                return _openAppSettingsCommand ?? (_openAppSettingsCommand = new Command(OpenAppSettings));
            }
        }

        private void OpenAppSettings()
        {
            _settingsHelper.OpenAppSettings();
        }
    }
}
