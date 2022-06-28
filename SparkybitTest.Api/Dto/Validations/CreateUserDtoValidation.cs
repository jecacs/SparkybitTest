using FluentValidation;

namespace SparkybitTest.Api.Dto.Validations;

public class CreateUserDtoValidation : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidation()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Please specify a first name");
    }
}