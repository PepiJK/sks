using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using KochWermann.SKS.Package.BusinessLogic.Entities;
using NUnit.Framework;
using System.Collections.Generic;
using FluentValidation;
using System;
using AutoMapper;
using Moq;
using KochWermann.SKS.Package.DataAccess.Interfaces;
using KochWermann.SKS.Package.BusinessLogic.Mapper;
using Microsoft.Extensions.Logging;

using BLException = KochWermann.SKS.Package.BusinessLogic.Entities.BLException;

namespace KochWermann.SKS.Package.BusinessLogic.Tests
{
    public class WarehouseLogicTests
    {
        private IWarehouseLogic _warehouseLogic;
        private Warehouse _validWarehouse;
        private string _validCode = "CODE123";
        private string _notFoundCode = "CODE321";
        private string _invalidCode = "hi";

        [SetUp]
        public void Setup()
        {
            _validWarehouse = new Warehouse{
                Code = _validCode,
                Description = "This should be a valid description",
                HopType = "Warehouse",
                Level = 0,
                ProcessingDelayMins = 1,
                LocationName = "Root",
                LocationCoordinates = new GeoCoordinate{Lat = 13, Lon = 47},
                NextHops = new List<WarehouseNextHops>{new WarehouseNextHops{TraveltimeMins = 69}}
            };

            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DalMapperProfile());
            });
            var mapper = mockMapper.CreateMapper();

            //mock warehouse repository
            var mock = new Mock<IWarehouseRepository>();
            mock.Setup(warehouseRepository => warehouseRepository.GetRootWarehouse()).Returns(mapper.Map<DataAccess.Entities.Warehouse>(_validWarehouse));
            mock.Setup(warehouseRepository => warehouseRepository.Create(
                It.IsAny<DataAccess.Entities.Hop>()
            )).Returns<DataAccess.Entities.Hop>(h => h.Code);
            mock.Setup(warehouseRepository => warehouseRepository.GetWarehouseByCode(
                _validCode
            )).Returns(new DataAccess.Entities.Warehouse{
                LocationCoordinates = new NetTopologySuite.Geometries.Point(1, 1)
            });
            mock.Setup(warehouseRepository => warehouseRepository.GetHopByCode(
                _validCode
            )).Returns(new DataAccess.Entities.Warehouse{
                LocationCoordinates = new NetTopologySuite.Geometries.Point(1, 1)
            });
            mock.Setup(warehouseRepository => warehouseRepository.GetHopByCode(
                _notFoundCode
            )).Throws(new DataAccess.Entities.DALNotFoundException("Code Not Found", new Exception()));
            mock.Setup(warehouseRepository => warehouseRepository.GetWarehouseByCode(
                _notFoundCode
            )).Throws(new DataAccess.Entities.DALNotFoundException("Code Not Found", new Exception()));

            var loggerMock = new Mock<ILogger<WarehouseLogic>>();

            _warehouseLogic = new WarehouseLogic(mapper, mock.Object, loggerMock.Object);
        }

        [Test]
        public void Should_Export_Warehouses()
        {
            var res = _warehouseLogic.ExportWarehouses();
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<Warehouse>(res);
            Assert.AreEqual("Root", res.LocationName);
        }

        [Test]
        public void Should_Import_Warehouses()
        {
            Assert.DoesNotThrow(() => _warehouseLogic.ImportWarehouses(_validWarehouse));
        }

        [Test]
        public void Should_Throw_Exception_On_Import_Warehouses_Of_Null_Warehouse()
        {
            Assert.Throws<BLValidationException>(() => _warehouseLogic.ImportWarehouses(null));
        }

        [Test]
        public void Should_Throw_Exception_On_Import_Warehouses_Of_Invalid_Code()
        {
            _validWarehouse.Code = _invalidCode;
            Assert.Throws<BLValidationException>(() => _warehouseLogic.ImportWarehouses(_validWarehouse));
        }

        [Test]
        public void Should_Throw_Exception_On_Import_Warehouses_Of_Invalid_Next_Hop_Truck()
        {
            _validWarehouse.NextHops[0].Hop = new Truck(); 
            Assert.Throws<BLValidationException>(() => _warehouseLogic.ImportWarehouses(_validWarehouse));
        }

        [Test]
        public void Should_Throw_Exception_On_Import_Warehouses_Of_Invalid_Next_Hop_Warehouse()
        {
            _validWarehouse.NextHops[0].Hop = new Warehouse(); 
            Assert.Throws<BLValidationException>(() => _warehouseLogic.ImportWarehouses(_validWarehouse));
        }

        [Test]
        public void Should_Throw_Exception_On_Import_Warehouses_Of_Invalid_Next_Hop_Transferwarehouse()
        {
            _validWarehouse.NextHops[0].Hop = new TransferWarehouse(); 
            Assert.Throws<BLValidationException>(() => _warehouseLogic.ImportWarehouses(_validWarehouse));
        }

        [Test]
        public void Should_Get_Hop_As_Warehouse()
        {
            var res = _warehouseLogic.GetHop(_validCode);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<Warehouse>(res);
        }

        [Test]
        public void Should_Throw_Exception_On_Get_Warehouses_Of_Invalid_Code()
        {
            Assert.Throws<BLValidationException>(() => _warehouseLogic.GetHop(_invalidCode));
        }

        [Test]
        public void Should_Throw_Exception_On_Get_Warehouses_Of_Null_Code()
        {
            Assert.Throws<BLValidationException>(() => _warehouseLogic.GetHop(null));
        }

        [Test]
        public void Should_Throw_Not_Found_Exception_On_Get_Warehouses_Of_Not_Found_Code()
        {
            Assert.Throws<BLNotFoundException>(() => _warehouseLogic.GetHop(_notFoundCode));
        }
    }
}