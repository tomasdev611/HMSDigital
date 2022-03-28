using MobileApp.Assets.Constants;
using MobileApp.Assets.Enums;
using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    class OrderListManulScannerViewModel : ManualScannerViewModel
    {

        public OrderListManulScannerViewModel() : base()
        {
        }

        public override void InitializeViewModel(object data = null)
        {
            ItemToScan = (ScannableOrderList)data;
        }

        private ScannableOrderList _itemToScan;

        new public ScannableOrderList ItemToScan
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

        protected async override void AddManually()
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
                    var quantityNeeded = _itemToScan.QuantityToFulfill - quantityAlreadyScanned;
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
            if (_itemToScan.DispatchTypeId == (int)OrderTypes.Pickup)
            {
                return (await _inventoryService.GetPatientInventory(ItemToScan.PatientUuId.ToString(), filterString)).FirstOrDefault();
            }
            return (await _inventoryService.GetInventory(filterString)).FirstOrDefault();
        }

    }
}
