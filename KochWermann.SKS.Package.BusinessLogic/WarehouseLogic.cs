using KochWermann.SKS.Package.BusinessLogic.Entities;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using FluentValidation;
using KochWermann.SKS.Package.BusinessLogic.Validators;
using System;
using System.Text.RegularExpressions;
using AutoMapper;
using KochWermann.SKS.Package.DataAccess.Interfaces;

namespace KochWermann.SKS.Package.BusinessLogic
{
    public class WarehouseLogic : IWarehouseLogic
    {
        private readonly IMapper _mapper;
        private readonly IWarehouseRepository _warehouseRepository;

        private string _codePattern = @"[A-Z0-9]{6,}";

        public WarehouseLogic(IMapper mapper, IWarehouseRepository warehouseRepository)
        {
            _mapper = mapper;
            _warehouseRepository = warehouseRepository;
        }
        
        public Warehouse ExportWarehouses()
        {
            return new Warehouse();
        }

        public void ImportWarehouses(Warehouse warehouse)
        { 
            ValidateWarehouse(warehouse);
        }

        public Warehouse GetWarehouse(string code)
        {
            ValidateCode(code);
            return new Warehouse();
        }

        private void ValidateWarehouse(Warehouse warehouse)
        {
            if (warehouse == null)
                throw new ArgumentNullException();
            
            var validator = new WarehouseValidator();
            var validationResult = validator.Validate(warehouse);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors); 

            var nextHopValidator = new NextHopValidator();
            foreach (var nextHop in warehouse.NextHops)
            {
                validationResult = nextHopValidator.Validate(nextHop);
                if (!validationResult.IsValid)
                    throw new ValidationException(validationResult.Errors);
            }
        }

        private void ValidateCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentNullException();
            
            if (!Regex.IsMatch(code, _codePattern))
                throw new ArgumentException("code does not match pattern.");
        }
    }
}