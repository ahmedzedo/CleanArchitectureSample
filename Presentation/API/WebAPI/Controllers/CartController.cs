﻿using CleanArchitecture.Application.Carts.Commands.AddItemToCart;
using CleanArchitecture.Application.Carts.Commands.RemoveCartItem;
using CleanArchitecture.Application.Carts.Commands.UpdateCartItem;
using CleanArchitecture.Application.Carts.Queries.GetCart;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.WebAPI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebAPI.Controllers
{
    [Authorize]
    public class CartController : BaseApiController
    {
        #region Actions
        [Authorize]
        [HttpPost("get-Cart")]
        public async Task<IActionResult> GetCart([FromBody] GetCartQuery model)
        {
            return Result(await Mediator.Send(model));
        }
        [Authorize]
        [HttpPost("add-item-to-cart")]
        public async Task<IActionResult> AddItemToCart([FromBody] AddItemToCartCommand model)
        {
            return Result(await Mediator.Send(model));
        }
        [Authorize]
        [HttpPost("edit-item-count")]
        public async Task<IActionResult> EditItemCount([FromBody] UpdateCartItemCommand model)
        {
            return Result(await Mediator.Send(model));
        }

        [Authorize]
        [HttpPost("remove-item")]
        public async Task<IActionResult> RemoveCartItem([FromBody] RemoveCartItemCommand model)
        {
            return Result(await Mediator.Send(model));
        }
        #endregion
    }
}
