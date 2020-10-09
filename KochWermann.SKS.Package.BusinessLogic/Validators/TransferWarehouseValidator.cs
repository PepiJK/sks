using System.Security.AccessControl;
using FluentValidation;
using KochWermann.SKS.Package.BusinessLogic.Entities;

namespace KochWermann.SKS.Package.BusinessLogic.Validators
{
        public class TransferWarehouseValidator : AbstractValidator<Transferwarehouse>
    {
        public TransferWarehouseValidator()
        {
            this.RuleFor(p => p.Description)
                .Matches(@"[A-Za-zÄäÖöÜüß 0-9-]{1,}");

            this.RuleFor(p => p.LocationCoordinates)
                .NotNull();

            this.RuleFor(p => p.Code)
                .Matches(@"[A-Z0-9]{6,}");
        }
    }
}