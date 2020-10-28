using KochWermann.SKS.Package.BusinessLogic.Entities;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using FluentValidation;
using KochWermann.SKS.Package.BusinessLogic.Validators;
using AutoMapper;
using KochWermann.SKS.Package.DataAccess.Interfaces;

namespace KochWermann.SKS.Package.BusinessLogic
{
    public class WarehouseLogic : IWarehouseLogic
    {
        private readonly IMapper _mapper;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly WarehouseValidator _warehouseValidator = new WarehouseValidator();
        private readonly NextHopValidator _nextHopValidator = new NextHopValidator();
        private readonly CodeValidator _codeValidator = new CodeValidator();

        public WarehouseLogic(IMapper mapper, IWarehouseRepository warehouseRepository)
        {
            _mapper = mapper;
            _warehouseRepository = warehouseRepository;
        }
        
        public Warehouse ExportWarehouses()
        {
            var dalWarehouse = _warehouseRepository.GetRootWarehouse();
            var blWarehouse = _mapper.Map<BusinessLogic.Entities.Warehouse>(dalWarehouse);

            return blWarehouse;
        }

        public void ImportWarehouses(Warehouse warehouse)
        { 
            Validate<Warehouse>(warehouse, _warehouseValidator);
            warehouse.NextHops.ForEach(nextHop => Validate<WarehouseNextHops>(nextHop, _nextHopValidator));

            var dalWarehouse = _mapper.Map<DataAccess.Entities.Warehouse>(warehouse);
            _warehouseRepository.Create(dalWarehouse);
        }

        public Warehouse GetWarehouse(string code)
        {
            Validate<string>(code, _codeValidator);

            var dalWarehouse = _warehouseRepository.GetWarehouseByCode(code);
            var blWarehouse = _mapper.Map<BusinessLogic.Entities.Warehouse>(dalWarehouse);

            return blWarehouse;
        }

        private void Validate<T>(T instanceToValidate, AbstractValidator<T> validator)
        {
            var validationResult = validator.Validate(instanceToValidate);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors); 
        }
    }
}