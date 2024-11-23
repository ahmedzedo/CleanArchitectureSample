using CleanArchitecture.Application.Common.Abstracts.Account;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Common.Messaging;
using CleanArchitecture.Application.Users.Commands.Dtos;

namespace CleanArchitecture.Application.Users.Commands.CreateUser
{
    #region Request
    public record CreateUserCommand(string FirstName,
                                    string MiddleName,
                                    string ThirdName,
                                    string FamilyName,
                                    string Password,
                                    string Email) : BaseCommand<Guid>;

    #endregion

    #region Request Handler
    public class CreateUserCommandHandler : BaseCommandHandler<CreateUserCommand, Guid>
    {

        #region Dependencies
        private IIdentityService IdentityService { get; }

        #endregion

        #region Constructor
        public CreateUserCommandHandler(IServiceProvider serviceProvider, IApplicationDbContext dbContext, IIdentityService identityService)
           : base(serviceProvider, dbContext)
        {
            IdentityService = identityService;
        }
        #endregion

        #region Request Handle
        public override async Task<IResult<Guid>> HandleRequest(CreateUserCommand request,
                                                                  CancellationToken cancellationToken)
        {
            var user = new UserDto
            {
                UserName = request.UserName!,
                Email = request.Email,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                ThirdName = request.ThirdName,
                FamilyName = request.FamilyName
            };
            (IResult<bool> result, string userId) = await IdentityService.CreateUserAsync(user, request.Password);

            return result.IsSuccess && result.Error == null
                ? Result.Success(Guid.Parse(userId), 1)
                : Result.Failure<Guid>(result.Error!);
        }


        #endregion
    }
    #endregion
}
