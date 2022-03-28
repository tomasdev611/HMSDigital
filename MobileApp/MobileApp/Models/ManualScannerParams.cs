using Xamarin.Forms;

namespace MobileApp.Models
{
    public class ManualScannerParams : BindableObject
    {

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

        private bool _isStandalone;

        public bool IsStandalone
        {
            get
            {
                return _isStandalone;
            }
            set
            {
                _isStandalone = value;
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

        private string _patientUuid;

        public string PatientUuid
        {
            get
            {
                return _patientUuid;
            }
            set
            {
                _patientUuid = value;
                OnPropertyChanged();
            }
        }
    }
}
