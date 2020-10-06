using FluentValidation;
using KochWermann.SKS.Package.BusinessLogic.Entities;

namespace KochWermann.SKS.Package.BusinessLogic.Validators
{
    public class ParcelValidator : AbstractValidator<Parcel>
    {
        public ParcelValidator()
        {
            this.RuleFor(p => p.Recipient)
                .SetValidator(new RecipientValidator())
                .NotNull()
                .NotEqual(p => p.Sender);

            this.RuleFor(p => p.Sender)
                .SetValidator(new RecipientValidator())
                .NotNull()
                .NotEqual(p => p.Recipient);

            this.RuleFor(p => p.Weight)
                .GreaterThan(0.0f);

            this.RuleFor(p => p.TrackingId)
                .Matches("/^[A-Z0-9]{9}$/");

            this.RuleFor(p => p.VisitedHops)
                .NotNull();

            this.RuleFor(p => p.FutureHops)
                .NotNull();
        }
    }
}