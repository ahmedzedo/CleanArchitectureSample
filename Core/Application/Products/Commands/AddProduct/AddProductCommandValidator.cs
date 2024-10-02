using FluentValidation;

namespace CleanArchitecture.Application.Products.Commands.AddProduct
{
    public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        public AddProductCommandValidator()
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

    public class ProductDetailsDtoValidator : AbstractValidator<ProductItemDto>
    {
        public ProductDetailsDtoValidator()
        {
            RuleFor(i => i.Price).GreaterThan(0);
            RuleFor(i => i.Description).MaximumLength(250);
        }
    }
}
