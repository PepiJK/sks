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
                .Matches("/^A-([0-9]{4})$/");

            this.RuleFor(p => p.Street)
                .Matches("/^[A-Za-z]{3,} [0-9/A-Za-z]{1,}$/");

            this.RuleFor (p=> p.City)
                .Matches("/^[A-Z]{1}[A-Za-z -]{1,}&/");

            this.RuleFor (p => p.Name)
                .Matches("/^[A-Z]{1}[A-Za-z -]{1,}&/");
        }
    } 
}