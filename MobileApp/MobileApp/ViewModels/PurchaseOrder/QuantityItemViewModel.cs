using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MobileApp.Interface.Services;
using MobileApp.Models;
using MobileApp.Service;
using System.Linq;
using MobileApp.Utils;
using System.Windows.Input;
using Xamarin.Forms;
using MvvmHelpers;
using System.Collections.Generic;
using MobileApp.Pages.Screen.PurchaseOrder;

namespace MobileApp.ViewModels.PurchaseOrder 
{
    public class QuantityItemViewModel : BaseViewModel
    {
        #region Private Properties

        private readonly IPurchaseOrderService _purchaseOrderService;
        private PurchaseOrderLineItemModel _selectedLineItemModel;

        private bool _isRefreshing = true;
        private string _quantityReceived = "";

        private ICommand _closeCommand;
        private ICommand _doneCommand;

        #endregion

        /// <summary>
        /// Is Refreshing the View
        /// </summary>
        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Is Refreshing the View
        /// </summary>
        public string QuantityReceived
        {
            get
            {
                return _quantityReceived;
            }
            set
            {
                _quantityReceived = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Selected LineItemModel
        /// </summary>
        public PurchaseOrderLineItemModel SelectedLineItemModel
        {
            get
            {
                return _selectedLineItemModel;
            }
            set
            {
                _selectedLineItemModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Get the Purchase Orders
        /// </summary>
        public ICommand CloseSheetCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new Command(CloseCommandExecute));
            }
        }

        /// <summary>
        /// Get the Purchase Orders
        /// </summary>
        public ICommand DoneCommand
        {
            get
            {
                return _doneCommand ?? (_doneCommand = new Command(DoneCommandExecute));
            }
        }

        public QuantityItemViewModel() : base()
        {
            _purchaseOrderService = new InventoryManagementService();
            PageTitle = "Receive Quantity Based Items";
        }

        public override void InitializeViewModel(object data = null)
        {
            var lineItem = (PurchaseOrderLineItemModel)data;
            SelectedLineItemModel = lineItem;
            if(lineItem.QuantityRecieved > 0)
            {
                QuantityReceived = lineItem.QuantityRecieved.ToString();
            }
        }

        private async void CloseCommandExecute(object obj)
        {
            if(String.IsNullOrEmpty(QuantityReceived))
            {
                await _navigationService.NavigateBackAsync();
            }
            else
            {
                _toastMessageService.ShowToast("Please enter quantity received");
            }
        }

        private async void DoneCommandExecute(object obj)
        {
            if (String.IsNullOrEmpty(QuantityReceived))
            {
                _toastMessageService.ShowToast("Please enter quantity received");
                return;
            }

            try
            {
                if(Convert.ToInt32(QuantityReceived) > SelectedLineItemModel.Quantity)
                {
                    _toastMessageService.ShowToast("Cannot receive more items than you have");
                }

                SelectedLineItemModel.QuantityRecieved = Convert.ToInt32(QuantityReceived);

                this.NavigationBackParameter = SelectedLineItemModel;

                await _navigationService.NavigateBackAsync();
            }
            catch (Exception ex)
            {
                ReportCrash(ex);
            }
        }
    }
}
