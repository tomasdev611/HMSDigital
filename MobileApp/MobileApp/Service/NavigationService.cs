using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MobileApp.Interface;
using MobileApp.Methods;
using MobileApp.Pages.CommonPages;
using MobileApp.Pages.PopUpMenu;
using MobileApp.Pages.Screen;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace MobileApp.Service
{
    class NavigationService : INavigationService
    {
        private readonly AuthService _authenticationService;

        private readonly CurrentLocation _locationService;

        private readonly object _sync = new object();

        protected Application CurrentApplication => Application.Current;

        public NavigationService()
        {
            _authenticationService = new AuthService();
            _locationService = new CurrentLocation();
        }
        public async Task InitializeAsync()
        {
            if (NetworkService.CheckConnectivity())
            {
                var isAuthenticated = await _authenticationService.IsAuthenticated();
                if (isAuthenticated)
                {
                    var isLocationEnabled = _locationService.IsLocationEnabled();
                    if (!isLocationEnabled)
                    {
                        await PushPopupAsync<LocationPopup>();
                    }

                    var hasLocationPermission = await _locationService.RequestPermissionAsync();
                    if (!hasLocationPermission)
                    {
                        await PushModalAsync<NoLocationAccess>();
                    }
                    else
                    {
                        await NavigateToAsync(typeof(HamburgerMenu), true);
                    }
                }
                else
                {
                    await NavigateToAsync<LoginScreen>();
                }
            }
        }
        public Task NavigateToAsync<TView>() where TView : Page
        {
            return InternalNavigateToAsync(typeof(TView), null);
        }
        public Task NavigateToAsync<TView>(object parameter) where TView : Page
        {
            return InternalNavigateToAsync(typeof(TView), parameter);
        }
        public Task NavigateToAsync(Type viewType)
        {
            return InternalNavigateToAsync(viewType, null);
        }
        public Task NavigateToAsync(Type viewType, object parameter)
        {
            return InternalNavigateToAsync(viewType, parameter);
        }

        public Task ChangeDetailPageAsync<TView>() where TView : Page
        {
            return InternalNavigateToAsync(typeof(TView), null, true);
        }
        public Task ChangeDetailPageAsync<TView>(object parameter) where TView : Page
        {
            return InternalNavigateToAsync(typeof(TView), parameter, true);
        }
        public Task ChangeDetailPageAsync(Type viewType)
        {
            return InternalNavigateToAsync(viewType, null, true);
        }
        public Task ChangeDetailPageAsync(Type viewType, object parameter)
        {
            return InternalNavigateToAsync(viewType, parameter, true);
        }

        public Task PushPopupAsync<TView>() where TView : Page
        {
            return InternalPushPopupAsync(typeof(TView), null);
        }
        public Task PushPopupAsync<TView>(object parameter) where TView : Page
        {
            return InternalPushPopupAsync(typeof(TView), parameter);
        }
        public Task PushPopupAsync(Type viewType)
        {
            return InternalPushPopupAsync(viewType, null);
        }
        public Task PushPopupAsync(Type viewType, object parameter)
        {
            return InternalPushPopupAsync(viewType, parameter);
        }

        public Task PushModalAsync<TView>() where TView : Page
        {
            return InternalPushModalAsync(typeof(TView), null);
        }

        public Task PushModalAsync<TView>(object parameter) where TView : Page
        {
            return InternalPushModalAsync(typeof(TView), parameter);
        }

        public Task PushModalAsync(Type viewType)
        {
            return InternalPushModalAsync(viewType, null);
        }

        public Task PushModalAsync(Type viewType, object parameter)
        {
            return InternalPushModalAsync(viewType, parameter);
        }

        public async Task ClearBackStack()
        {
            await CurrentApplication.MainPage.Navigation.PopToRootAsync();
        }

        public async Task NavigateBackAsync()
        {
            if (CurrentApplication.MainPage is HamburgerMenu mainPage)
            {
                await mainPage.Detail.Navigation.PopAsync();
            }
            else if (CurrentApplication.MainPage != null)
            {
                await CurrentApplication.MainPage.Navigation.PopAsync();
            }
        }

        public async Task NavigateBackAsync(object parameter)
        {
            if(CurrentApplication.MainPage == null)
            {
                return;
            }

            (App.Current as App).NavigationBackParameter = parameter;

            if (CurrentApplication.MainPage is HamburgerMenu mainPage)
            {
                await mainPage.Detail.Navigation.PopAsync();
            }
            else if (CurrentApplication.MainPage != null)
            {
                await CurrentApplication.MainPage.Navigation.PopAsync();
            }
        }

        public async Task PopPopupAsync()
        {
            await InternalPopPopupAsync();
        }

        public async Task PopModalAsync()
        {
            await InternalPopModalAsync();
        }

        public virtual Task RemoveLastFromBackStackAsync()
        {
            if (CurrentApplication.MainPage is HamburgerMenu mainPage)
            {
                if (mainPage.Detail.Navigation.NavigationStack.Count >= 2)
                {
                    mainPage.Detail.Navigation.RemovePage(
                        mainPage.Detail.Navigation.NavigationStack[mainPage.Detail.Navigation.NavigationStack.Count - 2]);
                }
            }
            return Task.FromResult(true);
        }
        public async Task PopToRootAsync()
        {
            if (CurrentApplication.MainPage is HamburgerMenu mainPage)
            {
                await mainPage.Detail.Navigation.PopToRootAsync();
            }
        }

        protected virtual async Task InternalPushPopupAsync(Type viewtype, object parameter)
        {
            var page = CreatePage(viewtype, parameter);
            await CurrentApplication.MainPage.Navigation.PushPopupAsync(page as PopupPage);
        }

        protected virtual async Task InternalPopPopupAsync()
        {
            await CurrentApplication.MainPage.Navigation.PopPopupAsync();
        }

        protected virtual async Task InternalPushModalAsync(Type viewtype, object parameter)
        {
            (App.Current as App).NavigationBackParameter = null;
            var page = CreatePage(viewtype, parameter);
            await CurrentApplication.MainPage.Navigation.PushModalAsync(page);
        }

        protected virtual async Task InternalPopModalAsync()
        {
            await CurrentApplication.MainPage.Navigation.PopModalAsync();
        }

        protected virtual async Task InternalNavigateToAsync(Type viewType, object parameter, bool changeDetailPage = false)
        {
            try
            {
                Page page = CreatePage(viewType, parameter);
                if (page is HamburgerMenu || page is SplashScreen || page is ForbiddenScreen)
                {
                    CurrentApplication.MainPage = page;
                }
                else if (page is LoginScreen)
                {
                    CurrentApplication.MainPage = new HMSNavigationPage(page);
                }
                else if (CurrentApplication.MainPage is HamburgerMenu)
                {
                    var mainPage = CurrentApplication.MainPage as HamburgerMenu;
                    if (mainPage.Detail is HMSNavigationPage navigationPage && !changeDetailPage)
                    {
                        var currentPage = navigationPage.CurrentPage;
                        if (currentPage.GetType() != page.GetType())
                        {
                            await navigationPage.PushAsync(page);
                        }
                    }
                    else
                    {
                        navigationPage = new HMSNavigationPage(page);
                        mainPage.Detail = navigationPage;
                    }
                    mainPage.IsPresented = false;
                }
                else
                {
                    var navigationPage = CurrentApplication.MainPage as HMSNavigationPage;
                    if (navigationPage != null)
                    {
                        await navigationPage.PushAsync(page);
                    }
                    else
                    {
                        CurrentApplication.MainPage = new HMSNavigationPage(page);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Page GetMainPage()
        {
            return CurrentApplication.MainPage;
        }

        protected Page CreatePage(Type pageType, object parameter = null)
        {
            lock (_sync)
            {
                ConstructorInfo constructor;
                object[] parameters;
                if (parameter == null)
                {
                    constructor = pageType.GetTypeInfo()
                        .DeclaredConstructors
                        .FirstOrDefault(c => !c.GetParameters().Any());
                    parameters = new object[]
                    {
                    };
                }
                else
                {
                    constructor = pageType.GetTypeInfo()
                        .DeclaredConstructors
                        .FirstOrDefault(
                            c =>
                            {
                                var p = c.GetParameters();
                                return p.Length == 1
                                       && p[0].ParameterType == parameter.GetType();
                            });
                    parameters = new[]
                    {
                        parameter
                    };
                }
                if (constructor == null)
                {
                    throw new InvalidOperationException("No suitable constructor found for page ");
                }
                var page = constructor.Invoke(parameters) as Page;
                return page;
            }
        }
    }
}