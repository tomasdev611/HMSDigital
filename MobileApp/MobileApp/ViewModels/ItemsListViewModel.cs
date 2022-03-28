using System.Collections.ObjectModel;
using MobileApp.Models;

namespace MobileApp.ViewModels
{
    public class ItemsListViewModel : BaseViewModel
    {

        public ItemsListViewModel() : base()
        {

        }

        public override void InitializeViewModel(object data = null)
        {
            ItemsList = (ObservableCollection<ItemsList>)data;
        }

        private ObservableCollection<ItemsList> _itemsList;

        public ObservableCollection<ItemsList> ItemsList
        {
            get
            {
                return _itemsList;
            }
            set
            {
                _itemsList = value;
                OnPropertyChanged();
            }
        }

        private bool _isLoading;

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = true;
                OnPropertyChanged();
            }
        }
    }
}
