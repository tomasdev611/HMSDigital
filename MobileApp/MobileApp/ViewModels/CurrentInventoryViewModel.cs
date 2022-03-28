using MobileApp.Interface;
using MobileApp.Models;
using MobileApp.Service;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    class CurrentInventoryViewModel : BaseViewModel
    {
        private readonly InventoryService _inventoryService;

        public CurrentInventoryViewModel() : base()
        {
            _inventoryService = new InventoryService();
            LoadCurrentInventory();
        }

        ICommand _refreshInventoryCommand;
        public ICommand RefreshInventoryCommand
        {
            get
            {
                return _refreshInventoryCommand ?? (_refreshInventoryCommand = new Command(LoadCurrentInventory));
            }
        }

        ObservableCollection<ListGroup<Inventory>> _inventoryGroupList;

        public ObservableCollection<ListGroup<Inventory>> InventoryGroupList
        {
            get
            {
                return _inventoryGroupList;
            }
            set
            {
                _inventoryGroupList = value;
                OnPropertyChanged();
            }
        }

        bool _isRefreshing = true;

        public bool IsInventoryRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        protected async void LoadCurrentInventory()
        {
            try
            {
                var inventoryList = await _inventoryService.GetCurrentInventoryAsync();
                if (inventoryList != null)
                {
                    var inventoryGroupByItems = inventoryList.Where(i => i.Count != 0).GroupBy(i => i.ItemId);
                    var groupList = new ObservableCollection<ListGroup<Inventory>>();

                    foreach (var group in inventoryGroupByItems)
                    {
                        var groupedList = group.AsEnumerable<Inventory>().ToList();
                        var Name = group.FirstOrDefault().Item.Name;
                        var count = groupedList.Sum(gl => gl.Count);

                        groupList.Add(new ListGroup<Inventory>(
                            Name,
                            groupedList,
                            count
                        ));
                    }
                    InventoryGroupList = groupList;
                }
            }
            catch(Exception ex)
            {
                ReportCrash(ex);
                return;
            }
            finally
            {
                IsInventoryRefreshing = false;
            }
        }

    }
}
