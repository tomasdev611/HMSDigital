using System;
using Xamarin.Forms;

namespace MobileApp.Models
{
    public class LoadListCounts : BindableObject
    {
        private int _productsCount;
        private int _itemsCount;
        private int _ordersCount;
        private int _trucksCount;
        public int ProductsCount
        {
            get
            {
                return _productsCount;
            }
            set
            {
                _productsCount = value;
                OnPropertyChanged();
            }
        }
        public int ItemsCount
        {
            get
            {
                return _itemsCount;
            }
            set
            {
                _itemsCount = value;
                OnPropertyChanged();
            }
        }
        public int OrdersCount
        {
            get
            {
                return _ordersCount;
            }
            set
            {
                _ordersCount = value;
                OnPropertyChanged();
            }
        }
        public int TrucksCount
        {
            get
            {
                return _trucksCount;
            }
            set
            {
                _trucksCount = value;
                OnPropertyChanged();
            }
        }
    }
}
