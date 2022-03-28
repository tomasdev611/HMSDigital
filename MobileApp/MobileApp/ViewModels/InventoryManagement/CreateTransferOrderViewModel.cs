using System;
using MobileApp.Interface.Services;
using MobileApp.Models;
using MobileApp.Service;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using MobileApp.Pages.Screen.PurchaseOrder;
using MobileApp.Utils;
using System.Collections.Generic;
using MvvmHelpers;

namespace MobileApp.ViewModels.InventoryManagement
{
    public class CreateTransferOrderViewModel : BaseViewModel
    {
        private bool _isEmptyView = false;
        private DateTime _currentDate = DateTime.Now.Date;
        private TransferOrder _currentTransferOrder;

        private ICommand _closeSheetCommand;

        private readonly IInventoryService _inventoryService;

        /// <summary>
        /// Validate if the View is Empty
        /// </summary>
        public bool IsEmptyView
        {
            get
            {
                return _isEmptyView;
            }
            set
            {
                _isEmptyView = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Validate if the View is Empty
        /// </summary>
        public DateTime CurrentDate
        {
            get
            {
                return _currentDate;
            }
            set
            {
                _currentDate = value;
                OnPropertyChanged();
            }
        }

        public TransferOrder CurrentTransferOrder
        {
            get
            {
                return _currentTransferOrder;
            }
            set
            {
                _currentTransferOrder = value;
                OnPropertyChanged();
            }
        }

        public CreateTransferOrderViewModel() : base()
        {
            _inventoryService = new InventoryManagementService();
            PageTitle = "Create Transfer Order";
        }
    }
}