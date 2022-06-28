using FluentValidation;

namespace SparkybitTest.Api.Dto.Validations;

public class LoginRequestDtoValidation : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidation()
    {
        RuleFor(x => x.Login).NotNull().NotEmpty().WithMessage("Login must be not empty");
        RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password must be not empty");
    }
}