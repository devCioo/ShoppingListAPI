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
            
            if (shoppingList is null)
            {
                return NotFound();
            }

           return Ok(shoppingList);
        }

        [HttpPost]
        public ActionResult Post([FromRoute] int userId, [FromBody] CreateShoppingListDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var shoppingListId = _shoppingListService.CreateShoppingList(userId, dto);

            return Created($"api/user/{userId}/shoppinglist/{shoppingListId}", null);
        }

        [HttpDelete("{shoppingListId}")]
        public ActionResult Delete([FromRoute] int userId, [FromRoute] int shoppingListId)
        {
            var isDeleted = _shoppingListService.RemoveShoppingList(userId, shoppingListId);

            if (isDeleted)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpPut("{shoppingListId}")]
        public ActionResult Update([FromRoute] int userId, [FromRoute] int shoppingListId, [FromBody] UpdateShoppingListDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isUpdated = _shoppingListService.UpdateShoppingList(userId, shoppingListId, dto);
            if (!isUpdated)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
