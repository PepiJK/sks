using FluentValidation;
using KochWermann.SKS.Package.BusinessLogic.Entities;

namespace KochWermann.SKS.Package.BusinessLogic.Validators
{
        public class NextHopValidator : AbstractValidator<WarehouseNextHops>
    {
        public NextHopValidator()
        {
            this.RuleFor(p => p.TraveltimeMins)
                .NotNull();
        }
    }
}