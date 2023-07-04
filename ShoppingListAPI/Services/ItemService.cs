using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoppingListAPI.Entities;
using ShoppingListAPI.Exceptions;
using ShoppingListAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ShoppingListAPI.Services
{
    public interface IItemService
    {
        List<ItemDto> GetAllShoppingListItems(int userId, int shoppingListId, ItemQuery query);
        ItemDto GetItemById(int userId, int shoppingListId, int itemId);
        int AddItemToShoppingList(int userId, int shoppingListId, CreateItemDto dto);
        void RemoveItemFromShoppingList(int userId, int shoppingListId, int itemId);
        void UpdateItemInShoppingList(int userId, int shoppingListId, int itemId, UpdateItemDto dto);
    }
    public class ItemService : IItemService
    {
        private readonly ShoppingListDbContext _dbContext;
        private readonly IMapper _mapper;

        public ItemService(ShoppingListDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public List<ItemDto> GetAllShoppingListItems(int userId, int shoppingListId, ItemQuery query)
        {
            var shoppingList = _dbContext.ShoppingLists
                .Where(sl => sl.UserId == userId)
                .Include(sl => sl.Items)
                .FirstOrDefault(sl => sl.Id == shoppingListId);

            if (shoppingList is null)
            {
                throw new NotFoundException("Shopping list not found");
            }

            var items = _dbContext.Items
                .Where(i => i.ShoppingListId == shoppingList.Id);

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelector = new Dictionary<string, Expression<Func<Item, object>>>
                {
                    { nameof(Item.Name), i => i.Name },
                    { nameof(Item.Description), i => i.Description },
                    { nameof(Item.UnitPrice), i => i.UnitPrice },
                    { nameof(Item.Quantity), i => i.Quantity }
                };

                var selectedColumn = columnsSelector[query.SortBy];

                items = query.SortDirection == SortDirection.Asc ? items.OrderBy(selectedColumn) : items.OrderByDescending(selectedColumn);
            }

            var itemDtos = _mapper.Map<List<ItemDto>>(items);

            return itemDtos;
        }
        public ItemDto GetItemById(int userId, int shoppingListId, int itemId)
        {
            var shoppingList = _dbContext.ShoppingLists
                .Where(sl => sl.UserId == userId)
                .FirstOrDefault(sl => sl.Id == shoppingListId);

            if (shoppingList is null)
            {
                throw new NotFoundException("Shopping list not found");
            }

            var item = _dbContext.Items
                .FirstOrDefault(i => i.Id == itemId);

            if (item is null || item.ShoppingListId != shoppingListId)
            {
                throw new NotFoundException("Item not found");
            }

            var itemDto = _mapper.Map<ItemDto>(item);

            return itemDto;
        }
        public int AddItemToShoppingList(int userId, int shoppingListId, CreateItemDto dto)
        {
            var shoppingList = _dbContext.ShoppingLists
                .Where(sl => sl.UserId == userId)
                .FirstOrDefault(sl => sl.Id == shoppingListId);

            if (shoppingList is null)
            {
                throw new NotFoundException("Shopping list not found");
            }

            var item = _mapper.Map<Item>(dto);
            item.ShoppingListId = shoppingListId;

            _dbContext.Items.Add(item);
            _dbContext.SaveChanges();

            return item.Id;
        }
        public void RemoveItemFromShoppingList(int userId, int shoppingListId, int itemId)
        {
            var shoppingList = _dbContext.ShoppingLists
                .Where(sl => sl.UserId == userId)
                .FirstOrDefault(sl => sl.Id == shoppingListId);

            if (shoppingList is null)
            {
                throw new NotFoundException("Shopping list not found");
            }

            var item = _dbContext.Items
                .FirstOrDefault(i => i.Id == itemId);

            if (item is null || item.ShoppingListId != shoppingListId)
            {
                throw new NotFoundException("Item not found");
            }

            _dbContext.Remove(item);
            _dbContext.SaveChanges();
        }

        public void UpdateItemInShoppingList(int userId, int shoppingListId, int itemId, UpdateItemDto dto)
        {
            var shoppingList = _dbContext.ShoppingLists
                .Where(sl => sl.UserId == userId)
                .FirstOrDefault(sl => sl.Id == shoppingListId);

            if (shoppingList is null)
            {
                throw new NotFoundException("Shopping list not found");
            }

            var item = _dbContext.Items
                .FirstOrDefault(i => i.Id == itemId);

            if (item is null || item.ShoppingListId != shoppingListId)
            {
                throw new NotFoundException("Item not found");
            }

            item.Name = dto.Name;
            item.Description = dto.Description;
            item.MeasureUnit = dto.MeasureUnit;
            item.UnitPrice = dto.UnitPrice;
            item.Quantity = dto.Quantity;

            _dbContext.SaveChanges();
        }
    }
}
