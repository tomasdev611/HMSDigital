using AutoMapper;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.JsonPatch;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public class ItemCategoriesService : IItemCategoriesService
    {
        private readonly IItemCategoryRepository _itemCategoryRepository;

        private readonly IItemRepository _itemRepository;

        private readonly IPaginationService _paginationService;

        private readonly IMapper _mapper;

        public ItemCategoriesService(IItemCategoryRepository itemCategoryRepository,
            IItemRepository itemRepository,
            IPaginationService paginationService,
            IMapper mapper)
        {
            _itemCategoryRepository = itemCategoryRepository;
            _itemRepository = itemRepository;
            _mapper = mapper;
            _paginationService = paginationService;
        }

        public async Task<PaginatedList<ItemCategory>> GetAllItemCategories(SieveModel sieveModel)
        {
            _itemCategoryRepository.SieveModel = sieveModel;
            var itemCategoriesCount = await _itemCategoryRepository.GetCountAsync(c => true);
            var itemCategoriesModel = await _itemCategoryRepository.GetAllAsync();
            var itemCategories = _mapper.Map<IEnumerable<ItemCategory>>(itemCategoriesModel);
            return _paginationService.GetPaginatedList(itemCategories, itemCategoriesCount, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<PaginatedList<ItemCategory>> SearchItemCategories(SieveModel sieveModel, string searchQuery)
        {
            _itemCategoryRepository.SieveModel = sieveModel;
            var itemCategoriesCount = await _itemCategoryRepository.GetCountAsync(i => i.Name.Contains(searchQuery));
            var itemCategoriesModel = await _itemCategoryRepository.GetManyAsync(i => i.Name.Contains(searchQuery));
            var itemCategories = _mapper.Map<IEnumerable<ItemCategory>>(itemCategoriesModel);
            return _paginationService.GetPaginatedList(itemCategories, itemCategoriesCount, sieveModel.Page, sieveModel.PageSize);
        }

            public async Task<ItemCategory> GetItemCategoryById(int categoryId)
        {
            var categoryModel = await _itemCategoryRepository.GetByIdAsync(categoryId);
            return _mapper.Map<ItemCategory>(categoryModel);
        }


        public async Task<ItemCategory> CreateItemCategory(ItemCategoryRequest categoryRequest)
        {
            if (string.IsNullOrEmpty(categoryRequest.Name))
            {
                throw new ValidationException("Item Category name cannot be null or empty");
            }
            var categoryExists = await _itemCategoryRepository.ExistsAsync(c => c.Name.ToLower() == categoryRequest.Name.ToLower());
            if (categoryExists)
            {
                throw new ValidationException($"Item Category with name ({categoryRequest.Name}) Already exists");
            }
            var categoryModel = _mapper.Map<ItemCategories>(categoryRequest);
            await _itemCategoryRepository.AddAsync(categoryModel);
            return _mapper.Map<ItemCategory>(categoryModel);
        }

        public async Task<ItemCategory> PatchItemCategory(int categoryId, JsonPatchDocument<ItemCategory> categoryPatchDocument)
        {
            var categoryModel = await _itemCategoryRepository.GetByIdAsync(categoryId);
            var initialCategoryName = categoryModel.Name;
            if (categoryModel == null)
            {
                throw new ValidationException($"Item Category with Id ({categoryId}) not found");
            }
            var allowedPaths = new List<string>
            {
                "/Name"
            };

            foreach (var op in categoryPatchDocument.Operations)
            {
                if (!allowedPaths.Any(x => x.Equals(op.path, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ValidationException($"Attempt to modify data outside of user control. Logged and reported.");
                }
            }
            var modelPatch = _mapper.Map<JsonPatchDocument<ItemCategories>>(categoryPatchDocument);

            modelPatch.ApplyTo(categoryModel);

            var categoryExists = await _itemCategoryRepository.ExistsAsync(c => c.Name.ToLower() == categoryModel.Name.ToLower());
            if (categoryExists && !string.Equals(categoryModel.Name, initialCategoryName, StringComparison.OrdinalIgnoreCase))
            {
                throw new ValidationException($"Item Category with name ({categoryModel.Name}) Already exists");
            }

            await _itemCategoryRepository.UpdateAsync(categoryModel);

            return _mapper.Map<ItemCategory>(categoryModel);
        }

        public async Task DeleteItemCategory(int categoryId)
        {
            var categoryModel = await _itemCategoryRepository.GetByIdAsync(categoryId);
            
            if(categoryModel == null)
            {
                return;
            }

            var itemsForCategory = await _itemRepository.GetManyAsync(p=> p.ItemCategoryMapping.Any(c => c.ItemCategoryId == categoryId));
            if(itemsForCategory.Count() != 0)
            {
                throw new ValidationException($"Cannot delete category with items created");
            }
            await _itemCategoryRepository.DeleteAsync(categoryModel);
        }
    }
}
