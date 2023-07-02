using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingListAPI.Entities;
using ShoppingListAPI.Models;
using ShoppingListAPI.Services;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingListAPI.Controllers
{
    [Route("api/user/{userId}/shoppinglist")]
    [ApiController]
    public class ShoppingListController : ControllerBase
    {
        private readonly IShoppingListService _shoppingListService;

        public ShoppingListController(IShoppingListService shoppingListService)
        {
            _shoppingListService = shoppingListService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ShoppingListDto>> GetAll([FromRoute] int userId)
        {
            var shoppingLists = _shoppingListService.GetUserShoppingLists(userId);

            return Ok(shoppingLists);
        }

        [HttpGet("{shoppingListId}")]
        public ActionResult<ShoppingListDto> Get([FromRoute] int userId, [FromRoute] int shoppingListId)
        {
            var shoppingList = _shoppingListService.GetShoppingListById(userId, shoppingListId);

            return Ok(shoppingList);
        }

        [HttpPost]
        public ActionResult Post([FromRoute] int userId, [FromBody] CreateShoppingListDto dto)
        {
            var shoppingListId = _shoppingListService.CreateShoppingList(userId, dto);

            return Created($"api/user/{userId}/shoppinglist/{shoppingListId}", null);
        }

        [HttpDelete("{shoppingListId}")]
        public ActionResult Delete([FromRoute] int userId, [FromRoute] int shoppingListId)
        {
            _shoppingListService.RemoveShoppingList(userId, shoppingListId);

            return NoContent();
        }

        [HttpPut("{shoppingListId}")]
        public ActionResult Update([FromRoute] int userId, [FromRoute] int shoppingListId, [FromBody] UpdateShoppingListDto dto)
        {
            _shoppingListService.UpdateShoppingList(userId, shoppingListId, dto);

            return Ok();
        }
    }
}
