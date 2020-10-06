using System.Text.RegularExpressions;
using FluentValidation;
using KochWermann.SKS.Package.BusinessLogic.Entities;

namespace KochWermann.SKS.Package.BusinessLogic.Validators
{
    public class HopValidator : AbstractValidator<Hop>
    {
        public HopValidator()
        {
            this.RuleFor(p => p.Code)
                .Matches("/^[A-Z0-9]{9}$/");

            this.RuleFor(p => p.Description)
                .Matches("/^[A-Za-z 0-9-]{1,}&/");

            this.RuleFor(p => p.HopType)
                .NotNull();

            this.RuleFor(p => p.LocationCoordinates)
                .NotNull();
        }
    }
}