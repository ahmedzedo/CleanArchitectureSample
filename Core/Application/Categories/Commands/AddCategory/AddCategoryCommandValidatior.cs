using FluentValidation;

namespace CleanArchitecture.Application.Categories.Commands.AddCategory
{
    public class AddCategoryCommandValidatior : AbstractValidator<AddCategoryCommand>
    {
        public AddCategoryCommandValidatior()
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

            RuleFor(i => i.BriefAr)
               .NotEmpty()
               .MaximumLength(50)
               .MinimumLength(3);

            RuleFor(i => i.BriefEn)
               .NotEmpty()
               .MaximumLength(50)
               .MinimumLength(3);


            RuleFor(i => i.BriefFr)
               .NotEmpty()
               .MaximumLength(50)
               .MinimumLength(3);

        }
    }
}
