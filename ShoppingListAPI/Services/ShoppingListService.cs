using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingListAPI.Entities;
using ShoppingListAPI.Exceptions;
using ShoppingListAPI.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ShoppingListAPI.Services
{
    public interface IShoppingListService
    {
        IEnumerable<ShoppingListDto> GetUserShoppingLists(int userId);
        ShoppingListDto GetShoppingListById(int userId, int shoppingListId);
        int CreateShoppingList(int userId, CreateShoppingListDto dto);
        void RemoveShoppingList(int userId, int shoppingListId);
        void UpdateShoppingList(int userId, int shoppingListId, UpdateShoppingListDto dto);
        string ShoppingListStringify(ShoppingListDto dto);
    }
    public class ShoppingListService : IShoppingListService
    {
        private readonly ShoppingListDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ShoppingListService> _logger;

        public ShoppingListService(ShoppingListDbContext dbContext, IMapper mapper, ILogger<ShoppingListService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }
        public IEnumerable<ShoppingListDto> GetUserShoppingLists(int userId)
        {
            var shoppingLists = _dbContext
                .ShoppingLists
                .Where(sl => sl.UserId == userId)
                .ToList();

            var shoppingListsDtos = _mapper.Map<List<ShoppingListDto>>(shoppingLists);

            return shoppingListsDtos;
        }
        public ShoppingListDto GetShoppingListById(int userId, int shoppingListId)
        {
            var shoppingList = _dbContext.ShoppingLists
                .Include(sl => sl.Items)
                .Where(sl => sl.UserId == userId)
                .FirstOrDefault(sl => sl.Id == shoppingListId);

            if (shoppingList is null)
            {
                throw new NotFoundException("Shopping list not found");
            }

            var shoppingListDto = _mapper.Map<ShoppingListDto>(shoppingList);

            return shoppingListDto;
        }
        public int CreateShoppingList(int userId, CreateShoppingListDto dto)
        {
            var shoppingList = _mapper.Map<ShoppingList>(dto);
            shoppingList.UserId = userId;
            shoppingList.CreatedOn = DateTime.Now;

            _dbContext.ShoppingLists.Add(shoppingList);
            _dbContext.SaveChanges();

            return shoppingList.Id;
        }
        public void RemoveShoppingList(int userId, int shoppingListId)
        {
            var shoppingList = _dbContext.ShoppingLists
                .Where(sl => sl.UserId == userId)
                .FirstOrDefault(sl => sl.Id == shoppingListId);

            if (shoppingList is null)
            {
                throw new NotFoundException("Shopping list not found");
            }

            _dbContext.ShoppingLists.Remove(shoppingList);
            _dbContext.SaveChanges();
        }
        public void UpdateShoppingList(int userId, int shoppingListId, UpdateShoppingListDto dto)
        {
            var shoppingList = _dbContext.ShoppingLists
                .Where(sl => sl.UserId == userId)
                .FirstOrDefault(sl => sl.Id == shoppingListId);

            if (shoppingList is null)
            {
                throw new NotFoundException("Shopping list not found");
            }

            shoppingList.Name = dto.Name;
            shoppingList.Description = dto.Description;

            _dbContext.SaveChanges();
        }
        public string ShoppingListStringify(ShoppingListDto dto)
        {
            var shoppingListData = $"\t\t\t<<< {dto.Name} >>>\n" +
                $"Description: {dto.Description}\n\n";

            int i = 1;
            double total = 0, sum = 0;
            foreach(ItemDto item in dto.Items)
            {
                total = item.UnitPrice * item.Quantity;
                shoppingListData += $"{i}. Item: {item.Name}\t"
                    + $"Cost: {item.UnitPrice}/" 
                    + (item.MeasureUnit == 0 ? "unit" : "kg")
                    + $"\tQuantity: {item.Quantity}\tIn total: {total}\n{item.Description}\n";
                sum += total;
                i++;
            }
            shoppingListData += $"\tTotal sum: {sum}";

            return shoppingListData;
        }
    }
}
