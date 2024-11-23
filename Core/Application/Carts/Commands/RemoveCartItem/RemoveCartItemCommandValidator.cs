using FluentValidation;

namespace CleanArchitecture.Application.Carts.Commands.RemoveCartItem
{
    public class RemoveCartItemCommandValidator : AbstractValidator<RemoveCartItemCommand>
    {
        public RemoveCartItemCommandValidator()
        {
            RuleFor(i => i.CartId)
               .NotEmpty()
               .WithMessage("Cart Id Couldn't be Empty");
            RuleFor(i => i.CartItemId)
                .NotEmpty()
                .WithMessage("CartItemId couldn't be Empty");
        }
    }
}
