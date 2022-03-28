using System.Collections.ObjectModel;
using MobileApp.Models;
using MobileApp.ViewModels;
using Xamarin.Forms;

namespace MobileApp.Pages.CommonPages
{
    public partial class ItemsListView : ContentPage
    {
        private ObservableCollection<ItemsList> _itemsList;

        private readonly BaseViewModel _viewModel;

        public ItemsListView(ObservableCollection<ItemsList> itemsList)
        {
            _itemsList = itemsList;
            _viewModel = new ItemsListViewModel();
            this.BindingContext = _viewModel;

            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            PagesUtils.PageNavigationFollowUp(nameof(ItemsListView));
            _viewModel.InitializeViewModel(_itemsList);
        }
    }
}
