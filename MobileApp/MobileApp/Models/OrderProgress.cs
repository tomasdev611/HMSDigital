using Xamarin.Forms;

namespace MobileApp.Models
{
    public class OrderProgress : BindableObject
    {
        private int _deliveryOrdersCount;
        private int _deliveryItemsCount;
        private int _pickupOrdersCount;
        private int _pickupItemsCount;
        private int _completedOrdersCount;
        private int _completedItemsCount;

        public int DeliveryOrdersCount
        {
            get
            {
                return _deliveryOrdersCount;
            }
            set
            {
                _deliveryOrdersCount = value;
                OnPropertyChanged();
            }
        }

        public int DeliveryItemsCount
        {
            get
            {
                return _deliveryItemsCount;
            }
            set
            {
                _deliveryItemsCount = value;
                OnPropertyChanged();
            }
        }
        
        public int PickupOrdersCount
        {
            get
            {
                return _pickupOrdersCount;
            }
            set
            {
                _pickupOrdersCount = value;
                OnPropertyChanged();
            }
        }

        public int PickupItemsCount
        {
            get
            {
                return _pickupItemsCount;
            }
            set
            {
                _pickupItemsCount = value;
                OnPropertyChanged();
            }
        }
        
        public int CompletedOrdersCount
        {
            get
            {
                return _completedOrdersCount;
            }
            set
            {
                _completedOrdersCount = value;
                OnPropertyChanged();
            }
        }

        public int CompletedItemsCount
        {
            get
            {
                return _completedItemsCount;
            }
            set
            {
                _completedItemsCount = value;
                OnPropertyChanged();
            }
        }
    }
}
