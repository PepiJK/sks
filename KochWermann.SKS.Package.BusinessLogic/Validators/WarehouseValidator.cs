using FluentValidation;
using KochWermann.SKS.Package.BusinessLogic.Entities;

namespace KochWermann.SKS.Package.BusinessLogic.Validators
{
        public class WarehouseValidator : AbstractValidator<Warehouse>
    {
        public WarehouseValidator()
        {
            this.RuleFor(p => p.Description)
                .Matches("/^[A-Za-z 0-9-]{1,}&/");
            
            this.RuleFor(p => p.Code)
                .Matches("/^[A-Z0-9]{9}$/");

            this.RuleFor(p => p.NextHops)
                .NotNull();
        }
    }
}