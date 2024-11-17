using CleanArchitecture.Application.Carts.Commands.AddItemToCart;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
