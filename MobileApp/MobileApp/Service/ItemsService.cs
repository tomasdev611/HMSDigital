using MobileApp.Exceptions;
using MobileApp.Interface;
using MobileApp.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.Service
{
    class ItemsService
    {
        private readonly IItemsApi _itemApi;
        public ItemsService()
        {
            _itemApi = RestService.For<IItemsApi>(HMSHttpClientFactory.GetCoreHttpClient());
        }
        public async Task<IEnumerable<Item>> GetItemsList(string filter = null)
        {
            try
            {
                var itemsResponse = await _itemApi.GetItems(filter);
                if(itemsResponse.Content != null)
                {
                    return itemsResponse.Content.Records;
                }
                return null;
            }
            catch
            {
                throw;
            }
        }
    }
}
