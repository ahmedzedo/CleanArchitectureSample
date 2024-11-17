using FluentValidation;

namespace CleanArchitecture.Application.Carts.Commands.AddItemToCart
{
    public class AddItemToCartCommandValidator : AbstractValidator<AddItemToCartCommand>
    {
        public AddItemToCartCommandValidator()
        {
            RuleFor(i => i.ProductItemId)
                .NotEmpty()
                .WithMessage("product Item Id Couldn't be Empty");
            RuleFor(i => i.Count)
                .GreaterThan(0);
        }
    }
}
