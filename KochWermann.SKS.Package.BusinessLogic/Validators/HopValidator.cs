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
                .Matches(@"[A-Z0-9]{6,}");

            this.RuleFor(p => p.Description)
                .Matches(@"[A-Za-zÄäÖöÜüß \/0-9-]+");

            this.RuleFor(p => p.ProcessingDelayMins)
                .GreaterThan(0);
        }
    }
}