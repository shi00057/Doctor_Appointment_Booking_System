using CST8002.Application.DTOs;
using FluentValidation;

namespace CST8002.Application.Validation
{
    public sealed class CancelAppointmentRequestValidator : AbstractValidator<CancelAppointmentRequest>
    {
        public CancelAppointmentRequestValidator()
        {
            RuleFor(x => x.ApptId).GreaterThan(0);
            RuleFor(x => x.PatientId).GreaterThan(0);
        }
    }
}
