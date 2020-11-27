using NUnit.Framework;
using KochWermann.SKS.Package.Services.Controllers;
using KochWermann.SKS.Package.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AutoMapper;
using KochWermann.SKS.Package.Services.Mapper;
using Moq;
using KochWermann.SKS.Package.BusinessLogic.Interfaces;
using FizzWare.NBuilder;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace KochWermann.SKS.Package.Services.Tests.ControllerTests
{
    public class WarehouseManagementApiTests
    {
        private WarehouseManagementApiController _warehouseManagementApiController;
        private Services.DTOs.Warehouse _testWarehouse;
        private string _testCode = "CODE123";
        private string _notFoundCode = "CODE321";
        private string _invalidCode = "hallo";

        [SetUp]
        public void Setup()
        {
            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BlMapperProfile());
            });
            var mapper = mockMapper.CreateMapper();

            //generate test object
            _testWarehouse = Builder<Services.DTOs.Warehouse>.CreateNew()
                .With(x => x.Level = 0)
                .With(x => x.LocationCoordinates = new Services.DTOs.GeoCoordinate{Lat = 50, Lon = 50})
                .With(x => x.HopType = "Warehouse")
                .With(x => x.NextHops = new List<Services.DTOs.WarehouseNextHops>{
                    Builder<Services.DTOs.WarehouseNextHops>.CreateNew()
                        .With(x => x.Hop = Builder<Services.DTOs.Hop>.CreateNew()
                            .With(x => x.LocationCoordinates = new Services.DTOs.GeoCoordinate{Lat = 100, Lon = 100})
                        .Build())
                    .Build()
                })
            .Build();

            //mock tracking logic
            var mock = new Mock<IWarehouseLogic>();    
            
            mock.Setup(trackingLogic => trackingLogic.ExportWarehouses()).Returns(new BusinessLogic.Entities.Warehouse());     
            
            mock.Setup(trackingLogic => trackingLogic.GetHop(
                _testCode
            )).Returns(new BusinessLogic.Entities.Warehouse());
            mock.Setup(trackingLogic => trackingLogic.GetHop(
                _notFoundCode
            )).Throws(new BusinessLogic.Entities.BLNotFoundException("Code Not Found", new System.Exception()));
            mock.Setup(trackingLogic => trackingLogic.GetHop(
                _invalidCode
            )).Throws(new BusinessLogic.Entities.BLException("Code Is Invalid", new System.Exception()));
            
            mock.Setup(trackingLogic => trackingLogic.ImportWarehouses(
                It.IsAny<BusinessLogic.Entities.Warehouse>()
            ));
            mock.Setup(trackingLogic => trackingLogic.ImportWarehouses(
                It.Is<BusinessLogic.Entities.Warehouse>(w => w.LocationCoordinates == null)
            )).Throws(new BusinessLogic.Entities.BLException("Invalid Warehouse", new System.Exception()));

            var loggerMock = new Mock<ILogger<WarehouseManagementApiController>>();

            _warehouseManagementApiController = new WarehouseManagementApiController(mapper, mock.Object, loggerMock.Object);
        }

        [Test]
        public void Should_Return_Ok_On_Export_Warehouses()
        {
            var res = _warehouseManagementApiController.ExportWarehouses();
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<OkObjectResult>(res);
            Assert.IsInstanceOf<Services.DTOs.Warehouse>((res as OkObjectResult).Value);
        }

        [Test]
        public void Should_Return_Ok_On_Get_Warehouse()
        {
            var res = _warehouseManagementApiController.GetWarehouse(_testCode);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<OkObjectResult>(res);
            Assert.IsInstanceOf<Services.DTOs.Warehouse>((res as OkObjectResult).Value);
        }

        [Test]
        public void Should_Return_Not_Found_On_Get_Warehouse()
        {
            var res = _warehouseManagementApiController.GetWarehouse(_notFoundCode);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<NotFoundObjectResult>(res);
            Assert.IsInstanceOf<Services.DTOs.Error>((res as NotFoundObjectResult).Value);
        }

        [Test]
        public void Should_Return_Bad_Request_On_Get_Warehouse_Of_Null_Code()
        {
            var res = _warehouseManagementApiController.GetWarehouse(null);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<BadRequestObjectResult>(res);
        }

        [Test]
        public void Should_Return_Bad_Request_On_Get_Warehouse_Of_Invalid_Code()
        {
            var res = _warehouseManagementApiController.GetWarehouse(_invalidCode);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<BadRequestObjectResult>(res);
        }

        [Test]
        public void Should_Return_Ok_On_Import_Warehouses()
        {
            var res = _warehouseManagementApiController.ImportWarehouses(_testWarehouse);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<OkObjectResult>(res);
        }

        [Test]
        public void Should_Return_Bad_Request_On_Import_Null_Warehouses()
        {
            var res = _warehouseManagementApiController.ImportWarehouses(null);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<BadRequestObjectResult>(res);
        }

        [Test]
        public void Should_Return_Bad_Request_On_Import_Invalid_Warehouses()
        {
            _testWarehouse.LocationCoordinates = null;
            var res = _warehouseManagementApiController.ImportWarehouses(_testWarehouse);
            Assert.IsNotNull(res);
            Assert.IsInstanceOf<BadRequestObjectResult>(res);
        }
    }
}