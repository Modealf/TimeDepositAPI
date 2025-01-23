using FluentValidation;
using TimeDepositAPI.DTOs;
using TimeDepositAPI.Models;

namespace TimeDepositAPI.Validators
{
    public class CustomDepositRequestValidator : AbstractValidator<CreateCustomDepositRequestDto>
    {
        public CustomDepositRequestValidator()
        {
            RuleFor(x => x.Amount).NotNull().WithMessage("user must input amount of deposit").GreaterThan(0).WithMessage("amount must be greater than 0");
            RuleFor(x => x.DurationInDays).NotNull().WithMessage("user must input duration of deposit").GreaterThan(0).LessThanOrEqualTo(365).WithMessage("duration cannot exceed 365 days");
        }
    }
}


