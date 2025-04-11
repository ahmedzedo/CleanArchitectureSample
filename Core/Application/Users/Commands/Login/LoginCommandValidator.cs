using CleanArchitecture.Application.Common.Abstracts.Account;
using FluentValidation;

namespace CleanArchitecture.Application.Users.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public IUserService UserService { get; set; }

        public LoginCommandValidator(IUserService userService)
        {
            UserService = userService;
            RuleFor(c => c.UserName)
                .NotEmpty()
                .MustAsync(async (username, cancellation) =>
                {
                    return !string.IsNullOrEmpty(await UserService.GetUserAsync(username!));
                })
                .WithMessage("invalid username or password");

            RuleFor(c => c.Password)
                .NotEmpty()
                .MustAsync(async (context, password, cancellation) =>
                {
                    try
                    {
                        return await UserService.CheckPasswordAsync(context.UserName!, password);
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }).WithMessage(m => "invalid username or password");
          
        }
    }
}
