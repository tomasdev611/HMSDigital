using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MobileApp.Models;

namespace MobileApp.Utils
{
    public static class LoadListUtils
    {
        public static LoadListCounts GetSiteLoadListCounts(SiteLoadList siteLoadList)
        {
            if (siteLoadList == null)
            {
                return null;
            }
            return new LoadListCounts()
            {
                ProductsCount = siteLoadList.TotalItemCount,
                ItemsCount = GetLoadList(siteLoadList.Loadlists).Sum(i => i.Quantity),
                OrdersCount = siteLoadList.TotalOrderCount,
                TrucksCount = siteLoadList.Loadlists != null ? siteLoadList.Loadlists.Count() : 0
            };
        }

        public static ObservableCollection<ItemsList> GetLoadList(IEnumerable<VehicleLoadlist> vehicleLoadLists)
        {
            if(vehicleLoadLists == null)
            {
                return new ObservableCollection<ItemsList>();
            }
            var loadList = vehicleLoadLists.Select(vehicleLoadList => RemoveFulfilledItem(vehicleLoadList));

            var distictItemNameList = loadList.SelectMany(loads => loads.Items.Select(i => i.Item.Name)).Distinct();
            var loadListItems = loadList.SelectMany(loads => loads.Items);

            return new ObservableCollection<ItemsList>(distictItemNameList.Select(it => new ItemsList()
            {
                ItemName = it,
                Quantity = loadListItems.Count(i => i.Item.Name == it)
            }));
        }

        public static VehicleLoadlist RemoveFulfilledItem(VehicleLoadlist vehicleLoadlist)
        {
            var loadListItems = vehicleLoadlist.Items;

            var fulfilledItems = vehicleLoadlist.Orders.SelectMany(o => o.OrderFulfillmentLineItems).GroupBy(i => i.ItemName).Select(l =>
                                                            new ItemsList()
                                                            {
                                                                ItemName = l.Key,
                                                                Quantity = l.Sum(lg => lg.Quantity)
                                                            });

            foreach (var loadListItem in loadListItems)
            {
                var fulfilledItem = fulfilledItems.FirstOrDefault(fi => fi.ItemName == loadListItem.Item.Name);

                if (fulfilledItem != null)
                {
                    loadListItem.ItemCount -= fulfilledItem.Quantity;
                }
            }

            vehicleLoadlist.Items = loadListItems.Where(i => i.ItemCount > 0);
            return vehicleLoadlist;
        }
    }
}
