using HMSDigital.Common.ViewModels;
using HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.JsonPatch;
using Sieve.Models;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface IItemCategoriesService
    {
        Task<PaginatedList<ItemCategory>> GetAllItemCategories(SieveModel sieveModel);

        Task<PaginatedList<ItemCategory>> SearchItemCategories(SieveModel sieveModel, string searchQuery);

        Task<ItemCategory> GetItemCategoryById(int categoryId);

        Task<ItemCategory> CreateItemCategory(ItemCategoryRequest itemCategoryRequest);

        Task<ItemCategory> PatchItemCategory(int categoryId, JsonPatchDocument<ItemCategory> itemCategoryPatchDocument);

        Task DeleteItemCategory(int categoryId);
    }
}
