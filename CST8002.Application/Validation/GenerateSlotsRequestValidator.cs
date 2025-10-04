using CST8002.Application.DTOs;
using FluentValidation;

namespace CST8002.Application.Validation
{
    public sealed class GenerateSlotsRequestValidator : AbstractValidator<GenerateSlotsRequest>
    {
        public GenerateSlotsRequestValidator()
        {
            RuleFor(x => x.DoctorId).GreaterThan(0);
            RuleFor(x => x.WorkDate).Must(d => d != default);
            RuleFor(x => x.StartHour).InclusiveBetween((byte)0, (byte)23);
            RuleFor(x => x.EndHour).InclusiveBetween((byte)1, (byte)24);
            RuleFor(x => x).Must(x => x.StartHour < x.EndHour);
        }
    }
}
