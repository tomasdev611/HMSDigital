using MobileApp.Assets.Constants;
using MobileApp.DataBaseAttributes;
using MobileApp.Interface;
using MobileApp.Service;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    class UndoScanPopupViewModel : BaseViewModel
    {
        private readonly DatabaseService<ScanItem> _databaseService;

        public UndoScanPopupViewModel() : base()
        {
            _databaseService = new DatabaseService<ScanItem>();
        }

        public override void InitializeViewModel(object data = null)
        {
            ScannedItem = (ScanItem)data;
        }

        private ScanItem _scannedItem;

        public ScanItem ScannedItem
        {
            get
            {
                return _scannedItem;
            }
            set
            {
                _scannedItem = value;
                OnPropertyChanged();
            }
        }

        public ICommand _undoCommand;

        public ICommand UndoCommand
        {
            get
            {
                return _undoCommand ?? (_undoCommand = new Command(Undo));
            }
        }

        public ICommand _cancelCommand;

        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new Command(Cancel));
            }
        }

        private async void Undo()
        {
            await _databaseService.DeleteAsync(ScannedItem);
            MessagingCenter.Send<string>(string.Empty, MessagingConstant.UNDO_SCANNED_INVENTORY);
            await _navigationService.PopPopupAsync();
        }

        private async void Cancel()
        {
            await _navigationService.PopPopupAsync();
        }
    }
}
