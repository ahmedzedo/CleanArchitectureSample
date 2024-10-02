using FluentValidation;

namespace CleanArchitecture.Application.Carts.Commands.AddItemToCart
{
    public class AddItemToCartCommandValidator : AbstractValidator<AddItemToCartCommand>
    {
        public AddItemToCartCommandValidator()
        {
            RuleFor(i => i.ProductItemId)
                .NotEmpty();
            RuleFor(i => i.Count)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
