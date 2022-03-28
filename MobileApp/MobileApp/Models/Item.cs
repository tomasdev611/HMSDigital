using Xamarin.Forms;

namespace MobileApp.Models
{
    public class Item : BindableObject
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

        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _itemNumber;

        public string ItemNumber
        {
            get
            {
                return _itemNumber;
            }
            set
            {
                _itemNumber = value;
                OnPropertyChanged();
            }
        }

        private bool _isSerialized;

        public bool IsSerialized
        {
            get
            {
                return _isSerialized;
            }
            set
            {
                _isSerialized = value;
                OnPropertyChanged();
            }
        }

        private bool _isAssetTagged;

        public bool IsAssetTagged
        {
            get
            {
                return _isAssetTagged;
            }
            set
            {
                _isAssetTagged = value;
                OnPropertyChanged();
            }
        }

        private bool _isLotNumbered;

        public bool IsLotNumbered
        {
            get
            {
                return _isLotNumbered;
            }
            set
            {
                _isLotNumbered = value;
                OnPropertyChanged();
            }
        }

        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }
    }
}
