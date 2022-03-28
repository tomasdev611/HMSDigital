using MobileApp.Models;
using MobileApp.ViewModels.PurchaseOrder;
using Xamarin.Forms;

namespace MobileApp.Pages.Screen.PurchaseOrder
{
    public partial class POReceiptPage : ContentPage
    {
        #region Private Properties

        private POReceiptViewModel _vm;
        private PurchaseOrderModel _purchaseOrder;

        #endregion

        public POReceiptPage()
        {
            InitializeComponent();
            _vm = new POReceiptViewModel();
            this.BindingContext = _vm;
        }

        public POReceiptPage(PurchaseOrderModel purchaseOrder)
        {
            _vm = new POReceiptViewModel();
            _purchaseOrder = purchaseOrder;
            InitializeComponent();
            this.BindingContext = _vm;
        }

        protected override void OnAppearing()
        {
            _vm.InitializeViewModel(_purchaseOrder);
        }
    }
}