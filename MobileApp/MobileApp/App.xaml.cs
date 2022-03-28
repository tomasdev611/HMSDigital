using System;
using System.IO;
using System.Linq;
using System.Reflection;
using MobileApp.Assets;
using MobileApp.Interface;
using MobileApp.Methods;
using MobileApp.Pages.CommonPages;
using MobileApp.Pages.PopUpMenu;
using MobileApp.Pages.Screen;
using MobileApp.Service;
using Newtonsoft.Json.Linq;
using Plugin.PushNotification;
using Xamarin.Forms;

namespace MobileApp
{
    public partial class App : Application
    {
        private static INavigationService _navigationService;

        private CurrentLocation _locationService;

        private readonly AuthService _authService;

        public object NavigationBackParameter { get; set; }

        public static INavigationService NavigationService
        {
            get
            {
                return _navigationService ?? (_navigationService = new NavigationService());
            }
        }

        public App()
        {
            NetworkService.RegisterConnectivityEvent();

            InitailizeConfigurations();
            InitializeComponent();
            DIPS.Xamarin.UI.Library.Initialize();
            CrossPushNotification.Current.OnNotificationAction += NotificationService.NotificationClickAction;
            _locationService = new CurrentLocation();
            _authService = new AuthService();
        }

        protected override void OnStart()
        {
            MainPage = new SplashScreen();
        }

        protected async override void OnResume()
        {
            var hasLocationPermission = await _locationService.RequestPermissionAsync();
            var modalPage = Application.Current.MainPage.Navigation.ModalStack.Count() >= 1 ? Application.Current.MainPage.Navigation.ModalStack.Last() : null;
            var isAuthenticated = await _authService.IsAuthenticated();
            var isLocationEnabled = _locationService.IsLocationEnabled();

            if (!isAuthenticated)
            {
                return;
            }
            if (modalPage != null && modalPage is NoLocationAccess && isLocationEnabled && hasLocationPermission)
            {
                await NavigationService.PopModalAsync();
            }

            if (!isLocationEnabled)
            {
                await NavigationService.PushPopupAsync<LocationPopup>();
            }
            else if (modalPage == null && !hasLocationPermission)
            {
                await NavigationService.PushModalAsync<NoLocationAccess>();
            }
        }

        public void InitailizeConfigurations()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var resName = assembly.GetManifestResourceNames()
                ?.FirstOrDefault(r => r.EndsWith("settings.json", StringComparison.OrdinalIgnoreCase));

            var configurationFile = assembly.GetManifestResourceStream(resName);

            var sr = new StreamReader(configurationFile);

            var json = sr.ReadToEnd();

            var configuration = JObject.Parse(json);

            AppConfiguration.BaseUrl = configuration.Value<string>("ApiBaseUrl");
            AppConfiguration.PatientUrl = configuration.Value<string>("ApiPatientUrl");
            AppConfiguration.NotificationUrl = configuration.Value<string>("ApiNotificationUrl");
            AppConfiguration.Environment = configuration.Value<string>("Environment");
            AppConfiguration.AppCenterAndroid = configuration.Value<string>("AppCenterAndroid");
            AppConfiguration.AppCenteriOS = configuration.Value<string>("AppCenteriOS");
        }
    }
}