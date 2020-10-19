using FluentValidation;
using KochWermann.SKS.Package.BusinessLogic.Entities;

namespace KochWermann.SKS.Package.BusinessLogic.Validators
{
    public class TruckValidator : AbstractValidator<Truck>
    {
        public TruckValidator()
        {         
            this.RuleFor(p => p.Description)
                .Matches(@"[A-Za-zÄäÖöÜüß \/0-9-]+");

            this.RuleFor(p => p.LocationCoordinates)
                .NotNull();

            this.RuleFor(p => p.Code)
                .Matches(@"[A-Z0-9]{6,}"); 
        }
    }
}