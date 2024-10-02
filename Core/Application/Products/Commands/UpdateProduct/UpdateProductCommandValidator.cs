using CleanArchitecture.Application.Products.Commands.AddProduct;
using FluentValidation;

namespace CleanArchitecture.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(i => i.NameAr)
                .NotEmpty()
                .MaximumLength(50)
                .MinimumLength(3);

            RuleFor(i => i.NameEn)
               .NotEmpty()
               .MaximumLength(50)
               .MinimumLength(3);


            RuleFor(i => i.NameFr)
               .NotEmpty()
               .MaximumLength(50)
               .MinimumLength(3);

            RuleFor(i => i.CategoriesIds)
                .NotEmpty();

            RuleFor(i => i.ItemsList)
                .NotEmpty();

            RuleForEach(i => i.ItemsList).SetValidator(new ProductDetailsDtoValidator());
        }
    }
}
