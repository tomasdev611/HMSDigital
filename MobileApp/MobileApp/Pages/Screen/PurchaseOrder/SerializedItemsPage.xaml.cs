using MobileApp.Models;
using MobileApp.ViewModels.PurchaseOrder;
using Xamarin.Forms;

namespace MobileApp.Pages.Screen.PurchaseOrder
{
    public partial class SerializedItemsPage : ContentPage
    {
        #region Private Properties

        private SerializedItemsViewModel _vm;
        private PurchaseOrderLineItemModel _selectedLineItem;

        #endregion

        public SerializedItemsPage()
        {
            InitializeComponent();
            _vm = new SerializedItemsViewModel();
            this.BindingContext = _vm;
        }

        public SerializedItemsPage(PurchaseOrderLineItemModel lineItem)
        {
            _vm = new SerializedItemsViewModel();
            _selectedLineItem = lineItem;
            InitializeComponent();
            this.BindingContext = _vm;
        }

        protected override void OnAppearing()
        {
            _vm.InitializeViewModel(_selectedLineItem);
        }
    }
}