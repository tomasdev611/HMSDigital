using System;
using System.Windows.Input;
using MobileApp.Assets.Constants;
using MobileApp.Exceptions;
using MobileApp.Methods;
using MobileApp.Models;
using MobileApp.Pages.CommonPages;
using MobileApp.Pages.Screen;
using MobileApp.Service;
using MobileApp.Utils;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class LoginScreenViewModel : BaseViewModel
    {
        private readonly AuthService _authService;

        private readonly NotificationService _notificationService;

        private readonly ResetPasswordService _resetPasswordService;

        private readonly PermissionHandler _permissionService;

        public LoginScreenViewModel() : base()
        {
            _authService = new AuthService();
            _resetPasswordService = new ResetPasswordService();
            _notificationService = new NotificationService();
            _permissionService = new PermissionHandler();
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

        private string _password;

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private bool _shouldShowError;

        public bool ShouldShowError
        {
            get
            {
                return _shouldShowError;
            }
            set
            {
                _shouldShowError = value;
                OnPropertyChanged();
            }
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        private bool _isLoading;

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        private ICommand _loginCommand;

        public ICommand LoginCommand
        {
            get
            {
                return _loginCommand ?? (_loginCommand = new Command(Login));
            }
        }

        private ICommand _resetPasswordCommand;

        public ICommand ResetPasswordCommand
        {
            get
            {
                return _resetPasswordCommand ?? (_resetPasswordCommand = new Command(NavigateToResetPassword));
            }
        }

        private async void Login()
        {
            try
            {
                IsLoading = true;
                var loginResult = await _authService.LoginAsync(new UserLogin
                {
                    UserName = Email,
                    Password = Password,
                    GrantType = "password"
                });

                if (loginResult == null)
                {
                    Password = "";
                    ShowErrorMessage("Wrong email and password");
                    return;
                }

                if (!await RolePermissionUtils.CheckPermissionExists())
                {
                    await _navigationService.NavigateToAsync(typeof(ForbiddenScreen), Email);
                    return;
                }

                if (await RolePermissionUtils.CheckPermission(PermissionName.Orders, PermissionAccess.MOBILE_FULFILL))
                {
                    await _notificationService.RegisterDeviceAsync();
                }

                var hasLocationPermission = await _permissionService.RequestPermissionAsync(PermissionConstants.LOCATION_PERMISSION);
                if (!hasLocationPermission)
                {
                    await _navigationService.PushModalAsync<NoLocationAccess>();
                }
                else
                {
                    await _navigationService.NavigateToAsync(typeof(HamburgerMenu), true);
                }


            }
            catch (BadRequestException ex)
            {
                _toastMessageService.ShowToast(ex.Message);
            }
            catch(Exception ex)
            {
                ReportCrash(ex);
                _toastMessageService.ShowToast("Not able to complete the request at the moment. Please try again");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ShowErrorMessage(string errorMessage)
        {
            ErrorMessage = errorMessage;
            ShouldShowError = true;
        }

        private async void NavigateToResetPassword()
        {
            try
            {
                IsLoading = true;

                var otpRequestResponse = await _resetPasswordService.SendOtpRequestAsync(Email);
                if (!otpRequestResponse)
                {
                    _toastMessageService.ShowToast("OTP request failed");
                    return;
                }
                await _navigationService.NavigateToAsync<PasswordResetScreen>(Email);
            }
            catch (BadRequestException ex)
            {
                _toastMessageService.ShowToast(ex.Message);
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                _toastMessageService.ShowToast("Not able to complete the request at the moment. Please try again");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}