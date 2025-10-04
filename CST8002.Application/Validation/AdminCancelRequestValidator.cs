using CST8002.Application.DTOs;
using FluentValidation;

namespace CST8002.Application.Validation
{
    public sealed class AdminCancelRequestValidator : AbstractValidator<AdminCancelRequest>
    {
        public AdminCancelRequestValidator()
        {
            RuleFor(x => x.ApptId).GreaterThan(0);

            RuleFor(x => x.Reason)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .Must(s => !string.IsNullOrWhiteSpace(s))
                .WithMessage("Reason is required.")
                .MaximumLength(256);
        }
    }
}
