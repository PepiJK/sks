using FluentValidation;
using KochWermann.SKS.Package.BusinessLogic.Entities;

namespace KochWermann.SKS.Package.BusinessLogic.Validators
{
        public class WarehouseValidator : AbstractValidator<Warehouse>
    {
        public WarehouseValidator()
        {
            this.RuleFor(p => p.Description)
                .Matches(@"[A-Za-zÄäÖöÜüß 0-9-]{1,}");
            
            this.RuleFor(p => p.Code)
                .Matches(@"[A-Z0-9]{6,}");

            this.RuleFor(p => p.NextHops)
                .NotNull();
        }
    }
}