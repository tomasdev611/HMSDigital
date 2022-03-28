using MobileApp.Models;
using MobileApp.ViewModels.PurchaseOrder;
using Xamarin.Forms;

namespace MobileApp.Pages.Screen.PurchaseOrder
{
    public partial class QuantityItemPage : ContentPage
    {
        #region Private Properties

        private QuantityItemViewModel _vm;
        private PurchaseOrderLineItemModel _selectedLineItem;

        #endregion

        public QuantityItemPage()
        {
            InitializeComponent();
            _vm = new QuantityItemViewModel();
            this.BindingContext = _vm;
        }

        public QuantityItemPage(PurchaseOrderLineItemModel lineItem)
        {
            _vm = new QuantityItemViewModel();
            _selectedLineItem = lineItem;
            InitializeComponent();
            this.BindingContext = _vm;
        }

        protected override void OnAppearing()
        {
            _vm.InitializeViewModel(_selectedLineItem);
            quantityEntry.Focus();
        }
    }
}
