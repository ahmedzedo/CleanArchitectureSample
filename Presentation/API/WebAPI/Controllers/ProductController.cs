using CleanArchitecture.Application.Products.Commands.AddProduct;
using CleanArchitecture.Application.Products.Commands.ChangeProductItemAmount;
using CleanArchitecture.Application.Products.Commands.DeleteProduct;
using CleanArchitecture.Application.Products.Commands.UpdateProduct;
using CleanArchitecture.Application.Products.Queries.GetPagedProducts;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.WebAPI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebAPI.Controllers
{
    public class ProductController : BaseApiController
    {
        #region Actions

        [HttpPost("add-Product")]
        public async Task<IActionResult> AddProdcut([FromBody] AddProductCommand model)
        {
            return Result(await Mediator.Send(model));
        }

        [HttpPost("update-Product")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommand model)
        {
            return Result(await Mediator.Send(model));
        }

        [HttpPost("change-productItem-amount")]
        public async Task<IActionResult> ChangeProductItemAmount([FromBody] ChangeProductItemAmountCommand model)
        {
            return Result(await Mediator.Send(model));
        }

        [HttpPost("Delete-Product")]
        public async Task<IActionResult> DeleteProduct([FromBody] DeleteProductCommand model)
        {
            return Result(await Mediator.Send(model));
        }

        [Authorize(Roles = Roles.Administrator)]
        [HttpPost("get-product-list")]
        public async Task<IActionResult> GetProductList([FromBody] GetPagedProductsQuery model)
        {
            return Result(await Mediator.Send(model));
        }
        #endregion
    }
}
