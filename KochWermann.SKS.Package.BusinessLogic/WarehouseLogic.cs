using KochWermann.SKS.Package.BusinessLogic.Entities;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using FluentValidation;
using KochWermann.SKS.Package.BusinessLogic.Validators;
using AutoMapper;
using KochWermann.SKS.Package.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using KochWermann.SKS.Package.BusinessLogic.Helpers;

namespace KochWermann.SKS.Package.BusinessLogic
{
    public class WarehouseLogic : IWarehouseLogic
    {
        private readonly IMapper _mapper;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly ILogger _logger;
        private readonly WarehouseValidator _warehouseValidator = new WarehouseValidator();
        private readonly NextHopValidator _nextHopValidator = new NextHopValidator();
        private readonly CodeValidator _codeValidator = new CodeValidator();


        public WarehouseLogic(IMapper mapper, IWarehouseRepository warehouseRepository, ILogger<WarehouseLogic> logger)
        {
            _mapper = mapper;
            _warehouseRepository = warehouseRepository;
            _logger = logger;
            _logger.LogTrace("WarehouseLogic created");
        }

        public Warehouse ExportWarehouses()
        {
            try
            {
                var root = _warehouseRepository.GetRootWarehouse();
                return _mapper.Map<Warehouse>(root);
            }
            catch (DataAccess.Entities.DALNotFoundException ex)
            {
                _logger.LogError($"Could not find root warehouse {ex}");
                throw new BLNotFoundException("Could not find root warehouse", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ExportWarehouse {ex}");
                throw new BLException("Error in ExportWarehouses", ex);
            }
        }

        public void ImportWarehouses(Warehouse warehouse)
        {
            try
            {
                BusinessLogicHelper.Validate<Warehouse>(warehouse, _warehouseValidator, _logger);
                warehouse.NextHops.ForEach(nextHop => BusinessLogicHelper.Validate<WarehouseNextHops>(nextHop, _nextHopValidator, _logger));

                _warehouseRepository.ClearAllTables();
                _warehouseRepository.Create(_mapper.Map<DataAccess.Entities.Warehouse>(warehouse));
            }
            catch (ValidationException ex)
            {
                _logger.LogError($"Validation error in warehouse or its next hops {ex}");
                throw new BLValidationException("Validation error in warehouse or its next hops", ex);            
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ImportWarehouses {ex}");
                throw new BLException("Error in ImportWarehouses", ex);
            }
        }

        public Warehouse GetWarehouse(string code)
        {
            try
            {
                BusinessLogicHelper.Validate<string>(code, _codeValidator, _logger);

                var dalWarehouse = _warehouseRepository.GetWarehouseByCode(code);
                return _mapper.Map<BusinessLogic.Entities.Warehouse>(dalWarehouse);
            }
            catch (DataAccess.Entities.DALNotFoundException ex)
            {
                _logger.LogError($"Could not find warehouse with code {code} {ex}");
                throw new BLNotFoundException($"Could not find warehouse with code {code}", ex);
            }
            catch (ValidationException ex)
            {
                _logger.LogError($"Validation error in code {code} {ex}");
                throw new BLValidationException($"Validation error in code {code}", ex); 
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetWarehouse {ex}");
                throw new BLException("Error in GetWarehouse", ex);
            }
        }

    }
}