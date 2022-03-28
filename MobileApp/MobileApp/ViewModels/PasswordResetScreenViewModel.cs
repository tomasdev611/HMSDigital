using System;
using System.Windows.Input;
using MobileApp.Assets.Constants;
using MobileApp.Exceptions;
using MobileApp.Models;
using MobileApp.Pages.Screen;
using MobileApp.Service;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class PasswordResetScreenViewModel : BaseViewModel
    {
        private readonly ResetPasswordService _resetPasswordService;

        public PasswordResetScreenViewModel() : base()
        {
            _resetPasswordService = new ResetPasswordService();

            GetImageSource();
        }

        public override void InitializeViewModel(object data = null)
        {
            Email = (string)data;
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

        private string _otp;

        public string Otp
        {
            get
            {
                return _otp;
            }
            set
            {
                _otp = value;
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

        private string _confirmPassword;

        public string ConfirmPassword
        {
            get
            {
                return _confirmPassword;
            }
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }

        private bool _isEntrySecure = true;

        public bool IsEntrySecure
        {
            get
            {
                return _isEntrySecure;
            }
            set
            {
                _isEntrySecure = value;
                OnPropertyChanged();
            }
        }

        private bool _isValid;

        public bool IsValid
        {
            get
            {
                return _isValid;
            }
            set
            {
                _isValid = value;
                OnPropertyChanged();
            }
        }

        private bool _isPasswordRuleVisible;

        public bool IsPasswordRuleVisible
        {
            get
            {
                return _isPasswordRuleVisible;
            }
            set
            {
                _isPasswordRuleVisible = value;
                OnPropertyChanged();
            }
        }

        private ImageSource _passwordVisibileImage;

        public ImageSource PasswordVisibleImage
        {
            get
            {
                return _passwordVisibileImage;
            }
            set
            {
                _passwordVisibileImage = value;
                OnPropertyChanged();
            }
        }

        private PasswordValidationState _passwordValidationState;

        public PasswordValidationState PasswordValidationState
        {
            get
            {
                return _passwordValidationState;
            }
            set
            {
                _passwordValidationState = value;
                OnPropertyChanged();
            }
        }

        private ICommand _togglePasswordVisibilityCommand;

        public ICommand TogglePasswordVisibilityCommand
        {
            get
            {
                return _togglePasswordVisibilityCommand ?? (_togglePasswordVisibilityCommand = new Command(TogglePasswordVisibility));
            }
        }

        private ICommand _resetPasswordCommand;

        public ICommand ResetPasswordCommand
        {
            get
            {
                return _resetPasswordCommand ?? (_resetPasswordCommand = new Command(ResetPassword));
            }
        }

        private ICommand _resendOtpCommand;

        public ICommand ResendOtpCommand
        {
            get
            {
                return _resendOtpCommand ?? (_resendOtpCommand = new Command(ResendOtp));
            }
        }

        private async void ResendOtp()
        {
            try
            {
                IsLoading = true;
                var response = await _resetPasswordService.SendOtpRequestAsync(Email);

                _toastMessageService.ShowToast("OTP resend", ToastMessageDuration.Long);
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

        private async void ResetPassword()
        {
            try
            {
                IsLoading = true;
                if (string.IsNullOrEmpty(Otp) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
                {
                    _toastMessageService.ShowToast("Invalid value");
                }
                var response = await _resetPasswordService.ResetPasswordAsync(new Models.ResetPassword
                {
                    Email = Email,
                    OTP = Otp,
                    Password = Password
                });

                _toastMessageService.ShowToast("Password Reset Successful", ToastMessageDuration.Long);
                await _navigationService.NavigateToAsync<LoginScreen>();
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

        private void TogglePasswordVisibility()
        {
            IsEntrySecure = !IsEntrySecure;
            GetImageSource();
        }

        private void GetImageSource()
        {
            if (IsEntrySecure)
            {
                PasswordVisibleImage = ImageSource.FromResource(AppConstants.IMAGE_PATH + "showPassword.png");
            }
            else
            {
                PasswordVisibleImage = ImageSource.FromResource(AppConstants.IMAGE_PATH + "hidePassword.png");
            }
        }
    }
}
