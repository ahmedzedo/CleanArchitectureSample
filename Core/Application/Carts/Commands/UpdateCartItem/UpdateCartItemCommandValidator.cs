using FluentValidation;

namespace CleanArchitecture.Application.Carts.Commands.UpdateCartItem
{
    public class UpdateCartItemCommandValidator : AbstractValidator<UpdateCartItemCommand>
    {

        public UpdateCartItemCommandValidator()
        {
            RuleFor(i => i.Id)
                .NotEmpty();
            RuleFor(i => i.Count)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
