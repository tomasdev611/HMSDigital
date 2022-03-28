using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MobileApp.Assets.Constants;
using MobileApp.Models;
using MobileApp.Service;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class ManualScannerViewModel : BaseViewModel
    {
        protected readonly InventoryService _inventoryService;

        public ManualScannerViewModel() : base()
        {
            _inventoryService = new InventoryService();
            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                ErrorMessage = "";
            }
        }

        public override void InitializeViewModel(object data = null)
        {
            ItemToScan = (ScannableLoadListGroup)data;
        }

        private ScannableLoadListGroup _itemToScan;

        public ScannableLoadListGroup ItemToScan
        {
            get
            {
                return _itemToScan;
            }
            set
            {
                _itemToScan = value;
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

        bool _isEnabled = true;
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        string _userInput;
        public string UserInput
        {
            get
            {
                return _userInput;
            }
            set
            {
                _userInput = value;
                if (!string.IsNullOrWhiteSpace(ErrorMessage))
                {
                    ErrorMessage = "";
                }
                OnPropertyChanged();
            }
        }

        private ICommand _addManuallyCommand;

        public ICommand AddManuallyCommand
        {
            get
            {
                return _addManuallyCommand ?? (_addManuallyCommand = new Command(AddManually));
            }
        }

        protected async virtual void AddManually()
        {
            var filterString = GetInventoryFilterString();

            if (string.IsNullOrEmpty(filterString))
            {
                return;
            }

            IsEnabled = false;
            IsLoading = true;

            try
            {
                var inventory = await GetInventory(filterString);
                if (inventory == null)
                {
                    ErrorMessage = "No inventory found for this tag";
                    return;
                }
                if (ItemToScan.IsStandAlone)
                {
                    inventory.Count = int.Parse(UserInput);
                }
                await _navigationService.PopPopupAsync();
                MessagingCenter.Send<Inventory>(inventory, MessagingConstant.SCANNED_INVENTORY);
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
                ErrorMessage = "Some Issue fetching inventory";
            }
            finally
            {
                IsEnabled = true;
                IsLoading = false;
            }
        }

        private string GetInventoryFilterString()
        {
            if (string.IsNullOrEmpty(UserInput))
            {
                ErrorMessage = "Please provide tag or quantity for item";
                return null;
            }
            if (ItemToScan.IsStandAlone)
            {
                if (int.TryParse(UserInput, out int quantity))
                {
                    var quantityAlreadyScanned = _itemToScan.Where(its => !its.IsCompleted).Sum(i => i.QuantityScanned);
                    var quantityNeeded = _itemToScan.QuantityToLoad - quantityAlreadyScanned;
                    if (quantity <= quantityNeeded)
                    {
                        return $"itemId == {ItemToScan.ItemId}";
                    }
                }
                ErrorMessage = "Please provide valid quantity";
                return null;
            }
            else
            {
                return $"(serialNumber | assetTagNumber | lotNumber) == {UserInput}";
            }
        }

        private async Task<Inventory> GetInventory(string filterString)
        {
            return (await _inventoryService.GetInventory(filterString)).FirstOrDefault();
        }

        private ICommand _cancelCommand;

        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new Command(Cancel));
            }
        }

        private async void Cancel()
        {
            await _navigationService.PopPopupAsync();
        }
    }
}
