using KochWermann.SKS.Package.BusinessLogic.Entities;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using FluentValidation;
using KochWermann.SKS.Package.BusinessLogic.Validators;


namespace KochWermann.SKS.Package.BusinessLogic
{
    public class WarehouseLogic : IWarehouseLogic
    {
        public Warehouse ExportWarehouses()
        {
            return new Warehouse();
        }

        public void ImportWarehouses(Warehouse warehouse)
        { 
            Hop h = warehouse.NextHops[0].Hop;
            if (h is Warehouse)
            {
                
            }
            
            IValidator<Warehouse> validator = new WarehouseValidator();
            var validationResult = validator.Validate(warehouse);

            if(validationResult.IsValid)
            {

            }
            else
            {
                throw new FluentValidation.ValidationException("new warehouse " + validationResult.Errors.ToString()); 
            }
        }

        public Warehouse GetWarehouse(string code)
        {
            return new Warehouse();
        }
    }
}