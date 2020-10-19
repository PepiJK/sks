using FluentValidation;
using KochWermann.SKS.Package.BusinessLogic.Entities;

namespace KochWermann.SKS.Package.BusinessLogic.Validators
{
    public class RecipientValidator : AbstractValidator<Recipient>
    {
        public RecipientValidator()
        {
            this.RuleFor(p => p.Country)
                .Must(p => p.Equals("Österreich") || p.Equals("Austria"));

            this.RuleFor(p => p.PostalCode)
                .Matches(@"A-[0-9]{4}");

            this.RuleFor(p => p.Street)
                .Matches(@"[A-Za-zÄäÖöÜüß]{3,} [0-9\/A-Za-zÄäÖöÜüß]{1,}");

            this.RuleFor (p=> p.City)
                .Matches(@"[A-ZÄÖÜ]{1}[A-Za-zÄäÖöÜüß -]{1,}");

            this.RuleFor (p => p.Name)
                .Matches(@"[A-ZÄäÖöÜüß]{1}[A-Za-z -]{1,}");
        }
    } 
}