using System;
using Xamarin.Forms;

namespace MobileApp.Models
{
    public class Inventory : BindableObject
    {
        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private Item _item;
        public Item Item {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
                OnPropertyChanged();
            }
        }

        private int _statusId;

        public int StatusId
        {
            get
            {
                return _statusId;
            }
            set
            {
                _statusId = value;
                OnPropertyChanged();
            }
        }

        private string _status;
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        private string _currentLocationType;
        public string CurrentLocationType
        {
            get
            {
                return _currentLocationType;
            }
            set
            {
                _currentLocationType = value;
                OnPropertyChanged();
            }
        }

        private string _lotNumber;
        public string LotNumber
        {
            get
            {
                return _lotNumber;
            }
            set
            {
                _lotNumber = value;
                OnPropertyChanged();
            }
        }

        private string _assetTagNumber;

        public string AssetTagNumber
        {
            get
            {
                return _assetTagNumber;
            }
            set
            {
                _assetTagNumber = value;
                OnPropertyChanged();
            }
        }

        private int _itemId;

        public int ItemId
        {
            get
            {
                return _itemId;
            }
            set
            {
                _itemId = value;
                OnPropertyChanged();
            }
        }

        private string _serialNumber;

        public string SerialNumber
        {
            get
            {
                return _serialNumber;
            }
            set
            {
                _serialNumber = value;
                OnPropertyChanged();
            }
        }

        private int _count;
        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }

        private int _currentLocationTypeId;
        public int CurrentLocationTypeId
        {
            get
            {
                return _currentLocationTypeId;
            }
            set
            {
                _currentLocationTypeId = value;
                OnPropertyChanged();
            }
        }

        private int? _currentLocationId;
        public int? CurrentLocationId
        {
            get
            {
                return _currentLocationId;
            }
            set
            {
                _currentLocationId = value;
                OnPropertyChanged();
            }
        }
    }
}
