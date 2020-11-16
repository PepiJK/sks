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
                var blRoot = _mapper.Map<Warehouse>(root);
                return blRoot;
            }
            catch (DataAccess.Entities.DALNotFoundException ex)
            {
                throw BusinessLogicHelper.NotFoundExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            catch (DataAccess.Entities.DALException ex)
            {
                throw BusinessLogicHelper.ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            catch (Exception ex)
            {
                throw BusinessLogicHelper.ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
        }

        public void ImportWarehouses(Warehouse warehouse)
        {
            try
            {
                _logger.LogInformation("Import Warehouses");
                BusinessLogicHelper.Validate<Warehouse>(warehouse, _warehouseValidator, _logger);
                warehouse.NextHops.ForEach(nextHop => BusinessLogicHelper.Validate<WarehouseNextHops>(nextHop, _nextHopValidator, _logger));

                _warehouseRepository.Clear();
                _warehouseRepository.Create(_mapper.Map<DataAccess.Entities.Warehouse>(warehouse));
            }
            catch (DataAccess.Entities.DALException ex)
            {
                throw BusinessLogicHelper.ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            catch (ValidationException ex)
            {
                throw BusinessLogicHelper.ValidationExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            catch (Exception ex)
            {
                throw BusinessLogicHelper.ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }

        }

        public Warehouse GetWarehouse(string code)
        {
            try
            {
                BusinessLogicHelper.Validate<string>(code, _codeValidator, _logger);

                var dalWarehouse = _warehouseRepository.GetWarehouseByCode(code);
                var blWarehouse = _mapper.Map<BusinessLogic.Entities.Warehouse>(dalWarehouse);

                return blWarehouse;
            }
            catch (DataAccess.Entities.DALNotFoundException ex)
            {
                throw BusinessLogicHelper.NotFoundExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            catch (DataAccess.Entities.DALException ex)
            {
                throw BusinessLogicHelper.ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            catch (ValidationException ex)
            {
                throw BusinessLogicHelper.ValidationExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
            catch (Exception ex)
            {
                throw BusinessLogicHelper.ExceptionHandler($"{ex.GetType()} Exception in {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex, _logger);
            }
        }


    }
}