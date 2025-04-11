using CleanArchitecture.Application.Categories.Commands.AddCategory;
using CleanArchitecture.Application.Categories.Queries.GetAllCategories;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.WebAPI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebAPI.Controllers
{
    public class CategoryController : BaseApiController
    {
        #region Actions
       
        [Authorize(Roles = Roles.Administrator)]
        [HttpPost("add-category")]
        public async Task<IActionResult> AddCategpry([FromBody] AddCategoryCommand model)
        {
            return Result(await Mediator.Send(model));
        }

        [Authorize]
        [HttpPost("get-Category-list")]
        public async Task<IActionResult> GetCategoryList([FromBody] GetAllCategoriesQuery model)
        {
            return Result(await Mediator.Send(model));
        }
        #endregion
    }
}
