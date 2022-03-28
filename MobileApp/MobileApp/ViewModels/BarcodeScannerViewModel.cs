using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MobileApp.Assets.Constants;
using MobileApp.Interface;
using MobileApp.Models;
using MobileApp.Service;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    class BarcodeScannerViewModel : BaseViewModel
    {
        private readonly InventoryService _inventoryService;

        private readonly IScanner _sledScanner;

        public BarcodeScannerViewModel() : base()
        {
            _inventoryService = new InventoryService();
            _sledScanner = DependencyService.Get<IScanner>();

            SubscribeToSledEvents();
            CheckSledScanner();
        }

        private void SubscribeToSledEvents()
        {
            MessagingCenter.Subscribe<string>(this, MessagingConstant.CONNECTION_ERROR, (scanner) =>
            {
                ShouldEnableCamScanner = true;
                _toastMessageService.ShowToast("Failed to connect scanner", ToastMessageDuration.Long);
                return;
            });

            MessagingCenter.Subscribe<string>(this, MessagingConstant.BARCODE_SCANNED, async (scannedCode) =>
            {
                await BarcodeScanned(scannedCode);
                return;
            });
        }

        private void CheckSledScanner()
        {
            if (_sledScanner != null && _sledScanner.StartDecoder())
            {
                ShouldEnableCamScanner = false;
            }
        }

        public override void DestroyViewModel()
        {
            MessagingCenter.Unsubscribe<string>(this, MessagingConstant.CONNECTION_ERROR);
            MessagingCenter.Unsubscribe<string>(this, MessagingConstant.BARCODE_SCANNED);

            _sledScanner?.StopDecoder();
        }

        bool _isLoading;
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

        private bool _shouldEnableCamScanner = true;

        public bool ShouldEnableCamScanner
        {
            get
            {
                return _shouldEnableCamScanner;
            }
            set
            {
                _shouldEnableCamScanner = value;
                OnPropertyChanged();
            }
        }

        string _errorMessage;
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

        Inventory _inventory;
        public Inventory InventoryDetails
        {
            get
            {
                return _inventory;
            }
            set
            {
                _inventory = value;
                OnPropertyChanged();
            }
        }

        string _scannedTag;
        public string ScannedTag
        {
            get
            {
                return _scannedTag;
            }
            set
            {
                _scannedTag = value;
                OnPropertyChanged();
            }
        }

        private async Task<Inventory> GetInventoryForScannedTag()
        {
            try
            {
                var filterString = $"(serialNumber | assetTagNumber| lotNumber) == {ScannedTag}";
                return (await _inventoryService.GetInventory(filterString)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                _toastMessageService.ShowToast("Error occured while fetching the inventory details");
                return null;
            }
        }

        private async Task BarcodeScanned(string scanningResult)
        {
            try
            {
                ScannedTag = scanningResult;

                if (ScannedTag == null)
                {
                    return;
                }

                IsLoading = true;
                InventoryDetails = await GetInventoryForScannedTag();

                if (InventoryDetails == null)
                {
                    ShowError("Scanned item does not match any inventory item");
                    return;
                }
                IsLoading = false;
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                _toastMessageService.ShowToast("Error occured while fetching the inventory details");
            }
        }

        private void ShowError(string errorMessage)
        {
            ErrorMessage = $"Inventory not scanned. Try again";
            IsLoading = false;
        }

        ICommand _onDeviceScanCompleteCommand;

        public ICommand OnDeviceScanCompleteCommand
        {
            get
            {
                return _onDeviceScanCompleteCommand ?? (_onDeviceScanCompleteCommand = new Command(OnDeviceScanComplete));
            }
        }

        ICommand _finishScanningCommand;

        public ICommand FinishScanningCommand
        {
            get
            {
                return _finishScanningCommand ?? (_finishScanningCommand = new Command(FinishScanning));
            }
        }

        public async void OnDeviceScanComplete(object scanningResult)
        {
            await BarcodeScanned((scanningResult as ZXing.Result).Text);
            return;
        }

        public async void FinishScanning()
        {
            await _navigationService.NavigateBackAsync();
            if (InventoryDetails != null)
            {
                MessagingCenter.Send<Inventory>(InventoryDetails, MessagingConstant.SCANNED_INVENTORY);
            }
        }
    }
}