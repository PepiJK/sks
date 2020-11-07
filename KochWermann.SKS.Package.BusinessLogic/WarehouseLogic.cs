using KochWermann.SKS.Package.BusinessLogic.Entities;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using FluentValidation;
using KochWermann.SKS.Package.BusinessLogic.Validators;
using AutoMapper;
using KochWermann.SKS.Package.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace KochWermann.SKS.Package.BusinessLogic
{
    public class WarehouseLogic : IWarehouseLogic
    {
        private readonly IMapper _mapper;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly WarehouseValidator _warehouseValidator = new WarehouseValidator();
        private readonly NextHopValidator _nextHopValidator = new NextHopValidator();
        private readonly CodeValidator _codeValidator = new CodeValidator();
        private readonly ILogger _logger;


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
            catch (DataAccess.Entities.DAL_NotFound_Exception ex)
            {
                throw NotFound_ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
            catch (DataAccess.Entities.DAL_Exception ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
            catch (Exception ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
        }

        public void ImportWarehouses(Warehouse warehouse)
        {
            try
            {
                _logger.LogInformation("Importing Warehouse");
                Validate<Warehouse>(warehouse, _warehouseValidator);
                warehouse.NextHops.ForEach(nextHop => Validate<WarehouseNextHops>(nextHop, _nextHopValidator));

                var dalWarehouse = _mapper.Map<DataAccess.Entities.Warehouse>(warehouse);
                _warehouseRepository.Create(dalWarehouse);
            }
            catch (DataAccess.Entities.DAL_Exception ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
            catch (Exception ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }

        }

        public Warehouse GetWarehouse(string code)
        {
            try
            {
                Validate<string>(code, _codeValidator);

                var dalWarehouse = _warehouseRepository.GetWarehouseByCode(code);
                var blWarehouse = _mapper.Map<BusinessLogic.Entities.Warehouse>(dalWarehouse);

                return blWarehouse;
            }
            catch (DataAccess.Entities.DAL_NotFound_Exception ex)
            {
                throw NotFound_ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
            catch (DataAccess.Entities.DAL_Exception ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
            catch (Exception ex)
            {
                throw ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
        }

        private void Validate<T>(T instanceToValidate, AbstractValidator<T> validator)
        {
            var validationResult = validator.Validate(instanceToValidate);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
        }

        private BL_Exception ExceptionHandler(string method, Exception ex)
        {
            _logger.LogError(ex.ToString());
            return new BL_Exception(method, ex);
        }

        private BL_NotFound_Exception NotFound_ExceptionHandler(string method, Exception ex)
        {
            _logger.LogError(ex.ToString());
            return new BL_NotFound_Exception(method, ex);
        }
    }
}