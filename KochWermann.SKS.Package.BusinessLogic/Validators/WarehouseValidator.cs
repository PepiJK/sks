using FluentValidation;
using KochWermann.SKS.Package.BusinessLogic.Entities;

namespace KochWermann.SKS.Package.BusinessLogic.Validators
{
    public class WarehouseValidator : AbstractValidator<Warehouse>
    {
        public WarehouseValidator()
        {
            Include(new HopValidator());

            this.RuleFor(p => p.NextHops)
                .NotNull();

            RuleForEach(p => p.NextHops)
                .SetValidator(new NextHopValidator());
        }
    }
}