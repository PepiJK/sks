using FluentValidation;

namespace KochWermann.SKS.Package.BusinessLogic.Validators
{
    public class CodeValidator : AbstractValidator<string>
    {
        private readonly string _pattern = "^[A-Z]{4}\\d{1,4}$";
        public CodeValidator()
        {
            RuleFor(c => c).Matches(_pattern);
        }
    }
}