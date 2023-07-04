using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingListAPI.Entities;
using ShoppingListAPI.Models;
using ShoppingListAPI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;

namespace ShoppingListAPI.Controllers
{
    [Route("api/shoppinglist")]
    [ApiController]
    [Authorize]
    public class ShoppingListController : ControllerBase
    {
        private readonly ShoppingListDbContext _dbContext;
        private readonly IShoppingListService _shoppingListService;
        private readonly IMapper _mapper;

        public ShoppingListController(ShoppingListDbContext dbContext, IShoppingListService shoppingListService, IMapper mapper)
        {
            _dbContext = dbContext;
            _shoppingListService = shoppingListService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ShoppingListDto>> GetAll()
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var shoppingLists = _shoppingListService.GetUserShoppingLists(userId);

            return Ok(shoppingLists);
        }

        [HttpGet("{shoppingListId}")]
        public ActionResult<ShoppingListDto> Get([FromRoute] int shoppingListId)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var shoppingList = _shoppingListService.GetShoppingListById(userId, shoppingListId);

            return Ok(shoppingList);
        }

        [HttpPost]
        [Authorize(Policy = "ShoppingListsLimit")]
        public ActionResult Post([FromBody] CreateShoppingListDto dto)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var shoppingListId = _shoppingListService.CreateShoppingList(userId, dto);

            return Created($"api/shoppinglist/{shoppingListId}", null);
        }

        [HttpDelete("{shoppingListId}")]
        public ActionResult Delete([FromRoute] int shoppingListId)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            _shoppingListService.RemoveShoppingList(userId, shoppingListId);

            return NoContent();
        }

        [HttpPut("{shoppingListId}")]
        public ActionResult Update([FromRoute] int shoppingListId, [FromBody] UpdateShoppingListDto dto)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            _shoppingListService.UpdateShoppingList(userId, shoppingListId, dto);

            return Ok();
        }

        [HttpGet("{shoppingListId}/download")]
        public IActionResult Download([FromRoute] int shoppingListId)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var shoppingList = _dbContext.ShoppingLists
                .Include(sl => sl.Items)
                .Where(sl => sl.UserId == userId)
                .FirstOrDefault(sl => sl.Id == shoppingListId);
            var shoppingListDto = _mapper.Map<ShoppingListDto>(shoppingList);

            var shoppingListData = _shoppingListService.ShoppingListStringify(shoppingListDto);
            var fileName = $"ShoppingList_{shoppingList.Name}_{DateTime.Now.ToString("ddMMyy-HHmmss")}.txt";

            var tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
            System.IO.File.WriteAllText(tempFilePath, shoppingListData);
            byte[] fileBytes = System.IO.File.ReadAllBytes(tempFilePath);

            return File(fileBytes, "application/octet-stream", fileName);
        }
    }
}
