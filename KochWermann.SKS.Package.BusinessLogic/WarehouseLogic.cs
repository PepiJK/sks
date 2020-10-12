using KochWermann.SKS.Package.BusinessLogic.Entities;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using FluentValidation;
using KochWermann.SKS.Package.BusinessLogic.Validators;
using System;
using System.Text.RegularExpressions;

namespace KochWermann.SKS.Package.BusinessLogic
{
    public class WarehouseLogic : IWarehouseLogic
    {
        private string _codePattern = @"[A-Z0-9]{6,}";
        public Warehouse ExportWarehouses()
        {
            return new Warehouse();
        }

        public void ImportWarehouses(Warehouse warehouse)
        { 
            if (warehouse == null)
                throw new ArgumentNullException();
                
            Hop h = warehouse.NextHops[0].Hop;
            if (h is Warehouse)
            {
                // ?
            }
            
            IValidator<Warehouse> validator = new WarehouseValidator();
            var validationResult = validator.Validate(warehouse);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors); 
        }

        public Warehouse GetWarehouse(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentNullException();
            
            if (!Regex.IsMatch(code, _codePattern))
                throw new ArgumentException("code does not match pattern.");

            return new Warehouse();
        }
    }
}