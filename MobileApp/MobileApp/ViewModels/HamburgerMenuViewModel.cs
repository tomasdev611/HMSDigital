using System;
using MobileApp.Assets.Constants;
using MobileApp.Pages.Screen;
using MobileApp.Utils;

namespace MobileApp.ViewModels
{
    public class HamburgerMenuViewModel : BaseViewModel
    {
        private bool _shouldShowFlashMessage;

        public HamburgerMenuViewModel(bool shouldShowFlashMessage) : base()
        {
            _shouldShowFlashMessage = shouldShowFlashMessage;
        }

        public override void InitializeViewModel(object data = null)
        {
            GetHomeScreen();
        }

        private async void GetHomeScreen()
        {
            try
            {
                if (_navigationService.GetMainPage() is HamburgerMenu mainPage)
                {
                    if (mainPage.Detail is HMSNavigationPage navigationPage)
                    {
                        return;
                    }
                    if (!await RolePermissionUtils.CheckPermissionExists())
                    {
                        await _navigationService.PushModalAsync<ForbiddenScreen>(string.Empty);
                        return;
                    }

                    await _navigationService.NavigateToAsync<Dashboard>(_shouldShowFlashMessage);
                    return;
                }
            }
            catch(Exception ex)
            {
                ReportCrash(ex);
                return;
            }
        }
    }
}
