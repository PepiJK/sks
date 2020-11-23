using FluentValidation;
using KochWermann.SKS.Package.BusinessLogic.Entities;

namespace KochWermann.SKS.Package.BusinessLogic.Validators
{
    public class NextHopValidator : AbstractValidator<WarehouseNextHops>
    {
        public NextHopValidator()
        {
            this.RuleFor(p => p.TraveltimeMins)
                .GreaterThan(0);

            this.When(p => p.Hop is Warehouse, () =>
            {
                RuleFor(p => p.Hop as Warehouse)
                .SetValidator<WarehouseValidator>(t =>
                {
                    return new WarehouseValidator();
                });
            });

            this.When(p => p.Hop is Truck, () =>
            {
                RuleFor(p => p.Hop as Truck)
                .SetValidator<TruckValidator>(t =>
                {
                    return new TruckValidator();
                });
            });

            this.When(p => p.Hop is TransferWarehouse, () =>
            {
                RuleFor(p => p.Hop as TransferWarehouse)
                .SetValidator<TransferWarehouseValidator>(t =>
                {
                    return new TransferWarehouseValidator();
                });
            });
        }
    }
}