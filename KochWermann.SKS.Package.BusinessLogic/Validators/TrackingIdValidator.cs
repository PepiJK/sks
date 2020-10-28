using FluentValidation;

namespace KochWermann.SKS.Package.BusinessLogic.Validators
{
    public class TrackingIdValidator : AbstractValidator<string>
    {
        private readonly string _pattern = "^[A-Z0-9]{9}$";
        public TrackingIdValidator()
        {
            RuleFor(t => t).Matches(_pattern);
        }
    }
}