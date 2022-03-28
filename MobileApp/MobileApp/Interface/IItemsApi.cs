using MobileApp.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.Interface
{
    interface IItemsApi
    {
        [Get("/api/items")]
        Task<ApiResponse<PaginatedList<Item>>> GetItems(string filters);
    }
}
