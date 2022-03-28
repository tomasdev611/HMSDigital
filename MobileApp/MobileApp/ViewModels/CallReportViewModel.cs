using MobileApp.Methods;
using MobileApp.Service;
using MobileApp.Utils;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class CallReportViewModel : BaseViewModel
    {
        private readonly Dialer _dialer;

        public CallReportViewModel() : base()
        {
            _dialer = new Dialer();
        }

        private string _callingNumber;

        public string CallingNumber
        {
            get
            {
                return _callingNumber;
            }
            set
            {
                _callingNumber = value;
                OnPropertyChanged();
            }
        }

        public override void InitializeViewModel(object data = null)
        {
            CallingNumber = data.ToString();
        }

        private ICommand _callSupportCommand;

        public ICommand CallSupportCommand
        {
            get
            {
                return _callSupportCommand ?? (_callSupportCommand = new Command(CallSupport));
            }
        }

        private async void CallSupport()
        {
            try
            {
                var currentSiteId = await UserDetailsUtils.GetUsersCurrentSiteId();

                var siteDetails = await CacheManager.GetSiteDetailByIdFromCache(currentSiteId ?? 0);
                _dialer.OpenDialerWithNumber(siteDetails.SitePhoneNumber.FirstOrDefault().PhoneNumber.Number.ToString());
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                _toastMessageService.ShowToast("Error occured while getting site contact information");
                return;
            }
        }

        private ICommand _proceedToDeliveryCommand;

        public ICommand ProceedToDeliveryCommand
        {
            get
            {
                return _proceedToDeliveryCommand ?? (_proceedToDeliveryCommand = new Command(ProceedToDelivery));
            }
        }

        private async void ProceedToDelivery()
        {
            await _navigationService.PopPopupAsync();
        }
    }
}
