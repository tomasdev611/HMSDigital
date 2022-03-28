using MobileApp.Assets.Constants;
using MobileApp.Assets.Enums;
using System.Runtime.CompilerServices;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Pages.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderTypeLabel : ContentView
    {

        public static readonly BindableProperty OrderTypeIdProperty = BindableProperty.Create(nameof(OrderTypeId), typeof(int), typeof(OrderTypeLabel), 0);


        public int OrderTypeId
        {
            get
            {
                return (int)GetValue(OrderTypeIdProperty);
            }
            set
            {
                SetValue(OrderTypeIdProperty, value);
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if(string.Equals(propertyName, "OrderTypeId"))
            {
                SetOrderType();
            }
            base.OnPropertyChanged(propertyName);
        }

        public OrderTypeLabel()
        {
            InitializeComponent();
            orderTypeLabel.BindingContext = this;
        }

        private Color _labelColor;

        public Color LabelColor
        {
            get
            {
                return _labelColor;
            }
            set
            {
                _labelColor = value;
                OnPropertyChanged();
            }
        }

        private string _orderType;

        public string OrderType
        {
            get
            {
                return _orderType;
            }
            set
            {
                _orderType = value;
                OnPropertyChanged();
            }
        }

        private void SetOrderType()
        {
            switch (OrderTypeId)
            {
                case (int)OrderTypes.Delivery:
                    OrderType = "Delivery";
                    LabelColor = Color.FromHex(OrderTypeColors.Delivery);
                    break;
                case (int)OrderTypes.Pickup:
                    OrderType = "Pickup";
                    LabelColor = Color.FromHex(OrderTypeColors.Pickup);
                    break;
                case (int)OrderTypes.Patient_Move:
                    OrderType = "Move";
                    LabelColor = Color.FromHex(OrderTypeColors.Move);
                    break;
                case (int)OrderTypes.Exchange:
                    OrderType = "Exchange";
                    LabelColor = Color.FromHex(OrderTypeColors.Exchange);
                    break;
            }
        }
    }
}